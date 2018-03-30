//
// GameData.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using AIWolf.Lib;
using System.Collections.Generic;
using System.Linq;

namespace AIWolf.Server
{
    public class GameData
    {
        int talkIdx = 0;
        int whisperIdx = 0;
        Agent attackedDead;
        Agent cursedFox;
        Agent executed;
        GameSetting gameSetting;

        /// <summary>
        /// The day.
        /// </summary>
        public int Day { get; private set; }

        /// <summary>
        /// The map between the agent and its status.
        /// </summary>
        public IDictionary<Agent, Status> StatusMap { get; private set; } = new Dictionary<Agent, Status>();

        /// <summary>
        /// The map between the agent and its role.
        /// </summary>
        public IDictionary<Agent, Role> RoleMap { get; private set; } = new Dictionary<Agent, Role>();

        /// <summary>
        /// The list of talks.
        /// </summary>
        public IList<Talk> TalkList { get; } = new List<Talk>();

        /// <summary>
        /// The list of whispers.
        /// </summary>
        public IList<Whisper> WhisperList { get; } = new List<Whisper>();

        /// <summary>
        /// The list of votes.
        /// </summary>
        public IList<Vote> VoteList { get; } = new List<Vote>();

        /// <summary>
        /// The list of the latest votes.
        /// </summary>
        public IList<Vote> LatestVoteList { get; set; } = new List<Vote>();

        /// <summary>
        /// The list of votes for attack.
        /// </summary>
        public IList<Vote> AttackVoteList { get; } = new List<Vote>();

        /// <summary>
        /// The list of the latest votes for attack.
        /// </summary>
        public IList<Vote> LatestAttackVoteList { get; set; } = new List<Vote>();

        /// <summary>
        /// The map between the agent and its chances of talk remaining.
        /// </summary>
        public IDictionary<Agent, int> RemainTalkMap { get; } = new Dictionary<Agent, int>();

        /// <summary>
        /// Thae map between the agent and its chances of whispers remaining.
        /// </summary>
        public IDictionary<Agent, int> RemainWhisperMap { get; } = new Dictionary<Agent, int>();

        /// <summary>
        /// The latest divination.
        /// </summary>
        public Judge Divine { get; set; }

        /// <summary>
        /// The latest guard.
        /// </summary>
        public Guard Guard { get; set; }

        /// <summary>
        /// The agent executed yesterday.
        /// </summary>
        public Agent Executed
        {
            get => executed;
            set => executed = SetDead(value);
        }

        /// <summary>
        /// The agent killed by the wolf yesterday.
        /// </summary>
        public Agent AttackedDead
        {
            get => attackedDead;
            set => attackedDead = SetDead(value);
        }

        /// <summary>
        /// The agent attacked by the wolf yesterday. (No matter whether or not the attack succeeded.)
        /// </summary>
        public Agent Attacked { get; set; }

        /// <summary>
        /// The fox killed by curse yesterday.
        /// </summary>
        public Agent CursedFox
        {
            get => cursedFox;
            set => cursedFox = SetDead(value);
        }

        /// <summary>
        /// The list of agents that died yesterday.
        /// </summary>
        IList<Agent> LastDeadAgentList { get; } = new List<Agent>();

        /// <summary>
        /// The game data of yesterday.
        /// </summary>
        public GameData DayBefore { get; private set; }

        /// <summary>
        /// The list of agents.
        /// </summary>
        public IList<Agent> AgentList => new List<Agent>(RoleMap.Keys);

        /// <summary>
        /// GameData for tomorrow.
        /// </summary>
        public GameData NextDay
        {
            get
            {
                GameData gameData = new GameData(gameSetting)
                {
                    Day = Day + 1,
                    StatusMap = new Dictionary<Agent, Status>(StatusMap),
                    RoleMap = new Dictionary<Agent, Role>(RoleMap),
                    DayBefore = this
                };
                foreach (Agent agent in gameData.AgentList.Where(a => StatusMap[a] == Status.ALIVE))
                {
                    gameData.RemainTalkMap[agent] = gameSetting.MaxTalk;
                    if (RoleMap[agent] == Role.WEREWOLF)
                    {
                        gameData.RemainWhisperMap[agent] = gameSetting.MaxWhisper;
                    }
                }
                return gameData;
            }
        }

        public int NextTalkIdx => talkIdx++;
        public int NextWhisperIdx => whisperIdx++;

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="gameSetting">The setting of the game.</param>
        public GameData(GameSetting gameSetting)
        {
            this.gameSetting = gameSetting;
        }

        /// <summary>
        /// Generates a GameInfo.
        /// </summary>
        /// <param name="agent">The owner of the GameInfo.</param>
        /// <returns>The instance of GameInfo.</returns>
        /// <remarks>If agent is null, stuff the GameInfo with the all information available.</remarks>
        public GameInfo GetGameInfo(Agent agent)
        {
            Role role = agent != null ? RoleMap[agent] : Role.UNC;
            GameInfo gameInfo = new GameInfo()
            {
                Agent = agent,
                LatestVoteList = gameSetting.VoteVisible ? LatestVoteList : null,
                LatestExecutedAgent = Executed,
                LatestAttackVoteList = agent == null || role == Role.WEREWOLF ? LatestAttackVoteList : null,
                TalkList = TalkList,
                StatusMap = StatusMap,
                ExistingRoleList = RoleMap.Values.Distinct().ToList(),
                RemainTalkMap = RemainTalkMap,
                RemainWhisperMap = role == Role.WEREWOLF ? RemainWhisperMap : null,
                WhisperList = agent == null || role == Role.WEREWOLF ? WhisperList : null,
                Day = Day
            };
            if (role == Role.WEREWOLF)
            {
                gameInfo.RoleMap = AgentList.Where(a => RoleMap[a] == Role.WEREWOLF).ToDictionary(a => a, a => Role.WEREWOLF);
            }
            else if (role == Role.FREEMASON)
            {
                gameInfo.RoleMap = AgentList.Where(a => RoleMap[a] == Role.FREEMASON).ToDictionary(a => a, a => Role.FREEMASON);
            }
            else
            {
                gameInfo.RoleMap = new Dictionary<Agent, Role>() { { agent, role } };
            }

            if (DayBefore != null)
            {
                gameInfo.ExecutedAgent = DayBefore.Executed;
                gameInfo.LastDeadAgentList = DayBefore.LastDeadAgentList;
                gameInfo.VoteList = gameSetting.VoteVisible ? DayBefore.VoteList : null;
                gameInfo.MediumResult = role == Role.MEDIUM && DayBefore.Executed != null
                    ? new Judge(Day, agent, DayBefore.Executed, DayBefore.RoleMap[DayBefore.Executed].GetSpecies()) : null;
                gameInfo.DivineResult = agent == null || role == Role.SEER ? DayBefore.Divine : null;
                if (agent == null || role == Role.WEREWOLF)
                {
                    gameInfo.AttackedAgent = DayBefore.Attacked;
                    gameInfo.AttackVoteList = DayBefore.AttackVoteList;
                }
                gameInfo.GuardedAgent = (agent == null || role == Role.BODYGUARD) && DayBefore.Guard != null ? DayBefore.Guard.Target : null;
                gameInfo.CursedFox = agent == null ? DayBefore.CursedFox : null;
            }

            return gameInfo;
        }

        /// <summary>
        /// Generates the GameInfo containing the complete agent-role mapping.
        /// </summary>
        /// <param name="agent">The owner of the GameInfo.</param>
        /// <returns>The instance of GameInfo.</returns>
        /// <remarks>If agent is null, stuff the GameInfo with the all information available.</remarks>
        public GameInfo GetFinalGameInfo(Agent agent)
        {
            GameInfo gameInfo = GetGameInfo(agent);
            gameInfo.RoleMap = RoleMap;
            return gameInfo;
        }

        /// <summary>
        /// Adds a new agent to the game.
        /// </summary>
        /// <param name="agent">The agent to be added.</param>
        /// <param name="status">The agent's status.</param>
        /// <param name="role">The agent's role.</param>
        public void AddAgent(Agent agent, Status status, Role role)
        {
            RoleMap[agent] = role;
            StatusMap[agent] = status;
            RemainTalkMap[agent] = gameSetting.MaxTalk;
            if (role == Role.WEREWOLF)
            {
                RemainWhisperMap[agent] = gameSetting.MaxWhisper;
            }
        }

        /// <summary>
        /// Adds a talk to TalkList.
        /// </summary>
        /// <param name="agent">The talker.</param>
        /// <param name="talk">The uttered talk.</param>
        public void AddTalk(Agent agent, Talk talk)
        {
            if (talk.Text != Talk.OVER && talk.Text != Talk.SKIP)
            {
                if (RemainTalkMap[agent] == 0)
                {
                    throw new System.Exception(agent + " has no chance to talk remaining.");
                }
                RemainTalkMap[agent]--;
            }
            TalkList.Add(talk);
        }

        /// <summary>
        /// Adds a whisper to WhisperList.
        /// </summary>
        /// <param name="agent">The whisperer.</param>
        /// <param name="whisper">The uttered whisper.</param>
        public void AddWhisper(Agent agent, Whisper whisper)
        {
            if (whisper.Text != Talk.OVER && whisper.Text != Talk.SKIP)
            {
                if (RemainWhisperMap[agent] == 0)
                {
                    throw new System.Exception(agent + " has no chance to whisper remaining.");
                }
                RemainWhisperMap[agent]--;
            }
            WhisperList.Add(whisper);
        }

        /// <summary>
        /// Adds a agent to LastDeadAgentList.
        /// </summary>
        /// <param name="agent">The agent to be added.</param>
        public void AddLastDeadAgent(Agent agent)
        {
            if (!LastDeadAgentList.Contains(agent))
            {
                LastDeadAgentList.Add(SetDead(agent));
            }
        }

        Agent SetDead(Agent agent)
        {
            if (agent != null)
            {
                StatusMap[agent] = Status.DEAD;
            }
            return agent;
        }
    }
}
