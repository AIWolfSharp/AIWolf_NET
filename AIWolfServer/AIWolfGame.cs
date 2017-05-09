//
// AIWolfGame.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using AIWolf.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AIWolf.Server
{
#if JHELP
    /// <summary>
    /// 人狼知能ゲームクラス
    /// </summary>    
#else
    /// <summary>
    /// AIWolf game class.
    /// </summary>
#endif
    public class AIWolfGame
    {
        string logFileName;

#if JHELP
        /// <summary>
        /// 乱数ジェネレーター
        /// </summary>
#else
        /// <summary>
        /// The random generator.
        /// </summary>
#endif
        public Random Rand { get; set; } = new Random();

        GameSetting GameSetting { get; }
        IGameServer GameServer { get; }
        GameData GameData { get; set; }

#if JHELP
        /// <summary>
        /// コンソールにログを表示するか否か
        /// </summary>
#else
        /// <summary>
        /// Whether or not the console logs are shown.
        /// </summary>
#endif
        bool ShowConsoleLog { get; set; } = true;

#if JHELP
        /// <summary>
        /// ロガー
        /// </summary>
#else
        /// <summary>
        /// The logger.
        /// </summary>
#endif
        public FileGameLogger GameLogger { get; set; }

        Dictionary<Agent, string> AgentNameMap { get; set; }

        string LogFileName
        {
            get => logFileName;
            set => GameLogger = new FileGameLogger(logFileName = value);
        }

        int Day => GameData.Day;
        List<Agent> AgentList => GameData.AgentList;
        List<Agent> OrderedAgentList => AgentList.OrderBy(x => x.AgentIdx).ToList();
        List<Agent> AliveAgentList => AgentList.Where(a => StatusOf(a) == Status.ALIVE).ToList();
        List<Agent> AliveHumanList => AliveAgentList.Where(a => RoleOf(a).GetSpecies() == Species.HUMAN).ToList();
        List<Agent> AliveWolfList => AliveAgentList.Where(a => RoleOf(a) == Role.WEREWOLF).ToList();
        bool GameFinished => GetWinner() != Team.UNC;


#if JHELP
        /// <summary>
        /// AIWolfGameクラスの新しいインスタンスを初期化する
        /// </summary>
        /// <param name="gameSetting">ゲーム設定</param>
        /// <param name="gameServer">ゲームサーバ</param>
#else
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="gameSetting">The setting of this game.</param>
        /// <param name="gameServer">The game server.</param>
#endif
        public AIWolfGame(GameSetting gameSetting, IGameServer gameServer)
        {
            GameSetting = gameSetting;
            GameServer = gameServer;
        }

#if JHELP
        /// <summary>
        /// ゲームを初期化する
        /// </summary>
#else
        /// <summary>
        /// Initializes the game.
        /// </summary>
#endif
        void Init()
        {
            GameData = new GameData(GameSetting);
            AgentNameMap = new Dictionary<Agent, string>();
            GameServer.GameData = GameData;

            List<Agent> agentList = GameServer.ConnectedAgentList;

            if (agentList.Count != GameSetting.PlayerNum)
            {
                throw new Exception("Player num is " + GameSetting.PlayerNum + " but connected agent is " + agentList.Count);
            }
            agentList = agentList.Shuffle().ToList();

            Dictionary<Role, List<Agent>> requestRoleMap = new Dictionary<Role, List<Agent>>();
            foreach (Role role in Enum.GetValues(typeof(Role)))
            {
                if (role != Role.UNC)
                {
                    requestRoleMap[role] = new List<Agent>();
                }
            }
            List<Agent> noRequestAgentList = new List<Agent>();
            foreach (Agent agent in agentList)
            {
                Role requestedRole = GameServer.RequestRequestRole(agent);
                if (requestedRole != Role.UNC)
                {
                    if (requestRoleMap[requestedRole].Count < GameSetting.RoleNumMap[requestedRole])
                    {
                        requestRoleMap[requestedRole].Add(agent);
                    }
                    else
                    {
                        noRequestAgentList.Add(agent);
                    }
                }
                else
                {
                    noRequestAgentList.Add(agent);
                }
            }

            foreach (Role role in Enum.GetValues(typeof(Role)))
            {
                if (role == Role.UNC)
                {
                    continue;
                }
                List<Agent> requestedAgentList = requestRoleMap[role];
                for (int i = 0; i < GameSetting.RoleNumMap[role]; i++)
                {
                    if (requestedAgentList.Count == 0)
                    {
                        GameData.AddAgent(noRequestAgentList[0], Status.ALIVE, role);
                        noRequestAgentList.RemoveAt(0);
                    }
                    else
                    {
                        GameData.AddAgent(requestedAgentList[0], Status.ALIVE, role);
                        requestedAgentList.RemoveAt(0);
                    }
                }
            }

            GameServer.GameSetting = GameSetting;
            foreach (Agent agent in agentList)
            {
                GameServer.Init(agent);
                AgentNameMap[agent] = GameServer.RequestName(agent);
            }
        }

        /// <summary>
        /// Starts the game.
        /// </summary>
        public void Start()
        {
            try
            {
                Init();

                while (!GameFinished)
                {
                    ConsoleLog();
                    // Day phase.
                    DayStart();
                    if (Day == 0)
                    {
                        if (GameSetting.TalkOnFirstDay)
                        {
                            Whisper();
                            Talk();
                        }
                    }
                    else
                    {
                        Talk();
                    }
                    // Night phase.
                    Night();
                    if (GameLogger != null)
                    {
                        GameLogger.Flush();
                    }
                }
                ConsoleLog();
                Finish();

                if (ShowConsoleLog)
                {
                    Console.WriteLine("Winner:" + GetWinner());
                }
            }
            catch (LostClientException e)
            {
                if (GameLogger != null)
                {
                    GameLogger.Log("Lost Connection of " + e.Agent);
                }
                throw e;
            }
        }

        /// <summary>
        /// Finishes the game.
        /// </summary>
        void Finish()
        {
            if (GameLogger != null)
            {
                foreach (Agent agent in OrderedAgentList)
                {
                    GameLogger.Log(string.Format("{0},status,{1},{2},{3},{4}", Day, agent.AgentIdx, RoleOf(agent), StatusOf(agent), AgentNameMap[agent]));
                }
                GameLogger.Log(string.Format("{0},result,{1},{2},{3}", Day, AliveHumanList.Count, AliveWolfList.Count, GetWinner()));
                GameLogger.Close();
            }

            foreach (Agent agent in AgentList)
            {
                GameServer.Finish(agent);
            }
        }

        /// <summary>
        /// Returns the team which wins the game.
        /// </summary>
        /// <returns>The team which wins.</returns>
        Team GetWinner()
        {
            int humanSide = 0;
            int wolfSide = 0;
            int otherSide = 0;
            foreach (Agent agent in AliveAgentList)
            {
                switch (RoleOf(agent))
                {
                    case Role.FOX:
                        otherSide++;
                        break;
                    case Role.WEREWOLF:
                        wolfSide++;
                        break;
                    default:
                        humanSide++;
                        break;
                }
            }
            if (wolfSide == 0)
            {
                if (otherSide > 0)
                {
                    return Team.OTHERS;
                }
                return Team.VILLAGER;
            }
            else if (humanSide <= wolfSide)
            {
                if (otherSide > 0)
                {
                    return Team.OTHERS;
                }
                return Team.WEREWOLF;
            }
            return Team.UNC;
        }

        void ConsoleLog()
        {
            if (!ShowConsoleLog)
            {
                return;
            }

            GameData yesterday = GameData.DayBefore;

            Console.WriteLine("=============================================");
            if (yesterday != null)
            {
                Console.WriteLine("Day {0:00}", yesterday.Day);
                Console.WriteLine("========talk========");
                foreach (Talk talk in yesterday.TalkList)
                {
                    Console.WriteLine(talk);
                }
                Console.WriteLine("========Whisper========");
                foreach (Whisper whisper in yesterday.WhisperList)
                {
                    Console.WriteLine(whisper);
                }

                Console.WriteLine("========Actions========");
                foreach (Vote vote in yesterday.VoteList)
                {
                    Console.WriteLine("Vote:{0}->{1}", vote.Agent, vote.Target);
                }

                foreach (Vote vote in yesterday.AttackVoteList)
                {
                    Console.WriteLine("AttackVote:{0}->{1}", vote.Agent, vote.Target);
                }

                Console.WriteLine("{0} executed", yesterday.Executed);

                Judge divine = yesterday.Divine;
                if (divine != null)
                {
                    Console.WriteLine("{0} divine {1}. Result is {2}", divine.Agent, divine.Target, divine.Result);
                }

                if (yesterday.Guard != null)
                {
                    Console.WriteLine("{0} guarded", yesterday.Guard);
                }

                if (yesterday.AttackedDead != null)
                {
                    Console.WriteLine("{0} attacked", yesterday.AttackedDead);
                }

                if (yesterday.CursedFox != null)
                {
                    Console.WriteLine("{0} cursed", yesterday.CursedFox);
                }
            }
            Console.WriteLine("======");
            foreach (Agent agent in OrderedAgentList)
            {
                Console.Write("{0}\t{1}\t{2}\t{3}", agent, AgentNameMap[agent], StatusOf(agent), RoleOf(agent));
                if (yesterday != null)
                {
                    if (agent == yesterday.Executed)
                    {
                        Console.Write("\texecuted");
                    }
                    if (agent == yesterday.AttackedDead)
                    {
                        Console.Write("\tattacked");
                    }
                    Judge divine = yesterday.Divine;
                    if (divine != null && agent == divine.Target)
                    {
                        Console.Write("\tdivined");
                    }
                    Guard guard = yesterday.Guard;
                    if (guard != null && agent == guard.Target)
                    {
                        Console.Write("\tguarded");
                    }
                    if (agent == yesterday.CursedFox)
                    {
                        Console.Write("\tcursed");
                    }
                }
                Console.WriteLine();
            }
            Console.Write("Human:{0}\nWerewolf:{1}\n", AliveHumanList.Count, AliveWolfList.Count);
            if (GameSetting.RoleNumMap[Role.FOX] != 0)
            {
                Console.Write("Others:{0}\n", AliveAgentList.Where(a => RoleOf(a).GetTeam() == Team.OTHERS).Count());
            }
            Console.WriteLine("=============================================");
        }

        void Night()
        {
            foreach (Agent agent in AgentList)
            {
                GameServer.DayFinish(agent);
            }

            if (!GameSetting.TalkOnFirstDay && Day == 0)
            {
                Whisper();
            }

            Agent executed = null;
            List<Agent> candidates = null;
            if (Day != 0)
            {
                for (int i = 0; i <= GameSetting.MaxRevote; i++)
                {
                    Vote();
                    candidates = GetVotedCandidates(GameData.VoteList);
                    if (candidates.Count == 1)
                    {
                        executed = candidates[0];
                        break;
                    }
                }

                // In case of multiple candidates.
                if (executed == null && !GameSetting.EnableNoExecution)
                {
                    executed = candidates.Shuffle().First();
                }

                if (executed != null)
                {
                    GameData.Executed = executed;
                    if (GameLogger != null)
                    {
                        GameLogger.Log(string.Format("{0},execute,{1},{2}", Day, executed.AgentIdx, RoleOf(executed)));
                    }
                }
            }

            Divine();

            if (Day != 0)
            {
                Whisper();
                Guard();

                Agent attacked = null;
                if (!(AliveWolfList.Count == 1 && RoleOf(GameData.Executed) == Role.WEREWOLF))
                {
                    for (int i = 0; i <= GameSetting.MaxAttackRevote; i++)
                    {
                        if (i > 0 && GameSetting.WhisperBeforeRevote)
                        {
                            Whisper();
                        }
                        AttackVote();
                        candidates = GetAttackVotedCandidates(GameData.AttackVoteList.Where(v => v.Agent != executed).ToList());
                        if (candidates.Count == 1)
                        {
                            attacked = candidates[0];
                            break;
                        }
                    }

                    // In case of multiple candidates.
                    if (attacked == null && !GameSetting.EnableNoAttack)
                    {
                        attacked = candidates.Shuffle().First();
                    }

                    GameData.Attacked = attacked;

                    bool guarded = false;
                    if (GameData.Guard != null)
                    {
                        if (GameData.Guard.Target == attacked && attacked != null)
                        {
                            if (GameData.Executed == null || GameData.Executed != GameData.Guard.Agent)
                            {
                                guarded = true;
                            }
                        }
                    }
                    if (!guarded && attacked != null && RoleOf(attacked) != Role.FOX)
                    {
                        GameData.AttackedDead = attacked;
                        GameData.AddLastDeadAgent(attacked);
                        if (GameLogger != null)
                        {
                            GameLogger.Log(string.Format("{0},attack,{1},true", Day, attacked.AgentIdx));
                        }
                    }
                    else if (attacked != null)
                    {
                        if (GameLogger != null)
                        {
                            GameLogger.Log(string.Format("{0},attack,{1},false", Day, attacked.AgentIdx));
                        }
                    }
                    else
                    {
                        if (GameLogger != null)
                        {
                            GameLogger.Log(string.Format("{0},attack,-1,false", Day));
                        }

                    }

                }

            }

            GameData = GameData.NextDay;
            GameServer.GameData = GameData;
        }

        List<Agent> GetVotedCandidates(List<Vote> voteList)
        {
            Dictionary<Agent, int> counter = new Dictionary<Agent, int>();
            foreach (Vote vote in voteList)
            {
                if (StatusOf(vote.Target) == Status.ALIVE)
                {
                    if (counter.ContainsKey(vote.Target))
                    {
                        counter[vote.Target]++;
                    }
                    else
                    {
                        counter[vote.Target] = 1;
                    }
                }
            }
            int max = 0;
            if (counter.Count > 0)
            {
                max = counter.OrderBy(p => p.Value).Last().Value;
            }
            return counter.Keys.Where(a => counter[a] == max).ToList();
        }

        List<Agent> GetAttackVotedCandidates(List<Vote> voteList)
        {
            Dictionary<Agent, int> counter = new Dictionary<Agent, int>();
            foreach (Vote vote in voteList)
            {
                if (StatusOf(vote.Target) == Status.ALIVE && RoleOf(vote.Target) != Role.WEREWOLF)
                {
                    if (counter.ContainsKey(vote.Target))
                    {
                        counter[vote.Target]++;
                    }
                    else
                    {
                        counter[vote.Target] = 1;
                    }
                }
            }
            if (!GameSetting.EnableNoAttack)
            {
                foreach (Agent agent in AliveHumanList)
                {
                    if (counter.ContainsKey(agent))
                    {
                        counter[agent]++;
                    }
                    else
                    {
                        counter[agent] = 1;
                    }
                }
            }

            int max = counter.OrderBy(p => p.Value).Last().Value;
            return counter.Keys.Where(a => counter[a] == max).ToList();
        }

        void DayStart()
        {
            if (GameLogger != null)
            {
                foreach (Agent agent in OrderedAgentList)
                {
                    GameLogger.Log(string.Format("{0},status,{1},{2},{3},{4}", Day, agent.AgentIdx, RoleOf(agent), StatusOf(agent), AgentNameMap[agent]));
                }
            }

            foreach (Agent agent in AliveAgentList)
            {
                GameServer.DayStart(agent);
            }
        }

        void Talk()
        {
            foreach (Agent agent in AliveAgentList)
            {
                GameData.RemainTalkMap[agent] = GameSetting.MaxTalk;
            }

            Dictionary<Agent, int> skipCounter = new Dictionary<Agent, int>();
            for (int turn = 0; turn < GameSetting.MaxTalkTurn; turn++)
            {
                List<Talk> talkList = new List<Talk>();
                foreach (Agent agent in AliveAgentList.Shuffle())
                {
                    string text = Utterance.OVER;
                    if (GameData.RemainTalkMap[agent] > 0)
                    {
                        text = GameServer.RequestTalk(agent);
                    }
                    if (text == null || text.Length == 0)
                    {
                        text = Utterance.SKIP;
                    }
                    else
                    {
                        text = StripText(text);
                    }
                    if (text == Utterance.SKIP)
                    {
                        if (skipCounter.ContainsKey(agent))
                        {
                            skipCounter[agent]++;
                        }
                        else
                        {
                            skipCounter[agent] = 1;
                        }
                        if (skipCounter[agent] > GameSetting.MaxSkip)
                        {
                            text = Utterance.OVER;
                        }
                    }
                    talkList.Add(new Talk(GameData.NextTalkIdx, Day, turn, agent, text));
                    if (text != Utterance.OVER && text != Utterance.SKIP)
                    {
                        skipCounter[agent] = 0;
                    }
                }
                bool continueTalk = false;
                foreach (Talk talk in talkList)
                {
                    GameData.AddTalk(talk.Agent, talk);
                    if (GameLogger != null)
                    {
                        GameLogger.Log(string.Format("{0},talk,{1},{2},{3},{4}", Day, talk.Idx, talk.Turn, talk.Agent.AgentIdx, talk.Text));
                    }
                    if (talk.Text != Utterance.OVER)
                    {
                        continueTalk = true;
                    }
                }
                if (!continueTalk)
                {
                    break;
                }
            }
        }

        void Whisper()
        {
            // No whisper in case of solo werewolf.
            if (AliveWolfList.Count == 1)
            {
                return;
            }
            foreach (Agent agent in AliveWolfList)
            {
                GameData.RemainWhisperMap[agent] = GameSetting.MaxWhisper;
            }

            Dictionary<Agent, int> skipCounter = new Dictionary<Agent, int>();
            for (int turn = 0; turn < GameSetting.MaxWhisperTurn; turn++)
            {
                List<Whisper> whisperList = new List<Whisper>();
                foreach (Agent agent in AliveWolfList.Shuffle())
                {
                    string text = Utterance.OVER;
                    if (GameData.RemainWhisperMap[agent] > 0)
                    {
                        text = GameServer.RequestWhisper(agent);
                    }
                    if (text == null || text.Length == 0)
                    {
                        text = Utterance.SKIP;
                    }
                    else
                    {
                        text = StripText(text);
                    }
                    if (text == Utterance.SKIP)
                    {
                        if (skipCounter.ContainsKey(agent))
                        {
                            skipCounter[agent]++;
                        }
                        else
                        {
                            skipCounter[agent] = 1;
                        }
                        if (skipCounter[agent] > GameSetting.MaxSkip)
                        {
                            text = Utterance.OVER;
                        }
                    }
                    whisperList.Add(new Whisper(GameData.NextWhisperIdx, Day, turn, agent, text));
                    if (text != Utterance.OVER && text != Utterance.SKIP)
                    {
                        skipCounter[agent] = 0;
                    }
                }
                bool continueWhisper = false;
                foreach (Whisper whisper in whisperList)
                {
                    GameData.AddWhisper(whisper.Agent, whisper);
                    if (GameLogger != null)
                    {
                        GameLogger.Log(string.Format("{0},whisper,{1},{2},{3},{4}", Day, whisper.Idx, whisper.Turn, whisper.Agent.AgentIdx, whisper.Text));
                    }
                    if (whisper.Text != Utterance.OVER)
                    {
                        continueWhisper = true;
                    }
                }
                if (!continueWhisper)
                {
                    break;
                }
            }
        }

        void Vote()
        {
            GameData.VoteList.Clear();
            foreach (Agent agent in AliveAgentList)
            {
                Agent target = GameServer.RequestVote(agent);
                if (StatusOf(target) == Status.DEAD || target == null || agent == target)
                {
                    target = AliveAgentList.Where(a => a != agent).Shuffle().First();
                }
                Vote vote = new Vote(Day, agent, target);
                GameData.VoteList.Add(vote);
                if (GameLogger != null)
                {
                    GameLogger.Log(string.Format("{0},vote,{1},{2}", Day, agent.AgentIdx, target.AgentIdx));
                }
            }
            GameData.LatestVoteList = new List<Vote>(GameData.VoteList);
        }

        void Divine()
        {
            foreach (Agent agent in AliveAgentList.Where(a => RoleOf(a) == Role.SEER))
            {
                Agent target = GameServer.RequestDivineTarget(agent);
                Role targetRole = RoleOf(target);
                if (StatusOf(target) == Status.DEAD || target == null || targetRole == Role.UNC)
                {
                    // Do nothing.
                }
                else
                {
                    Judge divine = new Judge(Day, agent, target, targetRole.GetSpecies());
                    GameData.Divine = divine;

                    if (targetRole == Role.FOX)
                    {
                        GameData.AddLastDeadAgent(target);
                        GameData.CursedFox = target;
                    }

                    if (GameLogger != null)
                    {
                        GameLogger.Log(string.Format("{0},divine,{1},{2},{3}", Day, agent.AgentIdx, target.AgentIdx, divine.Result));
                    }
                }
            }
        }

        void Guard()
        {
            foreach (Agent agent in AliveAgentList.Where(a => RoleOf(a) == Role.BODYGUARD))
            {
                if (agent == GameData.Executed)
                {
                    continue;
                }
                Agent target = GameServer.RequestGuardTarget(agent);
                if (target == null || agent == target)
                {
                    // Do nothing.
                }
                else
                {
                    Guard guard = new Guard(Day, agent, target);
                    GameData.Guard = guard;

                    if (GameLogger != null)
                    {
                        GameLogger.Log(string.Format("{0},guard,{1},{2},{3}", Day, agent.AgentIdx, target.AgentIdx, RoleOf(target)));
                    }
                }
            }
        }

        Role RoleOf(Agent agent) => GameData.RoleMap[agent];
        Status StatusOf(Agent agent) => GameData.StatusMap[agent];

        void AttackVote()
        {
            GameData.AttackVoteList.Clear();
            foreach (Agent agent in AliveWolfList)
            {
                Agent target = GameServer.RequestAttackTarget(agent);
                if (StatusOf(target) == Status.DEAD || RoleOf(target) == Role.WEREWOLF || target == null)
                {
                    // Do nothing.
                }
                else
                {
                    Vote attackVote = new Vote(Day, agent, target);
                    GameData.AttackVoteList.Add(attackVote);

                    if (GameLogger != null)
                    {
                        GameLogger.Log(string.Format("{0},attackVote,{1},{2}", Day, attackVote.Agent.AgentIdx, attackVote.Target.AgentIdx));
                    }
                }
            }
            GameData.LatestAttackVoteList = new List<Vote>(GameData.AttackVoteList);
        }

        static readonly Regex regexStripText = new Regex(@"^Agent\[.+?\] (.+)");

        string StripText(string text)
        {
            var m = regexStripText.Match(text);
            if (m.Success)
            {
                return m.Groups[1].ToString();
            }
            return text;
        }
    }
}
