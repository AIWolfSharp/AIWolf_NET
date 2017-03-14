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
        /// 本日
        /// </summary>
#else
        /// <summary>
        /// Current day.
        /// </summary>
#endif
        [DataMember(Name = "day")]
        public int Day { get; }

#if JHELP
        /// <summary>
        /// このゲーム情報を受け取るエージェント
        /// </summary>
#else
        /// <summary>
        /// The agent who receives this GameInfo.
        /// </summary>
#endif
        public Agent Agent { get; }

        /// <summary>
        /// The index number of agent who receives this game information.
        /// </summary>
        [DataMember(Name = "agent")]
        int _Agent { get; }

#if JHELP
        /// <summary>
        /// このゲーム情報を受け取るエージェントの役職
        /// </summary>
#else
        /// <summary>
        /// The role of player who receives this GameInfo.
        /// </summary>
#endif
        public Role Role
        {
            get
            {
                return RoleMap.ContainsKey(Agent) ? RoleMap[Agent] : Role.UNC;
            }
        }

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
        public Judge MediumResult { get; }

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
        public Judge DivineResult { get; }

#if JHELP
        /// <summary>
        /// 昨夜追放されたエージェント
        /// </summary>
#else
        /// <summary>
        /// The agent executed last night.
        /// </summary>
#endif
        public Agent ExecutedAgent { get; }

        /// <summary>
        /// The index number of the agent executed last night.
        /// </summary>
        [DataMember(Name = "executedAgent")]
        int _ExecutedAgent { get; }

#if JHELP
        /// <summary>
        /// 直近に追放されたエージェント
        /// </summary>
#else
        /// <summary>
        /// The latest executed agent.
        /// </summary>
#endif
        public Agent LatestExecutedAgent { get; }

        /// <summary>
        /// The index number of the latest executed agent.
        /// </summary>
        [DataMember(Name = "latestExecutedAgent")]
        int _LatestExecutedAgent { get; }

#if JHELP
        /// <summary>
        /// 呪殺された妖狐
        /// </summary>
#else
        /// <summary>
        /// The fox killed by curse.
        /// </summary>
#endif
        public Agent CursedFox { get; }

        /// <summary>
        /// The index number of the fox killed by curse.
        /// </summary>
        [DataMember(Name = "cursedFox")]
        int _CursedFox { get; }

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
        public Agent AttackedAgent { get; }

        /// <summary>
        /// The index number of the agent decided to be attacked.
        /// </summary>
        [DataMember(Name = "attackedAgent")]
        int _AttackedAgent { get; }

#if JHELP
        /// <summary>
        /// 昨夜護衛されたエージェント
        /// </summary>
#else
        /// <summary>
        /// The agent guarded last night.
        /// </summary>
#endif
        public Agent GuardedAgent { get; }

        /// <summary>
        /// The index number of the agent guarded last night.
        /// </summary>
        [DataMember(Name = "guardedAgent")]
        int _GuardedAgent { get; }

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
        public List<Vote> VoteList { get; }

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
        public List<Vote> LatestVoteList { get; }

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
        public List<Vote> AttackVoteList { get; }

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
        public List<Vote> LatestAttackVoteList { get; }

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
        public List<Talk> TalkList { get; }

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
        public List<Whisper> WhisperList { get; }

#if JHELP
        /// <summary>
        /// 昨夜亡くなったエージェントのリスト
        /// </summary>
#else
        /// <summary>
        /// The list of agents who died last night.
        /// </summary>
#endif
        public List<Agent> LastDeadAgentList { get; }

        /// <summary>
        /// The list of indexes of agents who died last night.
        /// </summary>
        [DataMember(Name = "lastDeadAgentList")]
        List<int> _LastDeadAgentList { get; }

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
        public List<Role> ExistingRoleList { get; }

#if JHELP
        /// <summary>
        /// 全エージェントの生死状況
        /// </summary>
#else
        /// <summary>
        /// The statuses of all agents.
        /// </summary>
#endif
        public Dictionary<Agent, Status> StatusMap { get; }

        /// <summary>
        /// The statuses of all agents.
        /// </summary>
        [DataMember(Name = "statusMap")]
        Dictionary<int, string> _StatusMap { get; }

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
        public Dictionary<Agent, Role> RoleMap { get; }

        /// <summary>
        /// The known roles of agents.
        /// </summary>
        /// <remarks>
        /// If you are human, you know only yourself.
        /// If you are werewolf, you know other werewolves.
        /// </remarks>
        [DataMember(Name = "roleMap")]
        Dictionary<int, string> _RoleMap { get; }

#if JHELP
        /// <summary>
        /// トークの残り回数
        /// </summary>
#else
        /// <summary>
        /// The number of opportunities to talk remaining.
        /// </summary>
#endif
        public Dictionary<Agent, int> RemainTalkMap { get; }

        /// <summary>
        /// The number of opportunities to talk remaining.
        /// </summary>
        [DataMember(Name = "remainTalkMap")]
        Dictionary<int, int> _RemainTalkMap { get; }

#if JHELP
        /// <summary>
        /// 囁きの残り回数
        /// </summary>
#else
        /// <summary>
        /// The number of opportunities to whisper remaining.
        /// </summary>
#endif
        public Dictionary<Agent, int> RemainWhisperMap { get; }

        /// <summary>
        /// The number of opportunities to whisper remaining.
        /// </summary>
        [DataMember(Name = "remainWhisperMap")]
        Dictionary<int, int> _RemainWhisperMap { get; }

#if JHELP
        /// <summary>
        /// エージェントのリスト
        /// </summary>
#else
        /// <summary>
        /// The list of agents.
        /// </summary>
#endif
        public List<Agent> AgentList
        {
            get
            {
                return StatusMap.Keys.ToList();
            }
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
        public List<Agent> AliveAgentList
        {
            get
            {
                return AgentList.Where(a => StatusMap[a] == Status.ALIVE).ToList();
            }
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
            if (Day < 0)
            {
                Error.RuntimeError("Invalid day " + Day + ".");
                Error.Warning("Force it to be 0.");
                Day = 0;
            }

            Agent = Agent.GetAgent(agent);
            if (Agent == null)
            {
                Error.RuntimeError("Agent must not be null.");
                Error.Warning("Force it to be Agent[00].");
                Agent = Agent.GetAgent(0);
            }
            _Agent = Agent.AgentIdx;

            MediumResult = mediumResult;
            DivineResult = divineResult;

            ExecutedAgent = Agent.GetAgent(executedAgent);
            _ExecutedAgent = ExecutedAgent == null ? -1 : ExecutedAgent.AgentIdx;

            LatestExecutedAgent = Agent.GetAgent(latestExecutedAgent);
            _LatestExecutedAgent = LatestExecutedAgent == null ? -1 : LatestExecutedAgent.AgentIdx;

            CursedFox = Agent.GetAgent(cursedFox);
            _CursedFox = CursedFox == null ? -1 : CursedFox.AgentIdx;

            AttackedAgent = Agent.GetAgent(attackedAgent);
            _AttackedAgent = AttackedAgent == null ? -1 : AttackedAgent.AgentIdx;

            GuardedAgent = Agent.GetAgent(guardedAgent);
            _GuardedAgent = GuardedAgent == null ? -1 : GuardedAgent.AgentIdx;

            VoteList = voteList == null ? new List<Vote>() : voteList;
            LatestVoteList = latestVoteList == null ? new List<Vote>() : latestVoteList;
            AttackVoteList = attackVoteList == null ? new List<Vote>() : attackVoteList;
            LatestAttackVoteList = latestAttackVoteList == null ? new List<Vote>() : latestAttackVoteList;
            TalkList = talkList == null ? new List<Talk>() : talkList;
            WhisperList = whisperList == null ? new List<Whisper>() : whisperList;
            ExistingRoleList = existingRoleList == null ? new List<Role>() : existingRoleList;

            LastDeadAgentList = new List<Agent>();
            if (lastDeadAgentList != null)
            {
                foreach (var i in lastDeadAgentList)
                {
                    LastDeadAgentList.Add(Agent.GetAgent(i));
                }
            }
            _LastDeadAgentList = LastDeadAgentList.Select(a => a.AgentIdx).ToList();

            StatusMap = new Dictionary<Agent, Status>();
            if (statusMap != null)
            {
                foreach (var p in statusMap)
                {
                    Status status;
                    if (!Enum.TryParse(p.Value, out status))
                    {
                        Error.RuntimeError("Invalid status string " + p.Value + ".");
                        Error.Warning("Force it to be Status.ALIVE.");
                        status = Status.ALIVE;
                    }
                    StatusMap[Agent.GetAgent(p.Key)] = status;
                }
            }
            _StatusMap = StatusMap.ToDictionary(p => p.Key.AgentIdx, p => p.Value.ToString());

            RoleMap = new Dictionary<Agent, Role>();
            if (roleMap != null)
            {
                foreach (var p in roleMap)
                {
                    Role role;
                    if (!Enum.TryParse(p.Value, out role) || role == Role.UNC)
                    {
                        Error.RuntimeError("Invalid role string " + p.Value + ".");
                        Error.Warning("It is removed from role map.");
                    }
                    else
                    {
                        RoleMap[Agent.GetAgent(p.Key)] = role;
                    }
                }
            }
            _RoleMap = RoleMap.Where(m => m.Value != Role.UNC).ToDictionary(m => m.Key.AgentIdx, m => m.Value.ToString());

            RemainTalkMap = new Dictionary<Agent, int>();
            if (remainTalkMap != null)
            {
                foreach (var p in remainTalkMap)
                {
                    RemainTalkMap[Agent.GetAgent(p.Key)] = p.Value;
                }
            }
            _RemainTalkMap = RemainTalkMap.ToDictionary(p => p.Key.AgentIdx, p => p.Value);

            RemainWhisperMap = new Dictionary<Agent, int>();
            if (remainWhisperMap != null)
            {
                foreach (var p in remainWhisperMap)
                {
                    RemainWhisperMap[Agent.GetAgent(p.Key)] = p.Value;
                }
            }
            _RemainWhisperMap = RemainWhisperMap.ToDictionary(p => p.Key.AgentIdx, p => p.Value);
        }
    }
}
