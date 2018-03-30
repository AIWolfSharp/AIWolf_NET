//
// IGameInfo.cs
//
// Copyright (c) 2018 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using System.Collections.Generic;

namespace AIWolf.Lib
{
#if JHELP
    /// <summary>
    /// ゲーム情報クラスが実装すべきプロパティ
    /// </summary>
#else
    /// <summary>
    /// Defines the properties to be in the class for game information.
    /// </summary>
#endif
    public interface IGameInfo
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
        Agent Agent { get; }

#if JHELP
        /// <summary>
        /// 昨夜追放されたエージェント
        /// </summary>
#else
        /// <summary>
        /// The agent executed last night.
        /// </summary>
#endif
        Agent ExecutedAgent { get; }

#if JHELP
        /// <summary>
        /// 直近に追放されたエージェント
        /// </summary>
#else
        /// <summary>
        /// The latest executed agent.
        /// </summary>
#endif
        Agent LatestExecutedAgent { get; }

#if JHELP
        /// <summary>
        /// 呪殺された妖狐
        /// </summary>
#else
        /// <summary>
        /// The fox killed by curse.
        /// </summary>
#endif
        Agent CursedFox { get; }

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
        Agent AttackedAgent { get; }

#if JHELP
        /// <summary>
        /// 昨夜護衛されたエージェント
        /// </summary>
#else
        /// <summary>
        /// The agent guarded last night.
        /// </summary>
#endif
        Agent GuardedAgent { get; }

#if JHELP
        /// <summary>
        /// 全エージェントの生死状況
        /// </summary>
#else
        /// <summary>
        /// The statuses of all agents.
        /// </summary>
#endif
        IDictionary<Agent, Status> StatusMap { get; }

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
        IDictionary<Agent, Role> RoleMap { get; }

#if JHELP
        /// <summary>
        /// トークの残り回数
        /// </summary>
#else
        /// <summary>
        /// The number of opportunities to talk remaining.
        /// </summary>
#endif
        IDictionary<Agent, int> RemainTalkMap { get; }

#if JHELP
        /// <summary>
        /// 囁きの残り回数
        /// </summary>
#else
        /// <summary>
        /// The number of opportunities to whisper remaining.
        /// </summary>
#endif
        IDictionary<Agent, int> RemainWhisperMap { get; }

#if JHELP
        /// <summary>
        /// 昨夜亡くなったエージェントのリスト
        /// </summary>
#else
        /// <summary>
        /// The list of agents who died last night.
        /// </summary>
#endif
        IList<Agent> LastDeadAgentList { get; }

#if JHELP
        /// <summary>
        /// 本日
        /// </summary>
#else
        /// <summary>
        /// Current day.
        /// </summary>
#endif
        int Day { get; }

#if JHELP
        /// <summary>
        /// このゲーム情報を受け取るエージェントの役職
        /// </summary>
#else
        /// <summary>
        /// The role of player who receives this GameInfo.
        /// </summary>
#endif
        Role Role { get; }

#if JHELP
        /// <summary>
        /// エージェントのリスト
        /// </summary>
#else
        /// <summary>
        /// The list of agents.
        /// </summary>
#endif
        IList<Agent> AgentList { get; }

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
        Judge MediumResult { get; }

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
        Judge DivineResult { get; }

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
        IList<Vote> VoteList { get; }

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
        IList<Vote> LatestVoteList { get; }

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
        IList<Vote> AttackVoteList { get; }

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
        IList<Vote> LatestAttackVoteList { get; }

#if JHELP
        /// <summary>
        /// 本日の会話リスト
        /// </summary>
#else
        /// <summary>
        /// The list of today's talks.
        /// </summary>
#endif
        IList<Talk> TalkList { get; }

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
        IList<Whisper> WhisperList { get; }

#if JHELP
        /// <summary>
        /// 生存しているエージェントのリスト
        /// </summary>
#else
        /// <summary>
        /// The list of alive agents.
        /// </summary>
#endif
        IList<Agent> AliveAgentList { get; }

#if JHELP
        /// <summary>
        /// このゲームにおいて存在する役職のリスト
        /// </summary>
#else
        /// <summary>
        /// The list of existing roles in this game.
        /// </summary>
#endif
        IList<Role> ExistingRoleList { get; }

    }
}
