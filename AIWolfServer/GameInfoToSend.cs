//
// GameInfoToSend.cs
//
// Copyright (c) 2017 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using AIWolf.Lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace AIWolf.Server
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
    public class GameInfoToSend
    {
        [DataMember(Name = "agent")]
        int _agent = 0;

        [DataMember(Name = "executedAgent")]
        int _executedAgent = -1;

        [DataMember(Name = "latestExecutedAgent")]
        int _latestExecutedAgent = -1;

        [DataMember(Name = "cursedFox")]
        int _cursedFox = -1;

        [DataMember(Name = "attackedAgent")]
        int _attackedAgent = -1;

        [DataMember(Name = "guardedAgent")]
        int _guardedAgent = -1;

        List<Vote> _voteList = new List<Vote>();
        List<Vote> _latestVoteList = new List<Vote>();
        List<Vote> _attackVoteList = new List<Vote>();
        List<Vote> _latestAttackVoteList = new List<Vote>();
        List<Talk> _talkList = new List<Talk>();
        List<Whisper> _whisperList = new List<Whisper>();

        [DataMember(Name = "statusMap")]
        Dictionary<int, string> _statusMap = new Dictionary<int, string>();

        [DataMember(Name = "roleMap")]
        Dictionary<int, string> _roleMap = new Dictionary<int, string>();

        [DataMember(Name = "remainTalkMap")]
        Dictionary<int, int> _remainTalkMap = new Dictionary<int, int>();

        [DataMember(Name = "remainWhisperMap")]
        Dictionary<int, int> _remainWhisperMap = new Dictionary<int, int>();

        List<Role> _existingRoleList = new List<Role>();

        [DataMember(Name = "lastDeadAgentList")]
        List<int> _lastDeadAgentList = new List<int>();


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
        public Role Role => RoleMap.ContainsKey(Agent) ? RoleMap[Agent] : Role.UNC;

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
            get => Agent.GetAgent(_agent);
            set => _agent = value == null ? -1 : value.AgentIdx;
        }

#if JHELP
        /// <summary>
        /// エージェントのリスト
        /// </summary>
#else
        /// <summary>
        /// The list of agents.
        /// </summary>
#endif
        public IList<Agent> AgentList => StatusMap.Keys.ToList();

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
        /// 昨夜追放されたエージェント
        /// </summary>
#else
        /// <summary>
        /// The agent executed last night.
        /// </summary>
#endif
        public Agent ExecutedAgent
        {
            get => Agent.GetAgent(_executedAgent);
            set => _executedAgent = value == null ? -1 : value.AgentIdx;
        }

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
            get => Agent.GetAgent(_latestExecutedAgent);
            set => _latestExecutedAgent = value == null ? -1 : value.AgentIdx;
        }

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
            get => Agent.GetAgent(_cursedFox);
            set => _cursedFox = value == null ? -1 : value.AgentIdx;
        }

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
            get => Agent.GetAgent(_attackedAgent);
            set => _attackedAgent = value == null ? -1 : value.AgentIdx;
        }

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
            get => Agent.GetAgent(_guardedAgent);
            set => _guardedAgent = value == null ? -1 : value.AgentIdx;
        }

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
        public IList<Vote> VoteList
        {
            get => _voteList;
            set => _voteList = value == null ? new List<Vote>() : new List<Vote>(value);
        }

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
        public IList<Vote> LatestVoteList
        {
            get => _latestVoteList;
            set => _latestVoteList = value == null ? new List<Vote>() : new List<Vote>(value);
        }

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
        public IList<Vote> AttackVoteList
        {
            get => _attackVoteList;
            set => _attackVoteList = value == null ? new List<Vote>() : new List<Vote>(value);
        }

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
        public IList<Vote> LatestAttackVoteList
        {
            get => _latestAttackVoteList;
            set => _latestAttackVoteList = value == null ? new List<Vote>() : new List<Vote>(value);
        }

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
        public IList<Talk> TalkList
        {
            get => _talkList;
            set => _talkList = value == null ? new List<Talk>() : new List<Talk>(value);
        }

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
        public IList<Whisper> WhisperList
        {
            get => _whisperList;
            set => _whisperList = value == null ? new List<Whisper>() : new List<Whisper>(value);
        }

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
        public IList<Agent> AliveAgentList => AgentList.Where(a => StatusMap[a] == Status.ALIVE).ToList();

#if JHELP
        /// <summary>
        /// 全エージェントの生死状況
        /// </summary>
#else
        /// <summary>
        /// The statuses of all agents.
        /// </summary>
#endif
        public IDictionary<Agent, Status> StatusMap
        {
            get => _statusMap.ToDictionary(p => Agent.GetAgent(p.Key), p => (Status)Enum.Parse(typeof(Status), p.Value));
            set => _statusMap = value == null ? new Dictionary<int, string>() : value.ToDictionary(p => p.Key.AgentIdx, p => p.Value.ToString());
        }

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
        public IDictionary<Agent, Role> RoleMap
        {
            get => _roleMap.ToDictionary(p => Agent.GetAgent(p.Key), p => (Role)Enum.Parse(typeof(Role), p.Value));
            set => _roleMap = value == null ? new Dictionary<int, string>() : value.ToDictionary(p => p.Key.AgentIdx, p => p.Value.ToString());
        }

#if JHELP
        /// <summary>
        /// トークの残り回数
        /// </summary>
#else
        /// <summary>
        /// The number of opportunities to talk remaining.
        /// </summary>
#endif
        public IDictionary<Agent, int> RemainTalkMap
        {
            get => _remainTalkMap.ToDictionary(p => Agent.GetAgent(p.Key), p => p.Value);
            set => _remainTalkMap = value == null ? new Dictionary<int, int>() : value.ToDictionary(p => p.Key.AgentIdx, p => p.Value);
        }

#if JHELP
        /// <summary>
        /// 囁きの残り回数
        /// </summary>
#else
        /// <summary>
        /// The number of opportunities to whisper remaining.
        /// </summary>
#endif
        public IDictionary<Agent, int> RemainWhisperMap
        {
            get => _remainWhisperMap.ToDictionary(p => Agent.GetAgent(p.Key), p => p.Value);
            set => _remainWhisperMap = value == null ? new Dictionary<int, int>() : value.ToDictionary(p => p.Key.AgentIdx, p => p.Value);
        }

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
        public IList<Role> ExistingRoleList
        {
            get => _existingRoleList;
            set => _existingRoleList = value == null ? new List<Role>() : new List<Role>(value);
        }

#if JHELP
        /// <summary>
        /// 昨夜亡くなったエージェントのリスト
        /// </summary>
#else
        /// <summary>
        /// The list of agents who died last night.
        /// </summary>
#endif
        public IList<Agent> LastDeadAgentList
        {
            get => _lastDeadAgentList.Select(i => Agent.GetAgent(i)).ToList();
            set => _lastDeadAgentList = value == null ? new List<int>() : value.Select(a => a.AgentIdx).ToList();
        }

#if JHELP
        /// <summary>
        /// GameInfoクラスの新しいインスタンスを初期化する
        /// </summary>
#else
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
#endif
        public GameInfoToSend()
        {
        }

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
        GameInfoToSend(int day, int agent, Judge mediumResult, Judge divineResult, int executedAgent,
            int latestExecutedAgent, int cursedFox, int attackedAgent, int guardedAgent,
            List<Vote> voteList, List<Vote> latestVoteList,
            List<Vote> attackVoteList, List<Vote> latestAttackVoteList,
            List<Talk> talkList, List<Whisper> whisperList,
            List<int> lastDeadAgentList, List<Role> existingRoleList,
            Dictionary<int, string> statusMap, Dictionary<int, string> roleMap,
            Dictionary<int, int> remainTalkMap, Dictionary<int, int> remainWhisperMap)
        {
            Day = day;
            _agent = agent;
            MediumResult = mediumResult;
            DivineResult = divineResult;
            _executedAgent = executedAgent;
            _latestExecutedAgent = latestExecutedAgent;
            _cursedFox = cursedFox;
            _attackedAgent = attackedAgent;
            _guardedAgent = guardedAgent;
            _voteList = voteList;
            _latestVoteList = latestVoteList;
            _attackVoteList = attackVoteList;
            _latestAttackVoteList = latestAttackVoteList;
            _talkList = talkList;
            _whisperList = whisperList;
            _lastDeadAgentList = lastDeadAgentList;
            _existingRoleList = existingRoleList;
            _statusMap = statusMap;
            _roleMap = roleMap;
            _remainTalkMap = remainTalkMap;
            _remainWhisperMap = remainWhisperMap;
        }
    }
}
