//
// Content.cs
//
// Copyright (c) 2017 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//


using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AIWolf.Lib
{
#if JHELP
    /// <summary>
    /// Contentクラス
    /// </summary>
#else
    /// <summary>
    /// Content class.
    /// </summary>
#endif
    public class Content
    {
#if JHELP
        /// <summary>
        /// 定数SKIP
        /// </summary>
#else
        /// <summary>
        /// Constant SKIP.
        /// </summary>
#endif
        public static readonly Content SKIP = new Content(new SkipContentBuilder());

#if JHELP
        /// <summary>
        /// 定数OVER
        /// </summary>
#else
        /// <summary>
        /// Constant OVER.
        /// </summary>
#endif
        public static readonly Content OVER = new Content(new OverContentBuilder());

#if JHELP
        /// <summary>
        /// Contentのテキスト表現
        /// </summary>
#else
        /// <summary>
        /// The text representation of the Content.
        /// </summary>
#endif
        public string Text { get; internal set; }

#if JHELP
        /// <summary>
        /// Contentのトピック
        /// </summary>
#else
        /// <summary>
        /// The topic of the Content.
        /// </summary>
#endif
        public Topic Topic { get; }

#if JHELP
        /// <summary>
        /// Contentの主語
        /// </summary>
#else
        /// <summary>
        /// The subject of the Content.
        /// </summary>
#endif
        public Agent Subject { get; internal set; }

#if JHELP
        /// <summary>
        /// Contentの対象エージェント
        /// </summary>
#else
        /// <summary>
        /// The target agent of the Content.
        /// </summary>
#endif
        public Agent Target { get; }

#if JHELP
        /// <summary>
        /// Contentが言及する役職
        /// </summary>
#else
        /// <summary>
        /// The role the Content refers to.
        /// </summary>
#endif
        public Role Role { get; }

#if JHELP
        /// <summary>
        /// Contentが言及する種族
        /// </summary>
#else
        /// <summary>
        /// The species the Content refers to.
        /// </summary>
#endif
        public Species Result { get; }

#if JHELP
        /// <summary>
        /// Contentが言及する発話
        /// </summary>
#else
        /// <summary>
        /// The utterance the Content refers to.
        /// </summary>
#endif
        public Utterance Utterance { get; }

#if JHELP
        /// <summary>
        /// Content中の演算子
        /// </summary>
#else
        /// <summary>
        /// The operator in the Content.
        /// </summary>
#endif
        public Operator Operator { get; }

#if JHELP
        /// <summary>
        /// Content中の被演算子リスト
        /// </summary>
#else
        /// <summary>
        /// The list of the operands in the Content.
        /// </summary>
#endif
        public List<Content> ContentList { get; }

#if JHELP
        /// <summary>
        /// コピーコンストラクタ
        /// </summary>
        /// <param name="content">オリジナルのContent</param>
#else
        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="content">Original content.</param>
#endif
        public Content(Content content)
        {
            Text = content.Text;
            Topic = content.Topic;
            Subject = content.Subject;
            Target = content.Target;
            Role = content.Role;
            Result = content.Result;
            Utterance = content.Utterance;
            Operator = content.Operator;
            ContentList = content.ContentList;
        }

#if JHELP
        /// <summary>
        /// Contentクラスの新しいインスタンスを初期化する
        /// </summary>
        /// <param name="builder">発話内容に応じたContentBuilder</param>
#else
        /// <summary>
        /// Initializes a new instance of Content class.
        /// </summary>
        /// <param name="builder">ContentBuildr for the content.</param>
#endif
        public Content(ContentBuilder builder)
        {
            Text = builder.Text;
            Topic = builder.Topic;
            Subject = builder.Subject;
            Target = builder.Target;
            Role = builder.Role;
            Result = builder.Result;
            Utterance = builder.Utterance;
            Operator = builder.Operator;
            ContentList = builder.ContentList;
        }

#if JHELP
        /// <summary>
        /// Contentクラスの新しいインスタンスを初期化する
        /// </summary>
        /// <param name="text">発話テキスト</param>
#else
        /// <summary>
        /// Initializes a new instance of Content class.
        /// </summary>
        /// <param name="text">The uttered text.</param>
#endif
        public Content(string text)
        {
            Text = text;
            string sentence = GetSentence(Text);
            if (sentence != null) // Complex sentence.
            {
                Topic = Topic.OPERATOR;
                Operator = Operator.REQUEST;
                ContentList = new List<Content>();
                ContentList.Add(new Content(sentence));
            }
            else // Simple sentence.
            {
                string[] split = Text.Split();
                int offset = 0;
                if (split[0].StartsWith("Agent"))
                {
                    Subject = Agent.GetAgent(GetInt(split[0]));
                    offset = 1;
                }
                Topic topic;
                if (!Enum.TryParse(split[0 + offset], out topic))
                {
                    throw new AIWolfLibException("Content: Can't find any topic in " + split[0 + offset]);
                }
                Topic = topic;
                if (split.Length >= 2 + offset && split[1 + offset].StartsWith("Agent"))
                {
                    Target = Agent.GetAgent(GetInt(split[1 + offset]));
                }
                switch (Topic)
                {
                    case Topic.Skip:
                    case Topic.Over:
                        break;
                    case Topic.AGREE:
                    case Topic.DISAGREE:
                        if (split[1 + offset].Equals("TALK"))
                        {
                            Utterance = new Talk(GetInt(split[3 + offset]), GetInt(split[2 + offset]));
                        }
                        else
                        {
                            Utterance = new Whisper(GetInt(split[3 + offset]), GetInt(split[2 + offset]));
                        }
                        break;
                    case Topic.ESTIMATE:
                    case Topic.COMINGOUT:
                        Role role;
                        if (!Enum.TryParse(split[2 + offset], out role))
                        {
                            throw new AIWolfLibException("Content: Can't find any role in " + split[2 + offset]);
                        }
                        Role = role;
                        break;
                    case Topic.DIVINED:
                    case Topic.IDENTIFIED:
                        Species species;
                        if (!Enum.TryParse(split[2 + offset], out species))
                        {
                            throw new AIWolfLibException("Content: Can't find any species in " + split[2 + offset]);
                        }
                        Result = species;
                        break;
                    case Topic.ATTACK:
                    case Topic.DIVINATION:
                    case Topic.GUARD:
                    case Topic.GUARDED:
                    case Topic.VOTE:
                        break;
                    default:
                        break;
                }
            }
        }

        static readonly Regex regexGetInt = new Regex(@"-?[\d]+");

        /// <summary>
        /// Finds integer value from the given text.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <returns>Integer value if found, otherwise -1.</returns>
        static int GetInt(string text)
        {
            var m = regexGetInt.Match(text);
            if (m.Success)
            {
                return int.Parse(m.Value);
            }
            return -1;
        }

        static readonly Regex regexGetSentence = new Regex(@"REQUEST\((.+?)\)");

        static string GetSentence(string text)
        {
            var m = regexGetSentence.Match(text);
            if (m.Success)
            {
                return m.Groups[1].ToString();
            }
            return null;
        }
    }

#if JHELP
    /// <summary>
    /// 会話/囁きのトピック
    /// </summary>
#else
    /// <summary>
    /// Enumeration type for topic of talk/whisper.
    /// </summary>
#endif
    public enum Topic
    {
#if JHELP
        /// <summary>
        /// ダミートピック
        /// </summary>
#else
        /// <summary>
        /// Dummy topic.
        /// </summary>
#endif
        DUMMY,

#if JHELP
        /// <summary>
        /// 役職の推定
        /// </summary>
#else
        /// <summary>
        /// Estimation.
        /// </summary>
#endif
        ESTIMATE,

#if JHELP
        /// <summary>
        /// カミングアウト
        /// </summary>
#else
        /// <summary>
        /// Comingout.
        /// </summary>
#endif
        COMINGOUT,

#if JHELP
        /// <summary>
        /// 占い行為
        /// </summary>
#else
        /// <summary>
        /// Divination.
        /// </summary>
#endif
        DIVINATION,

#if JHELP
        /// <summary>
        /// 占い結果の報告
        /// </summary>
#else
        /// <summary>
        /// Report of a divination.
        /// </summary>
#endif
        DIVINED,

#if JHELP
        /// <summary>
        /// 霊媒結果の報告
        /// </summary>
#else
        /// <summary>
        /// Report of an identification.
        /// </summary>
#endif
        IDENTIFIED,

#if JHELP
        /// <summary>
        /// 護衛行為
        /// </summary>
#else
        /// <summary>
        /// Guard.
        /// </summary>
#endif
        GUARD,

#if JHELP
        /// <summary>
        /// 護衛先の報告
        /// </summary>
#else
        /// <summary>
        /// Report of a guard.
        /// </summary>
#endif
        GUARDED,

#if JHELP
        /// <summary>
        /// 投票先の表明
        /// </summary>
#else
        /// <summary>
        /// Vote.
        /// </summary>
#endif
        VOTE,

#if JHELP
        /// <summary>
        /// 襲撃先の表明
        /// </summary>
#else
        /// <summary>
        /// Attack.
        /// </summary>
#endif
        ATTACK,

#if JHELP
        /// <summary>
        /// 同意
        /// </summary>
#else
        /// <summary>
        /// Agreement.
        /// </summary>
#endif
        AGREE,

#if JHELP
        /// <summary>
        /// 不同意
        /// </summary>
#else
        /// <summary>
        /// Disagreement.
        /// </summary>
#endif
        DISAGREE,

#if JHELP
        /// <summary>
        /// 話す/囁くことはない
        /// </summary>
#else
        /// <summary>
        /// There is nothing to talk/whisper.
        /// </summary>
#endif
        Over,

#if JHELP
        /// <summary>
        /// 話す/囁くことはあるがこのターンはスキップ
        /// </summary>
#else
        /// <summary>
        /// Skip this turn though there is something to talk/whisper.
        /// </summary>
#endif
        Skip,

#if JHELP
        /// <summary>
        /// 演算子（正確にはトピックではない）
        /// </summary>
#else
        /// <summary>
        /// Operator.
        /// </summary>
#endif
        OPERATOR
    }

#if JHELP
    /// <summary>
    /// 演算子
    /// </summary>
#else
    /// <summary>
    /// Enumeration type for operator.
    /// </summary>
#endif
    public enum Operator
    {
#if JHELP
        /// <summary>
        /// 何もしない
        /// </summary>
#else
        /// <summary>
        /// No operation.
        /// </summary>
#endif
        NOP,

#if JHELP
        /// <summary>
        /// 行動の要求
        /// </summary>
#else
        /// <summary>
        /// Request for the action.
        /// </summary>
#endif
        REQUEST,

#if JHELP
        /// <summary>
        /// 行動の理由
        /// </summary>
#else
        /// <summary>
        /// Reason for the action.
        /// </summary>
#endif
        BECAUSE,

#if JHELP
        /// <summary>
        /// AND
        /// </summary>
#else
        /// <summary>
        /// AND.
        /// </summary>
#endif
        AND,

#if JHELP
        /// <summary>
        /// OR
        /// </summary>
#else
        /// <summary>
        /// OR.
        /// </summary>
#endif
        OR
    }

}
