//
// ContentBuilder.cs
//
// Copyright (c) 2017 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using System;
using System.Collections.Generic;

namespace AIWolf.Lib
{
#if JHELP
    /// <summary>
    /// 発話の種類
    /// </summary>
#else
    /// <summary>
    /// The type of an utterance.
    /// </summary>
#endif
    public enum UtteranceType
    {
#if JHELP
        /// <summary>
        /// 不明
        /// </summary>
#else
        /// <summary>
        /// Uncertain.
        /// </summary>
#endif
        [Obsolete]
        UNC,
#if JHELP
        /// <summary>
        /// 会話
        /// </summary>
#else
        /// <summary>
        /// Talk.
        /// </summary>
#endif
        TALK = 1,
#if JHELP
        /// <summary>
        /// 囁き
        /// </summary>
#else
        /// <summary>
        /// Whisper.
        /// </summary>
#endif
        WHISPER = 2
    }

#if JHELP
    /// <summary>
    /// 各種発話内容を生成するビルダークラスのためのクラス
    /// </summary>
#else
    /// <summary>
    /// A class for the builder classes to build Content of all kinds.
    /// </summary>
#endif
    public class ContentBuilder
    {
        internal ContentBuilder() { }

        private Agent _subject;
        private Agent _target;

#if JHELP
        /// <summary>
        /// Contentのトピック
        /// </summary>
#else
        /// <summary>
        /// The topic of the Content.
        /// </summary>
#endif
        internal Topic Topic { get; set; }

#if JHELP
        /// <summary>
        /// Contentの主語
        /// </summary>
#else
        /// <summary>
        /// The subject of the Content.
        /// </summary>
#endif
        internal Agent Subject { get => _subject; set => _subject = value ?? Agent.NONE; }

#if JHELP
        /// <summary>
        /// Contentの対象エージェント
        /// </summary>
#else
        /// <summary>
        /// The target agent of the Content.
        /// </summary>
#endif
        internal Agent Target { get => _target; set => _target = value ?? Agent.ANY; }

#if JHELP
        /// <summary>
        /// Contentが言及する役職
        /// </summary>
#else
        /// <summary>
        /// The role the Content refers to.
        /// </summary>
#endif
        internal Role Role { get; set; }

#if JHELP
        /// <summary>
        /// Contentが言及する種族
        /// </summary>
#else
        /// <summary>
        /// The species the Content refers to.
        /// </summary>
#endif
        internal Species Result { get; set; }

#if JHELP
        /// <summary>
        /// Contentが言及する発話
        /// </summary>
#else
        /// <summary>
        /// The utterance the Content refers to.
        /// </summary>
#endif
        internal IUtterance Utterance { get; set; }

#if JHELP
        /// <summary>
        /// Content中の演算子
        /// </summary>
#else
        /// <summary>
        /// The operator in the Content.
        /// </summary>
#endif
        internal Operator Operator { get; set; }

#if JHELP
        /// <summary>
        /// Content中の被演算子リスト
        /// </summary>
#else
        /// <summary>
        /// The list of the operands in the Content.
        /// </summary>
#endif
        internal IList<Content> ContentList { get; set; }

#if JHELP
        /// <summary>
        /// 被演算子に付加される日付
        /// </summary>
#else
        /// <summary>
        /// The date added to the operand.
        /// </summary>
#endif
        internal int Day { get; set; }
    }

#if JHELP
    /// <summary>
    /// 同意発話ビルダークラス
    /// </summary>
#else
    /// <summary>
    /// Builder class for the utterance of an agreement.
    /// </summary>
#endif
    public class AgreeContentBuilder : ContentBuilder
    {
#if JHELP
        /// <summary>
        /// AgreeContentBuilderクラスの新しいインスタンスを初期化します
        /// </summary>
        /// <param name="subject">同意を表明するエージェント</param>
        /// <param name="utteranceType">同意される発話の種類</param>
        /// <param name="talkDay">同意される発話の発話日</param>
        /// <param name="talkID">同意される発話のID</param>
#else
        /// <summary>
        /// Initializes a new instance of AgreeContentBuilder.
        /// </summary>
        /// <param name="subject">The agent who agrees.</param>
        /// <param name="utteranceType">The type of the agreed utterance.</param>
        /// <param name="talkDay">The day of the utterance agreed with.</param>
        /// <param name="talkID">The ID of the utterance agreed with.</param>
#endif
        public AgreeContentBuilder(Agent subject, UtteranceType utteranceType, int talkDay, int talkID)
        {
            Topic = Topic.AGREE;
            Subject = subject;

            if (utteranceType == UtteranceType.TALK)
            {
                Utterance = new Talk(talkID, talkDay, 0, Agent.NONE, "");
            }
            else if (utteranceType == UtteranceType.WHISPER)
            {
                Utterance = new Whisper(talkID, talkDay, 0, Agent.NONE, "");
            }
        }

#if JHELP
        /// <summary>
        /// AgreeContentBuilderクラスの新しいインスタンスを初期化します
        /// </summary>
        /// <param name="utteranceType">同意される発話の種類.</param>
        /// <param name="talkDay">同意される発話の発話日</param>
        /// <param name="talkID">同意される発話のID</param>
#else
        /// <summary>
        /// Initializes a new instance of AgreeContentBuilder.
        /// </summary>
        /// <param name="utteranceType">The type of the utterance agreed with.</param>
        /// <param name="talkDay">The day of the utterance agreed with.</param>
        /// <param name="talkID">The ID of the utterance agreed with.</param>
#endif
        [Obsolete]
        public AgreeContentBuilder(UtteranceType utteranceType, int talkDay = 0, int talkID = 0)
            : this(Agent.NONE, utteranceType, talkDay, talkID) { }
    }

#if JHELP
    /// <summary>
    /// 不同意発話ビルダークラス
    /// </summary>
#else
    /// <summary>
    /// Builder class for the utterance of a disagreement.
    /// </summary>
#endif
    public class DisagreeContentBuilder : AgreeContentBuilder
    {
#if JHELP
        /// <summary>
        /// DisagreeContentBuilderクラスの新しいインスタンスを初期化します
        /// </summary>
        /// <param name="subject">不同意を表明するエージェント</param>
        /// <param name="utteranceType">不同意される発話の種類</param>
        /// <param name="talkDay">不同意される発話の発話日</param>
        /// <param name="talkID">不同意される発話のID</param>
#else
        /// <summary>
        /// Initializes a new instance of DisagreeContentBuilder.
        /// </summary>
        /// <param name="subject">The agent who disagrees.</param>
        /// <param name="utteranceType">The type of the disagreed utterance.</param>
        /// <param name="talkDay">The date of the disagreed utterance.</param>
        /// <param name="talkID">The ID of the disagreed utterance.</param>
#endif
        public DisagreeContentBuilder(Agent subject, UtteranceType utteranceType, int talkDay, int talkID)
            : base(subject, utteranceType, talkDay, talkID)
        {
            Topic = Topic.DISAGREE;
        }

#if JHELP
        /// <summary>
        /// DisagreeContentBuilderクラスの新しいインスタンスを初期化します
        /// </summary>
        /// <param name="utteranceType">不同意される発話の種類</param>
        /// <param name="talkDay">不同意される発話の発話日</param>
        /// <param name="talkID">不同意される発話のID</param>
#else
        /// <summary>
        /// Initializes a new instance of DisagreeContentBuilder.
        /// </summary>
        /// <param name="utteranceType">The type of the disagreed utterance.</param>
        /// <param name="talkDay">The date of the disagreed utterance.</param>
        /// <param name="talkID">The ID of the disagreed utterance.</param>
#endif
        [Obsolete]
        public DisagreeContentBuilder(UtteranceType utteranceType, int talkDay = 0, int talkID = 0)
            : this(Agent.NONE, utteranceType, talkDay, talkID) { }
    }

#if JHELP
    /// <summary>
    /// 襲撃宣言発話ビルダークラス
    /// </summary>
#else
    /// <summary>
    /// Builder class for the declaration of an attack.
    /// </summary>
#endif
    public class AttackContentBuilder : ContentBuilder
    {
#if JHELP
        /// <summary>
        /// AttackContentBuilderクラスの新しいインスタンスを初期化します
        /// </summary>
        /// <param name="subject">襲撃するエージェント</param>
        /// <param name="target">襲撃されるエージェント</param>
#else
        /// <summary>
        /// Initializes a new instance of AttackContentBuilder.
        /// </summary>
        /// <param name="subject">The agent who will attack..</param>
        /// <param name="target">The agent to be attacked.</param>
#endif
        public AttackContentBuilder(Agent subject, Agent target)
        {
            Topic = Topic.ATTACK;
            Subject = subject;
            Target = target;
        }

#if JHELP
        /// <summary>
        /// AttackContentBuilderクラスの新しいインスタンスを初期化します
        /// </summary>
        /// <param name="target">襲撃されるエージェント</param>
#else
        /// <summary>
        /// Initializes a new instance of AttackContentBuilder.
        /// </summary>
        /// <param name="target">The agent to be attacked.</param>
#endif
        [Obsolete]
        public AttackContentBuilder(Agent target) : this(Agent.NONE, target) { }
    }

#if JHELP
    /// <summary>
    /// 襲撃報告発話ビルダークラス
    /// </summary>
#else
    /// <summary>
    /// Builder class for the report of an attack.
    /// </summary>
#endif
    public class AttackedContentBuilder : AttackContentBuilder
    {
#if JHELP
        /// <summary>
        /// AttackedContentBuilderクラスの新しいインスタンスを初期化します
        /// </summary>
        /// <param name="subject">襲撃したエージェント</param>
        /// <param name="target">襲撃されたエージェント</param>
#else
        /// <summary>
        /// Initializes a new instance of AttackedContentBuilder.
        /// </summary>
        /// <param name="subject">The agent who attacked.</param>
        /// <param name="target">The agent who was attacked.</param>
#endif
        public AttackedContentBuilder(Agent subject, Agent target) : base(subject, target)
        {
            Topic = Topic.ATTACKED;
        }
    }

#if JHELP
    /// <summary>
    /// 占い宣言発話ビルダークラス
    /// </summary>
#else
    /// <summary>
    /// Builder class for the declaration of a divination.
    /// </summary>
#endif
    public class DivinationContentBuilder : AttackContentBuilder
    {
#if JHELP
        /// <summary>
        /// DivinationContentBuilderクラスの新しいインスタンスを初期化します
        /// </summary>
        /// <param name="subject">占いを行うエージェント</param>
        /// <param name="target">占われるエージェント</param>
#else
        /// <summary>
        /// Initializes a new instance of DivinationContentBuilder.
        /// </summary>
        /// <param name="subject">The agent who will divine.</param>
        /// <param name="target">The agent to be divined.</param>
#endif
        public DivinationContentBuilder(Agent subject, Agent target) : base(subject, target)
        {
            Topic = Topic.DIVINATION;
        }

#if JHELP
        /// <summary>
        /// DivinationContentBuilderクラスの新しいインスタンスを初期化します
        /// </summary>
        /// <param name="target">占われるエージェント</param>
#else
        /// <summary>
        /// Initializes a new instance of DivinationContentBuilder.
        /// </summary>
        /// <param name="target">The agent to be divined.</param>
#endif
        [Obsolete]
        public DivinationContentBuilder(Agent target) : this(Agent.NONE, target) { }
    }

#if JHELP
    /// <summary>
    /// 占い報告発話ビルダークラス
    /// </summary>
#else
    /// <summary>
    /// Builder class for the report of a divination.
    /// </summary>
#endif
    public class DivinedResultContentBuilder : ContentBuilder
    {
#if JHELP
        /// <summary>
        /// DivinedResultContentBuilderクラスの新しいインスタンスを初期化します
        /// </summary>
        /// <param name="subject">占いを行ったエージェント</param>
        /// <param name="target">占われたエージェント</param>
        /// <param name="result">占われたエージェントの種族</param>
#else
        /// <summary>
        /// Initializes a new instance of DivinedResultContentBuilder.
        /// </summary>
        /// <param name="subject">The agent who divined.</param>
        /// <param name="target">The agent who was divined.</param>
        /// <param name="result">The species of the divined agent.</param>
#endif
        public DivinedResultContentBuilder(Agent subject, Agent target, Species result)
        {
            Topic = Topic.DIVINED;
            Subject = subject;
            Target = target;
            Result = result;
        }

#if JHELP
        /// <summary>
        /// DivinedResultContentBuilderクラスの新しいインスタンスを初期化します
        /// </summary>
        /// <param name="target">占われたエージェント</param>
        /// <param name="result">占われたエージェントの種族</param>
#else
        /// <summary>
        /// Initializes a new instance of DivinedResultContentBuilder.
        /// </summary>
        /// <param name="target">The agent who was divined.</param>
        /// <param name="result">The species of the divined agent.</param>
#endif
        [Obsolete]
        public DivinedResultContentBuilder(Agent target, Species result) : this(Agent.NONE, target, result) { }
    }

#if JHELP
    /// <summary>
    /// 護衛宣言発話ビルダークラス
    /// </summary>
#else
    /// <summary>
    /// Builder class for the declaration of a guard.
    /// </summary>
#endif
    public class GuardContentBuilder : AttackContentBuilder
    {
#if JHELP
        /// <summary>
        /// GuardContentBuilderクラスの新しいインスタンスを初期化します
        /// </summary>
        /// <param name="subject">護衛を行うエージェント</param>
        /// <param name="target">護衛されるエージェント</param>
#else
        /// <summary>
        /// Initializes a new instance of GuardContentBuilder.
        /// </summary>
        /// <param name="subject">The agent who will guard.</param>
        /// <param name="target">The agent to be guarded.</param>
#endif
        public GuardContentBuilder(Agent subject, Agent target) : base(subject, target)
        {
            Topic = Topic.GUARD;
        }

#if JHELP
        /// <summary>
        /// GuardContentBuilderクラスの新しいインスタンスを初期化します
        /// </summary>
        /// <param name="target">護衛されるエージェント</param>
#else
        /// <summary>
        /// Initializes a new instance of GuardContentBuilder.
        /// </summary>
        /// <param name="target">The agent to be guarded.</param>
#endif
        [Obsolete]
        public GuardContentBuilder(Agent target) : this(Agent.NONE, target) { }
    }

#if JHELP
    /// <summary>
    /// 護衛報告発話ビルダークラス
    /// </summary>
#else
    /// <summary>
    /// Builder class for the report of a guard.
    /// </summary>
#endif
    public class GuardedAgentContentBuilder : AttackContentBuilder
    {
#if JHELP
        /// <summary>
        /// GuardedAgentContentBuilderクラスの新しいインスタンスを初期化します
        /// </summary>
        /// <param name="subject">護衛を行ったエージェント</param>
        /// <param name="target">護衛されたエージェント</param>
#else
        /// <summary>
        /// Initializes a new instance of GuardedAgentContentBuilder.
        /// </summary>
        /// <param name="subject">The agent who guarded.</param>
        /// <param name="target">The agent who was guarded.</param>
#endif
        public GuardedAgentContentBuilder(Agent subject, Agent target) : base(subject, target)
        {
            Topic = Topic.GUARDED;
        }

#if JHELP
        /// <summary>
        /// GuardedAgentContentBuilderクラスの新しいインスタンスを初期化します
        /// </summary>
        /// <param name="target">護衛されたエージェント</param>
#else
        /// <summary>
        /// Initializes a new instance of GuardedAgentContentBuilder.
        /// </summary>
        /// <param name="target">The agent who was guarded.</param>
#endif
        [Obsolete]
        public GuardedAgentContentBuilder(Agent target) : this(Agent.NONE, target) { }
    }

#if JHELP
    /// <summary>
    /// 投票宣言発話ビルダークラス
    /// </summary>
#else
    /// <summary>
    /// Builder class for the declaration of a vote.
    /// </summary>
#endif
    public class VoteContentBuilder : AttackContentBuilder
    {
#if JHELP
        /// <summary>
        /// VoteContentBuilderクラスの新しいインスタンスを初期化します
        /// </summary>
        /// <param name="subject">投票を行うエージェント</param>
        /// <param name="target">投票されるエージェント</param>
#else
        /// <summary>
        /// Initializes a new instance of VoteContentBuilder.
        /// </summary>
        /// <param name="subject">The agent who will vote.</param>
        /// <param name="target">The agent to be voted.</param>
#endif
        public VoteContentBuilder(Agent subject, Agent target) : base(subject, target)
        {
            Topic = Topic.VOTE;
        }

#if JHELP
        /// <summary>
        /// VoteContentBuilderクラスの新しいインスタンスを初期化します
        /// </summary>
        /// <param name="target">投票されるエージェント</param>
#else
        /// <summary>
        /// Initializes a new instance of VoteContentBuilder.
        /// </summary>
        /// <param name="target">The agent to be voted.</param>
#endif
        [Obsolete]
        public VoteContentBuilder(Agent target) : this(Agent.NONE, target) { }
    }

#if JHELP
    /// <summary>
    /// 投票報告発話ビルダークラス
    /// </summary>
#else
    /// <summary>
    /// Builder class for the report of a vote.
    /// </summary>
#endif
    public class VotedContentBuilder : AttackContentBuilder
    {
#if JHELP
        /// <summary>
        /// VotedContentBuilderクラスの新しいインスタンスを初期化します
        /// </summary>
        /// <param name="subject">投票を行ったエージェント</param>
        /// <param name="target">投票されたエージェント</param>
#else
        /// <summary>
        /// Initializes a new instance of VotedContentBuilder.
        /// </summary>
        /// <param name="subject">The agent who voted.</param>
        /// <param name="target">The agent who was voted.</param>
#endif
        public VotedContentBuilder(Agent subject, Agent target) : base(subject, target)
        {
            Topic = Topic.VOTED;
        }
    }

#if JHELP
    /// <summary>
    /// カミングアウト発話ビルダークラス
    /// </summary>
#else
    /// <summary>
    /// Builder class for the utterance of a comingout.
    /// </summary>
#endif
    public class ComingoutContentBuilder : ContentBuilder
    {
#if JHELP
        /// <summary>
        /// ComingoutContentBuilderクラスの新しいインスタンスを初期化します
        /// </summary>
        /// <param name="subject">役職公開を行うエージェント</param>
        /// <param name="target">役職を公開されるエージェント</param>
        /// <param name="role">公開される役職</param>
#else
        /// <summary>
        /// Initializes a new instance of ComingoutContentBuilder.
        /// </summary>
        /// <param name="subject">The agent who discloses.</param>
        /// <param name="target">The disclosed agent.</param>
        /// <param name="role">The disclosed role.</param>
#endif
        public ComingoutContentBuilder(Agent subject, Agent target, Role role)
        {
            Topic = Topic.COMINGOUT;
            Subject = subject;
            Target = target;
            Role = role;
        }

#if JHELP
        /// <summary>
        /// ComingoutContentBuilderクラスの新しいインスタンスを初期化します
        /// </summary>
        /// <param name="target">役職を公開されるエージェント</param>
        /// <param name="role">公開される役職</param>
#else
        /// <summary>
        /// Initializes a new instance of ComingoutContentBuilder.
        /// </summary>
        /// <param name="target">The agent whose role is disclosed.</param>
        /// <param name="role">The disclosed role.</param>
#endif
        [Obsolete]
        public ComingoutContentBuilder(Agent target, Role role) : this(Agent.NONE, target, role) { }
    }

#if JHELP
    /// <summary>
    /// 推測発話ビルダークラス
    /// </summary>
#else
    /// <summary>
    /// Builder class for the utterance of a estimate.
    /// </summary>
#endif
    public class EstimateContentBuilder : ComingoutContentBuilder
    {
#if JHELP
        /// <summary>
        /// EstimateContentBuilderクラスの新しいインスタンスを初期化します
        /// </summary>
        /// <param name="subject">推測を行うエージェント</param>
        /// <param name="target">推測されるエージェント</param>
        /// <param name="role">推測される役職</param>
#else
        /// <summary>
        /// Initializes a new instance of EstimateContentBuilder.
        /// </summary>
        /// <param name="subject">The agent who estimates.</param>
        /// <param name="target">The estimated agent.</param>
        /// <param name="role">The estimated role.</param>
#endif
        public EstimateContentBuilder(Agent subject, Agent target, Role role)
            : base(subject, target, role)
        {
            Topic = Topic.ESTIMATE;
        }

#if JHELP
        /// <summary>
        /// EstimateContentBuilderクラスの新しいインスタンスを初期化します
        /// </summary>
        /// <param name="target">推測されるエージェント</param>
        /// <param name="role">推測される役職</param>
#else
        /// <summary>
        /// Initializes a new instance of EstimateContentBuilder.
        /// </summary>
        /// <param name="target">The estimated agent.</param>
        /// <param name="role">The estimated role.</param>
#endif
        [Obsolete]
        public EstimateContentBuilder(Agent target, Role role) : this(Agent.NONE, target, role) { }
    }

#if JHELP
    /// <summary>
    /// 霊媒結果報告ビルダークラス
    /// </summary>
#else
    /// <summary>
    /// Builder class for the report of an identification.
    /// </summary>
#endif
    public class IdentContentBuilder : DivinedResultContentBuilder
    {
#if JHELP
        /// <summary>
        /// IdentContentBuilderクラスの新しいインスタンスを初期化します
        /// </summary>
        /// <param name="subject">種族確認を行ったエージェント</param>
        /// <param name="target">種族確認をされたエージェント</param>
        /// <param name="result">確認されたエージェントの種族</param>
#else
        /// <summary>
        /// Initializes a new instance of IdentContentBuilder.
        /// </summary>
        /// <param name="agents">The agent who identified.</param>
        /// <param name="agents">The agent who was identified.</param>
        /// <param name="result">The species of the identified agent.</param>
#endif
        public IdentContentBuilder(Agent subject, Agent target, Species result)
            : base(subject, target, result)
        {
            Topic = Topic.IDENTIFIED;
        }

#if JHELP
        /// <summary>
        /// IdentContentBuilderクラスの新しいインスタンスを初期化します
        /// </summary>
        /// <param name="target">種族確認をされたエージェント</param>
        /// <param name="result">確認されたエージェントの種族</param>
#else
        /// <summary>
        /// Initializes a new instance of IdentContentBuilder.
        /// </summary>
        /// <param name="target">The identified agent.</param>
        /// <param name="result">The species of the identified agent.</param>
#endif
        [Obsolete]
        public IdentContentBuilder(Agent target, Species result) : this(Agent.NONE, target, result) { }
    }

#if JHELP
    /// <summary>
    /// 要求発話ビルダークラス
    /// </summary>
#else
    /// <summary>
    /// Builder class for a request.
    /// </summary>
#endif
    public class RequestContentBuilder : ContentBuilder
    {
#if JHELP
        /// <summary>
        /// RequestContentBuilderクラスの新しいインスタンスを初期化します
        /// </summary>
        /// <param name="subject">要求を行うエージェント</param>
        /// <param name="target">要求されるエージェント</param>
        /// <param name="action">要求されるアクション</param>
#else
        /// <summary>
        /// Initializes a new instance of RequestContentBuilder.
        /// </summary>
        /// <param name="subject">The agent who requests.</param>
        /// <param name="target">The requested agent.</param>
        /// <param name="content">The requested action.</param>
#endif
        public RequestContentBuilder(Agent subject, Agent target, Content action)
        {
            Topic = Topic.OPERATOR;
            Operator = Operator.REQUEST;
            Subject = subject;
            Target = target;
            ContentList = action == null ? null : new List<Content> { action }.AsReadOnly();
        }

#if JHELP
        /// <summary>
        /// RequestContentBuilderクラスの新しいインスタンスを初期化します
        /// </summary>
        /// <param name="target">要求をされるエージェント</param>
        /// <param name="action">要求されるアクション</param>
#else
        /// <summary>
        /// Initializes a new instance of RequestContentBuilder.
        /// </summary>
        /// <param name="target">The requested agent.</param>
        /// <param name="action">The requested action.</param>
#endif
        [Obsolete]
        public RequestContentBuilder(Agent target, Content action) : this(Agent.NONE, target, action) { }
    }

#if JHELP
    /// <summary>
    /// 照会発話ビルダークラス
    /// </summary>
#else
    /// <summary>
    /// Builder class for a inquiry.
    /// </summary>
#endif
    public class InquiryContentBuilder : RequestContentBuilder
    {
#if JHELP
        /// <summary>
        /// InquiryContentBuilderクラスの新しいインスタンスを初期化します
        /// </summary>
        /// <param name="subject">照会を行うエージェント</param>
        /// <param name="target">照会されるエージェント</param>
        /// <param name="content">照会事項</param>
#else
        /// <summary>
        /// Initializes a new instance of InquiryContentBuilder.
        /// </summary>
        /// <param name="subject">The agent who inquires.</param>
        /// <param name="target">The inquired agent.</param>
        /// <param name="content">The inquired matter.</param>
#endif
        public InquiryContentBuilder(Agent subject, Agent target, Content content)
            : base(subject, target, content)
        {
            Operator = Operator.INQUIRE;
        }
    }

#if JHELP
    /// <summary>
    /// 理由発話ビルダークラス
    /// </summary>
#else
    /// <summary>
    /// Builder class for the utterance of a reason.
    /// </summary>
#endif
    public class BecauseContentBuilder : ContentBuilder
    {
#if JHELP
        /// <summary>
        /// BecauseContentBuilderクラスの新しいインスタンスを初期化します
        /// </summary>
        /// <param name="subject">行動とその理由を述べるエージェント</param>
        /// <param name="reason">行動の理由</param>
        /// <param name="action">理由に基づく行動</param>
#else
        /// <summary>
        /// Initializes a new instance of BecauseContentBuilder.
        /// </summary>
        /// <param name="subject">The agent who express the action and its reason.</param>
        /// <param name="reason">The reason.</param>
        /// <param name="action">The action.</param>
#endif
        public BecauseContentBuilder(Agent subject, Content reason, Content action)
        {
            Topic = Topic.OPERATOR;
            Operator = Operator.BECAUSE;
            Subject = subject;

            if (reason == null)
            {
                Error.RuntimeError("Invalid argument: reason is null");
            }
            if (action == null)
            {
                Error.RuntimeError("Invalid argument: action is null");
            }
            ContentList = (reason == null || action == null) ? null : new List<Content> { reason, action }.AsReadOnly();
        }
    }

#if JHELP
    /// <summary>
    /// AND演算子ビルダークラス
    /// </summary>
#else
    /// <summary>
    /// Builder class for AND operator.
    /// </summary>
#endif
    public class AndContentBuilder : ContentBuilder
    {
#if JHELP
        /// <summary>
        /// AndContentBuilderクラスの新しいインスタンスを初期化します
        /// </summary>
        /// <param name="subject">この連言を主張するエージェント</param>
        /// <param name="contents">連言肢の並び</param>
#else
        /// <summary>
        /// Initializes a new instance of AndContentBuilder.
        /// </summary>
        /// <param name="subject">The agent who states this conjunction.</param>
        /// <param name="contents">The series of the conjuncts.</param>
#endif
        public AndContentBuilder(Agent subject, params Content[] contents)
        {
            Topic = Topic.OPERATOR;
            Operator = Operator.AND;
            Subject = subject;
            if (contents.Length == 0)
            {
                Error.RuntimeError("Invalid argument: contents is empty");
            }
            ContentList = contents.Length == 0 ? null : new List<Content>(contents).AsReadOnly();
        }
    }

#if JHELP
    /// <summary>
    /// OR演算子ビルダークラス
    /// </summary>
#else
    /// <summary>
    /// Builder class for OR operator.
    /// </summary>
#endif
    public class OrContentBuilder : AndContentBuilder
    {
#if JHELP
        /// <summary>
        /// OrContentBuilderクラスの新しいインスタンスを初期化します
        /// </summary>
        /// <param name="subject">この選言を主張するエージェント</param>
        /// <param name="contents">選言肢の並び</param>
#else
        /// <summary>
        /// Initializes a new instance of OrContentBuilder.
        /// </summary>
        /// <param name="subject">The agent who states this disjunction.</param>
        /// <param name="contents">The series of the disjuncts.</param>
#endif
        public OrContentBuilder(Agent subject, params Content[] contents) : base(subject, contents)
        {
            Operator = Operator.OR;
        }
    }

#if JHELP
    /// <summary>
    /// XOR演算子ビルダークラス
    /// </summary>
#else
    /// <summary>
    /// Builder class for XOR operator.
    /// </summary>
#endif
    public class XorContentBuilder : BecauseContentBuilder
    {
#if JHELP
        /// <summary>
        /// XorContentBuilderクラスの新しいインスタンスを初期化します
        /// </summary>
        /// <param name="subject">この排他的選言を主張するエージェント</param>
        /// <param name="disjunct1">第1選言肢</param>
        /// <param name="disjunct2">第2選言肢</param>
#else
        /// <summary>
        /// Initializes a new instance of XorContentBuilder.
        /// </summary>
        /// <param name="subject">The agent who states this exclusive disjunction.</param>
        /// <param name="disjunct1">The first disjunct.</param>
        /// <param name="disjunct2">The second disjunct.</param>
#endif
        public XorContentBuilder(Agent subject, Content disjunct1, Content disjunct2)
            : base(subject, disjunct1, disjunct2)
        {
            Operator = Operator.XOR;
        }
    }

#if JHELP
    /// <summary>
    /// NOT演算子ビルダークラス
    /// </summary>
#else
    /// <summary>
    /// Builder class for NOT operator.
    /// </summary>
#endif
    public class NotContentBuilder : ContentBuilder
    {
#if JHELP
        /// <summary>
        /// NotContentBuilderクラスの新しいインスタンスを初期化します
        /// </summary>
        /// <param name="subject">この否定を主張するエージェント</param>
        /// <param name="content">否定される命題</param>
#else
        /// <summary>
        /// Initializes a new instance of NotContentBuilder.
        /// </summary>
        /// <param name="subject">The agent who states this negation.</param>
        /// <param name="content">The negated proposition.</param>
#endif
        public NotContentBuilder(Agent subject, Content content)
        {
            Topic = Topic.OPERATOR;
            Operator = Operator.NOT;
            Subject = subject;
            ContentList = content == null ? null : new List<Content> { content }.AsReadOnly();
        }
    }

#if JHELP
    /// <summary>
    /// DAY演算子ビルダークラス
    /// </summary>
#else
    /// <summary>
    /// Builder class for DAY operator.
    /// </summary>
#endif
    public class DayContentBuilder : ContentBuilder
    {
#if JHELP
        /// <summary>
        /// DayContentBuilderクラスの新しいインスタンスを初期化します
        /// </summary>
        /// <param name="subject">日付を付加するエージェント</param>
        /// <param name="day">日付</param>
        /// <param name="content">日付を付加される発話</param>
#else
        /// <summary>
        /// Initializes a new instance of DayContentBuilder.
        /// </summary>
        /// <param name="subject">The agent who adds the date to the Content.</param>
        /// <param name="day">The date added to the Content.</param>
        /// <param name="content">The Content whose date is added.</param>
#endif
        public DayContentBuilder(Agent subject, int day, Content content)
        {
            Topic = Topic.OPERATOR;
            Operator = Operator.DAY;
            Subject = subject;
            Day = day;
            ContentList = content == null ? null : new List<Content> { content }.AsReadOnly();
        }
    }

#if JHELP
    /// <summary>
    /// SKIPビルダークラス
    /// </summary>
#else
    /// <summary>
    /// Builder class for SKIP.
    /// </summary>
#endif
    public class SkipContentBuilder : ContentBuilder
    {
        public SkipContentBuilder()
        {
            Topic = Topic.Skip;
        }
    }

#if JHELP
    /// <summary>
    /// OVERビルダークラス
    /// </summary>
#else
    /// <summary>
    /// Builder class for OVER.
    /// </summary>
#endif
    public class OverContentBuilder : ContentBuilder
    {
        public OverContentBuilder()
        {
            Topic = Topic.Over;
        }
    }
}
