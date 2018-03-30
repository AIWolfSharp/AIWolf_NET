//
// GameInfo.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using AIWolf.Lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AIWolf.Client
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
    public class GameInfo : IGameInfo
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
        public Agent Agent { get; }

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

#if JHELP
        /// <summary>
        /// 全エージェントの生死状況
        /// </summary>
#else
        /// <summary>
        /// The statuses of all agents.
        /// </summary>
#endif
        public IDictionary<Agent, Status> StatusMap { get; }

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
        public IDictionary<Agent, Role> RoleMap { get; }

#if JHELP
        /// <summary>
        /// トークの残り回数
        /// </summary>
#else
        /// <summary>
        /// The number of opportunities to talk remaining.
        /// </summary>
#endif
        public IDictionary<Agent, int> RemainTalkMap { get; }

#if JHELP
        /// <summary>
        /// 囁きの残り回数
        /// </summary>
#else
        /// <summary>
        /// The number of opportunities to whisper remaining.
        /// </summary>
#endif
        public IDictionary<Agent, int> RemainWhisperMap { get; }

#if JHELP
        /// <summary>
        /// 昨夜亡くなったエージェントのリスト
        /// </summary>
#else
        /// <summary>
        /// The list of agents who died last night.
        /// </summary>
#endif
        public IList<Agent> LastDeadAgentList { get; }

#if JHELP
        /// <summary>
        /// 本日
        /// </summary>
#else
        /// <summary>
        /// Current day.
        /// </summary>
#endif
        public int Day { get; }

#if JHELP
        /// <summary>
        /// このゲーム情報を受け取るエージェントの役職
        /// </summary>
#else
        /// <summary>
        /// The role of player who receives this GameInfo.
        /// </summary>
#endif
        public Role Role { get; }

#if JHELP
        /// <summary>
        /// エージェントのリスト
        /// </summary>
#else
        /// <summary>
        /// The list of agents.
        /// </summary>
#endif
        public IList<Agent> AgentList { get; }

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
        public Judge DivineResult { get; }

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
        public IList<Vote> VoteList { get; }

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
        public IList<Vote> LatestVoteList { get; }

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
        public IList<Vote> AttackVoteList { get; }

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
        public IList<Vote> LatestAttackVoteList { get; }

#if JHELP
        /// <summary>
        /// 本日の会話リスト
        /// </summary>
#else
        /// <summary>
        /// The list of today's talks.
        /// </summary>
#endif
        public IList<Talk> TalkList { get; }
        List<Talk> talkList;

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
        public IList<Whisper> WhisperList { get; }
        List<Whisper> whisperList;

#if JHELP
        /// <summary>
        /// 生存しているエージェントのリスト
        /// </summary>
#else
        /// <summary>
        /// The list of alive agents.
        /// </summary>
#endif
        public IList<Agent> AliveAgentList { get; }

#if JHELP
        /// <summary>
        /// このゲームにおいて存在する役職のリスト
        /// </summary>
#else
        /// <summary>
        /// The list of existing roles in this game.
        /// </summary>
#endif
        public IList<Role> ExistingRoleList { get; }

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
            VoteList = voteList.AsReadOnly();
            LatestVoteList = latestVoteList.AsReadOnly();
            AttackVoteList = attackVoteList.AsReadOnly();
            LatestAttackVoteList = latestAttackVoteList.AsReadOnly();
            this.talkList = talkList;
            TalkList = this.talkList.AsReadOnly();
            this.whisperList = whisperList;
            WhisperList = this.whisperList.AsReadOnly();
            LastDeadAgentList = lastDeadAgentList.Select(i => Agent.GetAgent(i)).ToList().AsReadOnly();
            ExistingRoleList = existingRoleList.AsReadOnly();
            StatusMap = new ReadOnlyDictionary<Agent, Status>(statusMap.ToDictionary(p => Agent.GetAgent(p.Key), p => (Status)Enum.Parse(typeof(Status), p.Value)));
            RoleMap = new ReadOnlyDictionary<Agent, Role>(roleMap.ToDictionary(p => Agent.GetAgent(p.Key), p => (Role)Enum.Parse(typeof(Role), p.Value)));
            RemainTalkMap = new ReadOnlyDictionary<Agent, int>(remainTalkMap.ToDictionary(p => Agent.GetAgent(p.Key), p => p.Value));
            RemainWhisperMap = new ReadOnlyDictionary<Agent, int>(remainWhisperMap.ToDictionary(p => Agent.GetAgent(p.Key), p => p.Value));

            Role = RoleMap[Agent];
            AgentList = StatusMap.Keys.Shuffle().ToList().AsReadOnly();
            AliveAgentList = AgentList.Where(a => StatusMap[a] == Status.ALIVE).ToList().AsReadOnly();
        }

        internal void AddTalk(Talk talk) => talkList.Add(talk);

        internal void AddWhisper(Whisper whisper) => whisperList.Add(whisper);
    }
}
