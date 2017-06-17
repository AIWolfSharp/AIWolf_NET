//
// GameInfo.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace AIWolf.Lib
{
#if JHELP
    /// <summary>
    /// ゲーム情報
    /// </summary>
#else
    /// <summary>
    /// Game information.
    /// </summary>
#endif
    [DataContract]
    public class GameInfo
    {
#if JHELP
        /// <summary>
        /// このゲーム情報を受け取るエージェント
        /// </summary>
#else
        /// <summary>
        /// The agent who receives this GameInfo.
        /// </summary>
#endif
        public Agent Agent
        {
            get => _agent;
            set
            {
                _agent = value;
                agent = value == null ? -1 : value.AgentIdx;
            }
        }

        [DataMember(Name = "agent")]
        int agent;
        Agent _agent;

#if JHELP
        /// <summary>
        /// 昨夜追放されたエージェント
        /// </summary>
#else
        /// <summary>
        /// The agent executed last night.
        /// </summary>
#endif
        public Agent ExecutedAgent
        {
            get => _executedAgent;
            set
            {
                _executedAgent = value;
                executedAgent = value == null ? -1 : value.AgentIdx;
            }
        }

        [DataMember(Name = "executedAgent")]
        int executedAgent;
        Agent _executedAgent;

#if JHELP
        /// <summary>
        /// 直近に追放されたエージェント
        /// </summary>
#else
        /// <summary>
        /// The latest executed agent.
        /// </summary>
#endif
        public Agent LatestExecutedAgent
        {
            get => _latestExecutedAgent;
            set
            {
                _latestExecutedAgent = value;
                latestExecutedAgent = value == null ? -1 : value.AgentIdx;
            }
        }

        [DataMember(Name = "latestExecutedAgent")]
        int latestExecutedAgent;
        Agent _latestExecutedAgent;

#if JHELP
        /// <summary>
        /// 呪殺された妖狐
        /// </summary>
#else
        /// <summary>
        /// The fox killed by curse.
        /// </summary>
#endif
        public Agent CursedFox
        {
            get => _cursedFox;
            set
            {
                _cursedFox = value;
                cursedFox = value == null ? -1 : value.AgentIdx;
            }
        }

        [DataMember(Name = "cursedFox")]
        int cursedFox;
        Agent _cursedFox;

#if JHELP
        /// <summary>
        /// 人狼による投票の結果襲撃先に決まったエージェント
        /// </summary>
        /// <remarks>人狼限定</remarks>
#else
        /// <summary>
        /// The agent decided to be attacked as a result of werewolves' vote.
        /// </summary>
        /// <remarks>Werewolf only.</remarks>
#endif
        public Agent AttackedAgent
        {
            get => _attackedAgent;
            set
            {
                _attackedAgent = value;
                attackedAgent = value == null ? -1 : value.AgentIdx;
            }
        }

        [DataMember(Name = "attackedAgent")]
        int attackedAgent;
        Agent _attackedAgent;

#if JHELP
        /// <summary>
        /// 昨夜護衛されたエージェント
        /// </summary>
#else
        /// <summary>
        /// The agent guarded last night.
        /// </summary>
#endif
        public Agent GuardedAgent
        {
            get => _guardedAgent;
            set
            {
                _guardedAgent = value;
                guardedAgent = value == null ? -1 : value.AgentIdx;
            }
        }

        [DataMember(Name = "guardedAgent")]
        int guardedAgent;
        Agent _guardedAgent;

#if JHELP
        /// <summary>
        /// 全エージェントの生死状況
        /// </summary>
#else
        /// <summary>
        /// The statuses of all agents.
        /// </summary>
#endif
        public Dictionary<Agent, Status> StatusMap
        {
            get => _statusMap;
            set
            {
                _statusMap = value;
                statusMap = value == null ? new Dictionary<int, string>() : value.ToDictionary(p => p.Key.AgentIdx, p => p.Value.ToString());
                AgentList = value == null ? new List<Agent>() : _statusMap.Keys.ToList();
            }
        }

        [DataMember(Name = "statusMap")]
        Dictionary<int, string> statusMap;
        Dictionary<Agent, Status> _statusMap;

#if JHELP
        /// <summary>
        /// 役職既知のエージェント
        /// </summary>
        /// <remarks>
        /// 人間の場合，自分自身しかわからない
        /// 人狼の場合，誰が他の人狼かがわかる
        /// </remarks>
#else
        /// <summary>
        /// The known roles of agents.
        /// </summary>
        /// <remarks>
        /// If you are human, you know only yourself.
        /// If you are werewolf, you know other werewolves.
        /// </remarks>
#endif
        public Dictionary<Agent, Role> RoleMap
        {
            get => _roleMap;
            set
            {
                _roleMap = value;
                roleMap = value == null ? new Dictionary<int, string>() : value.ToDictionary(p => p.Key.AgentIdx, p => p.Value.ToString());
                Role = RoleMap.ContainsKey(Agent) ? RoleMap[Agent] : Role.UNC;
            }
        }

        [DataMember(Name = "roleMap")]
        Dictionary<int, string> roleMap;
        Dictionary<Agent, Role> _roleMap;

#if JHELP
        /// <summary>
        /// トークの残り回数
        /// </summary>
#else
        /// <summary>
        /// The number of opportunities to talk remaining.
        /// </summary>
#endif
        public Dictionary<Agent, int> RemainTalkMap
        {
            get => _remainTalkMap;
            set
            {
                _remainTalkMap = value;
                remainTalkMap = value == null ? new Dictionary<int, int>() : value.ToDictionary(p => p.Key.AgentIdx, p => p.Value);
            }
        }

        [DataMember(Name = "remainTalkMap")]
        Dictionary<int, int> remainTalkMap;
        Dictionary<Agent, int> _remainTalkMap;

#if JHELP
        /// <summary>
        /// 囁きの残り回数
        /// </summary>
#else
        /// <summary>
        /// The number of opportunities to whisper remaining.
        /// </summary>
#endif
        public Dictionary<Agent, int> RemainWhisperMap
        {
            get => _remainWhisperMap;
            set
            {
                _remainWhisperMap = value;
                remainWhisperMap = value == null ? new Dictionary<int, int>() : value.ToDictionary(p => p.Key.AgentIdx, p => p.Value);
            }
        }

        [DataMember(Name = "remainWhisperMap")]
        Dictionary<int, int> remainWhisperMap;
        Dictionary<Agent, int> _remainWhisperMap;

        //List<Role> existingRoleList = new List<Role>();

#if JHELP
        /// <summary>
        /// 昨夜亡くなったエージェントのリスト
        /// </summary>
#else
        /// <summary>
        /// The list of agents who died last night.
        /// </summary>
#endif
        public List<Agent> LastDeadAgentList
        {
            get => _lastDeadAgentList;
            set
            {
                _lastDeadAgentList = value;
                lastDeadAgentList = value == null ? new List<int>() : value.Select(a => a.AgentIdx).ToList();
            }
        }

        [DataMember(Name = "lastDeadAgentList")]
        List<int> lastDeadAgentList;
        List<Agent> _lastDeadAgentList;

#if JHELP
        /// <summary>
        /// 本日
        /// </summary>
#else
        /// <summary>
        /// Current day.
        /// </summary>
#endif
        [DataMember(Name = "day")]
        public int Day { get; set; }

#if JHELP
        /// <summary>
        /// このゲーム情報を受け取るエージェントの役職
        /// </summary>
#else
        /// <summary>
        /// The role of player who receives this GameInfo.
        /// </summary>
#endif
        public Role Role { get; set; }

#if JHELP
        /// <summary>
        /// エージェントのリスト
        /// </summary>
#else
        /// <summary>
        /// The list of agents.
        /// </summary>
#endif
        public List<Agent> AgentList { get; set; }

#if JHELP
        /// <summary>
        /// 霊媒結果
        /// </summary>
        /// <remarks>霊媒師限定</remarks>
#else
        /// <summary>
        /// The result of the inquest.
        /// </summary>
        /// <remarks>Medium only.</remarks>
#endif
        [DataMember(Name = "mediumResult")]
        public Judge MediumResult { get; set; }

#if JHELP
        /// <summary>
        /// 占い結果
        /// </summary>
        /// <remarks>占い師限定</remarks>
#else
        /// <summary>
        /// The result of the dvination.
        /// </summary>
        /// <remarks>Seer only.</remarks>
#endif
        [DataMember(Name = "divineResult")]
        public Judge DivineResult { get; set; }

#if JHELP
        /// <summary>
        /// 追放投票のリスト
        /// </summary>
        /// <remarks>各プレイヤーの投票先がわかる</remarks>
#else
        /// <summary>
        /// The list of votes for execution.
        /// </summary>
        /// <remarks>You can see who votes to who.</remarks>
#endif
        [DataMember(Name = "voteList")]
        public List<Vote> VoteList { get; set; }

#if JHELP
        /// <summary>
        /// 直近の追放投票のリスト
        /// </summary>
        /// <remarks>各プレイヤーの投票先がわかる</remarks>
#else
        /// <summary>
        /// The latest list of votes for execution.
        /// </summary>
        /// <remarks>You can see who votes to who.</remarks>
#endif
        [DataMember(Name = "latestVoteList")]
        public List<Vote> LatestVoteList { get; set; }

#if JHELP
        /// <summary>
        /// 襲撃投票リスト
        /// </summary>
        /// <remarks>人狼限定</remarks>
#else
        /// <summary>
        /// The list of votes for attack.
        /// </summary>
        /// <remarks>Werewolf only.</remarks>
#endif
        [DataMember(Name = "attackVoteList")]
        public List<Vote> AttackVoteList { get; set; }

#if JHELP
        /// <summary>
        /// 直近の襲撃投票リスト
        /// </summary>
        /// <remarks>人狼限定</remarks>
#else
        /// <summary>
        /// The latest list of votes for attack.
        /// </summary>
        /// <remarks>Werewolf only.</remarks>
#endif
        [DataMember(Name = "latestAttackVoteList")]
        public List<Vote> LatestAttackVoteList { get; set; }

#if JHELP
        /// <summary>
        /// 本日の会話リスト
        /// </summary>
#else
        /// <summary>
        /// The list of today's talks.
        /// </summary>
#endif
        [DataMember(Name = "talkList")]
        public List<Talk> TalkList { get; set; }

#if JHELP
        /// <summary>
        /// 本日の囁きリスト
        /// </summary>
        /// <remarks>人狼限定</remarks>
#else
        /// <summary>
        /// The list of today's whispers.
        /// </summary>
        /// <remarks>Werewolf only.</remarks>
#endif
        [DataMember(Name = "whisperList")]
        public List<Whisper> WhisperList { get; set; }

#if JHELP
        /// <summary>
        /// 生存しているエージェントのリスト
        /// </summary>
        /// <remarks>すべてのエージェントが死んだ場合，nullではなく空のリストを返す</remarks>
#else
        /// <summary>
        /// The list of alive agents.
        /// </summary>
        /// <remarks>If all agents are dead, this returns an empty list, not null.</remarks>
#endif
        public List<Agent> AliveAgentList => AgentList.Where(a => StatusMap[a] == Status.ALIVE).ToList();

#if JHELP
        /// <summary>
        /// このゲームにおいて存在する役職のリスト
        /// </summary>
#else
        /// <summary>
        /// The list of existing roles in this game.
        /// </summary>
#endif
        [DataMember(Name = "existingRoleList")]
        public List<Role> ExistingRoleList { get; set; }

#if JHELP
        /// <summary>
        /// GameInfoクラスの新しいインスタンスを初期化する
        /// </summary>
#else
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
#endif
        public GameInfo() { }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="day">The current day.</param>
        /// <param name="agent">The agent who receives this.</param>
        /// <param name="mediumResult">The result of the inquest.</param>
        /// <param name="divineResult">The result of the divination.</param>
        /// <param name="executedAgent">The agent executed.</param>
        /// <param name="latestExecutedAgent">The latest executed agent.</param>
        /// <param name="cursedFox">The fox killed by curse.</param>
        /// <param name="attackedAgent">The agent attacked.</param>
        /// <param name="guardedAgent">The agent guarded.</param>
        /// <param name="voteList">The list of votes for execution.</param>
        /// <param name="latestVoteList">The latest list of votes for execution.</param>
        /// <param name="attackVoteList">The list of votes for attack.</param>
        /// <param name="latestAttackVoteList">The latest list of votes for attack.</param>
        /// <param name="talkList">The list of talks.</param>
        /// <param name="whisperList">The list of whispers.</param>
        /// <param name="lastDeadAgentList">The list of agents who died last night.</param>
        /// <param name="existingRoleList">The list of existing roles in this game.</param>
        /// <param name="statusMap">The map between agent and its status.</param>
        /// <param name="roleMap">The map between agent and its role.</param>
        /// <param name="remainTalkMap">The map between agent and its number of remaining talks.</param>
        /// <param name="remainWhisperMap">The map between agent and its number of remaining whispers.</param>
        [JsonConstructor]
        GameInfo(int day, int agent, Judge mediumResult, Judge divineResult, int executedAgent,
            int latestExecutedAgent, int cursedFox, int attackedAgent, int guardedAgent,
            List<Vote> voteList, List<Vote> latestVoteList,
            List<Vote> attackVoteList, List<Vote> latestAttackVoteList,
            List<Talk> talkList, List<Whisper> whisperList,
            List<int> lastDeadAgentList, List<Role> existingRoleList,
            Dictionary<int, string> statusMap, Dictionary<int, string> roleMap,
            Dictionary<int, int> remainTalkMap, Dictionary<int, int> remainWhisperMap)
        {
            Day = day;
            Agent = Agent.GetAgent(agent);
            MediumResult = mediumResult;
            DivineResult = divineResult;
            ExecutedAgent = Agent.GetAgent(executedAgent);
            LatestExecutedAgent = Agent.GetAgent(latestExecutedAgent);
            CursedFox = Agent.GetAgent(cursedFox);
            AttackedAgent = Agent.GetAgent(attackedAgent);
            GuardedAgent = Agent.GetAgent(guardedAgent);
            VoteList = voteList;
            LatestVoteList = latestVoteList;
            AttackVoteList = attackVoteList;
            LatestAttackVoteList = latestAttackVoteList;
            TalkList = talkList;
            WhisperList = whisperList;
            LastDeadAgentList = lastDeadAgentList.Select(i => Agent.GetAgent(i)).ToList();
            ExistingRoleList = existingRoleList;
            StatusMap = statusMap.ToDictionary(p => Agent.GetAgent(p.Key), p => (Status)Enum.Parse(typeof(Status), p.Value));
            RoleMap = roleMap.ToDictionary(p => Agent.GetAgent(p.Key), p => (Role)Enum.Parse(typeof(Role), p.Value));
            RemainTalkMap = remainTalkMap.ToDictionary(p => Agent.GetAgent(p.Key), p => p.Value);
            RemainWhisperMap = remainWhisperMap.ToDictionary(p => Agent.GetAgent(p.Key), p => p.Value);
        }
    }
}
