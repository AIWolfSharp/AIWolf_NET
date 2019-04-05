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
using System.Linq;
using System.Text.RegularExpressions;

namespace AIWolf.Lib
{
#if JHELP
    /// <summary>
    /// 発話内容クラス
    /// </summary>
#else
    /// <summary>
    /// Content class.
    /// </summary>
#endif
    public class Content : IEquatable<Content>
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
        public string Text { get; private set; }

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
        public Agent Subject { get; private set; }

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
        public IUtterance Utterance { get; }

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
        public IList<Content> ContentList { get; private set; }

#if JHELP
        /// <summary>
        /// Content中の被演算子に付加される日付
        /// </summary>
#else
        /// <summary>
        /// The date added to the operand int the Content.
        /// </summary>
#endif
        public int Day { get; }

        static readonly Regex regexAgent = new Regex(@"(Agent\[(\d+)\]|ANY)");

        // Converts the string representation of Agent into Agent.
        static Agent ToAgent(string input)
        {
            var m = regexAgent.Match(input);
            if (m.Success)
            {
                if (m.Groups[1].ToString() == "ANY")
                {
                    return Agent.ANY;
                }
                else
                {
                    return Agent.GetAgent(int.Parse(m.Groups[2].ToString()));
                }
            }
            return Agent.NONE;
        }

        static readonly Regex regexContents = new Regex(@"^[^\(\)]*(?:\(([^\(\)]*(((?<Open>\()[^\(\)]*)+((?<Close-Open>\))[^\(\)]*)+)*?(?(Open)(?!)))\)[^\(\)]*)*?$");

        // Converts the string representaion of the sequence of contents into IList<Content>.
        static IList<Content> GetContents(string input)
        {
            var m = regexContents.Match(input);
            if (m.Success)
            {
                var contents = new List<Content>();
                foreach (Capture c in m.Groups[1].Captures)
                {
                    contents.Add(new Content(c.Value));
                }
                return contents.AsReadOnly();
            }
            return null;
        }

        // Completes the subjects of the inner Contents.
        void CompleteInnerSubject()
        {
            if (ContentList == null)
            {
                return;
            }
            ContentList = ContentList.Select(c =>
            {
                if (c.Subject == Agent.NONE)
                {
                    if (Operator == Operator.INQUIRE || Operator == Operator.REQUEST)
                    {
                        return c.CopyAndReplaceSubject(Target);
                    }
                    if (Subject != Agent.NONE)
                    {
                        return c.CopyAndReplaceSubject(Subject);
                    }
                }
                c.CompleteInnerSubject();
                return c;
            }).ToList().AsReadOnly();
        }

        // Returns the copy of the Content whose Subject is replaced with newSubject.
        Content CopyAndReplaceSubject(Agent newSubject)
        {
            Content c = new Content(this)
            {
                Subject = newSubject
            };
            c.CompleteInnerSubject();
            c.NormalizeText();
            return c;
        }

        // Normalize Text.
        void NormalizeText()
        {
            switch (Topic)
            {
                case Topic.Skip:
                    Text = Talk.SKIP;
                    break;
                case Topic.Over:
                    Text = Talk.OVER;
                    break;
                case Topic.AGREE:
                case Topic.DISAGREE:
                    Text = (Subject == Agent.NONE ? "" : Subject == Agent.ANY ? "ANY " : $"{Subject.ToString()} ")
                + string.Join(" ", new string[] {
                        Topic.ToString(),
                        Utterance is Whisper ? "WHISPER" : "TALK",
                        $"day{Utterance.Day}",
                        $"ID:{Utterance.Idx}"
                    });
                    break;
                case Topic.ESTIMATE:
                case Topic.COMINGOUT:
                    Text = (Subject == Agent.NONE ? "" : Subject == Agent.ANY ? "ANY " : $"{Subject.ToString()} ")
                + string.Join(" ", new string[] {
                        Topic.ToString(),
                        Target == Agent.NONE ? "ANY" : Target.ToString(),
                        Role.ToString()
                    });
                    break;
                case Topic.DIVINED:
                case Topic.IDENTIFIED:
                    Text = (Subject == Agent.NONE ? "" : Subject == Agent.ANY ? "ANY " : $"{Subject.ToString()} ")
                + string.Join(" ", new string[] {
                        Topic.ToString(),
                        Target == Agent.NONE ? "ANY" : Target.ToString(),
                        Result.ToString()
                    });
                    break;
                case Topic.ATTACK:
                case Topic.ATTACKED:
                case Topic.DIVINATION:
                case Topic.GUARD:
                case Topic.GUARDED:
                case Topic.VOTE:
                case Topic.VOTED:
                    Text = (Subject == Agent.NONE ? "" : Subject == Agent.ANY ? "ANY " : $"{Subject.ToString()} ")
                + string.Join(" ", new string[] {
                        Topic.ToString(),
                        Target == Agent.NONE ? "ANY" : Target.ToString(),
                    });
                    break;
                case Topic.OPERATOR:
                    switch (Operator)
                    {
                        case Operator.REQUEST:
                        case Operator.INQUIRE:
                            Text = (Subject == Agent.NONE ? "" : Subject == Agent.ANY ? "ANY " : $"{Subject.ToString()} ")
                                        + string.Join(" ", new string[] {
                                Operator.ToString(),
                                Target == Agent.NONE ? "ANY" : Target.ToString(),
                                $"({(ContentList[0]?.Subject == Target ? StripSubject(ContentList[0].Text) : ContentList[0]?.Text)})"
                            });
                            break;
                        case Operator.BECAUSE:
                        case Operator.XOR:
                            Text = (Subject == Agent.NONE ? "" : Subject == Agent.ANY ? "ANY " : $"{Subject.ToString()} ")
                                        + string.Join(" ", new string[] {
                                Operator.ToString(),
                                $"({(ContentList[0]?.Subject == Subject ? StripSubject(ContentList[0].Text) : ContentList[0]?.Text)})",
                                $"({(ContentList[1]?.Subject == Subject ? StripSubject(ContentList[1].Text) : ContentList[1]?.Text)})"
                            });
                            break;
                        case Operator.AND:
                        case Operator.OR:
                            Text = (Subject == Agent.NONE ? "" : Subject == Agent.ANY ? "ANY " : $"{Subject.ToString()} ")
                                        + string.Join(" ", new string[] {
                                Operator.ToString(),
                                string.Join(" ", ContentList.Select(c => $"({(c.Subject == Subject ? StripSubject(c.Text) : c.Text)})").ToArray())
                            });
                            break;
                        case Operator.NOT:
                            Text = (Subject == Agent.NONE ? "" : Subject == Agent.ANY ? "ANY " : $"{Subject.ToString()} ")
                                        + string.Join(" ", new string[] {
                                Operator.ToString(),
                                $"({(ContentList[0].Subject == Subject ? StripSubject(ContentList[0].Text) : ContentList[0].Text)})"
                            });
                            break;
                        case Operator.DAY:
                            Text = (Subject == Agent.NONE ? "" : Subject == Agent.ANY ? "ANY " : $"{Subject.ToString()} ")
                                        + string.Join(" ", new string[] {
                                Operator.ToString(),
                                Day.ToString(),
                                $"({(ContentList[0].Subject == Subject ? StripSubject(ContentList[0].Text) : ContentList[0].Text)})"
                            });
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;

            }
        }

        static readonly Regex regexStripSubject = new Regex(@"^(Agent\[\d+\]|ANY|)\s*(\p{Lu}+.*)$");

#if JHELP
        /// <summary>
        /// 発話テキストから主語を除く
        /// </summary>
        /// <param name="input">発話テキスト</param>
        /// <returns>主語が除かれたテキスト</returns>
#else
        /// <summary>
        /// Remove subject from the uttered text.
        /// </summary>
        /// <param name="input">The uttered text.</param>
        /// <returns>The text with no subject.</returns>
#endif
        public static string StripSubject(string input)
        {
            var m = regexStripSubject.Match(input);
            if (m.Success)
            {
                return m.Groups[2].ToString();
            }
            return input;
        }

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
        internal Content(Content content)
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
            Day = content.Day;
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
            Topic = builder.Topic;
            Subject = builder.Subject;
            Target = builder.Target;
            Role = builder.Role;
            Result = builder.Result;
            Utterance = builder.Utterance;
            Operator = builder.Operator;
            ContentList = builder.ContentList;
            Day = builder.Day;
            CompleteInnerSubject();
            NormalizeText();
        }

        static readonly string agentPattern = @"\s+(Agent\[\d+\]|ANY)";
        static readonly string subjectPattern = @"^(Agent\[\d+\]|ANY|)\s*";
        static readonly string talkPattern = @"\s+(TALK|WHISPER)\s+day(-?\d+)\s+ID:(-?\d+)";
        static readonly string roleSpecPattern = @"\s+(BODYGUARD|FOX|FREEMASON|MEDIUM|POSSESSED|SEER|VILLAGER|WEREWOLF|HUMAN|ANY)";
        static readonly string parenPattern = @"(\(.*\))";
        static readonly string digitPattern = @"(\d+)";
        static readonly Regex regexAgree = new Regex(subjectPattern + "(AGREE|DISAGREE)" + talkPattern + "$");
        static readonly Regex regexEstimate = new Regex(subjectPattern + "(ESTIMATE|COMINGOUT)" + agentPattern + roleSpecPattern + "$");
        static readonly Regex regexDivined = new Regex(subjectPattern + "(DIVINED|IDENTIFIED)" + agentPattern + roleSpecPattern + "$");
        static readonly Regex regexAttack = new Regex(subjectPattern + "(ATTACK|ATTACKED|DIVINATION|GUARD|GUARDED|VOTE|VOTED)" + agentPattern + "$");
        static readonly Regex regexRequest = new Regex(subjectPattern + "(REQUEST|INQUIRE)" + agentPattern + @"\s+" + parenPattern + "$");
        static readonly Regex regexBecause = new Regex(subjectPattern + @"(BECAUSE|AND|OR|XOR|NOT|REQUEST)\s+" + parenPattern + "$");
        static readonly Regex regexDay = new Regex(subjectPattern + @"DAY\s+" + digitPattern + @"\s+" + parenPattern + "$");
        static readonly Regex regexSkip = new Regex("^(Skip|Over)$");

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
            var trimmed = text.Trim();
            var m = regexSkip.Match(trimmed);
            if (m.Success)
            {
                Topic = (Topic)Enum.Parse(typeof(Topic), m.Groups[1].ToString());
            }
            else if ((m = regexAgree.Match(trimmed)).Success)
            {
                Subject = ToAgent(m.Groups[1].ToString());
                Topic = (Topic)Enum.Parse(typeof(Topic), m.Groups[2].ToString());
                if (m.Groups[3].ToString() == "TALK")
                {
                    Utterance = new Talk(int.Parse(m.Groups[4].ToString()), int.Parse(m.Groups[5].ToString()), 0, Agent.NONE, "");
                }
                else
                {
                    Utterance = new Whisper(int.Parse(m.Groups[4].ToString()), int.Parse(m.Groups[5].ToString()), 0, Agent.NONE, "");
                }
            }
            else if ((m = regexEstimate.Match(trimmed)).Success)
            {
                Subject = ToAgent(m.Groups[1].ToString());
                Topic = (Topic)Enum.Parse(typeof(Topic), m.Groups[2].ToString());
                Target = ToAgent(m.Groups[3].ToString());
                Role = (Role)Enum.Parse(typeof(Role), m.Groups[4].ToString());
            }
            else if ((m = regexDivined.Match(trimmed)).Success)
            {
                Subject = ToAgent(m.Groups[1].ToString());
                Topic = (Topic)Enum.Parse(typeof(Topic), m.Groups[2].ToString());
                Target = ToAgent(m.Groups[3].ToString());
                Result = (Species)Enum.Parse(typeof(Species), m.Groups[4].ToString());
            }
            else if ((m = regexAttack.Match(trimmed)).Success)
            {
                Subject = ToAgent(m.Groups[1].ToString());
                Topic = (Topic)Enum.Parse(typeof(Topic), m.Groups[2].ToString());
                Target = ToAgent(m.Groups[3].ToString());
            }
            else if ((m = regexRequest.Match(trimmed)).Success)
            {
                Topic = Topic.OPERATOR;
                Subject = ToAgent(m.Groups[1].ToString());
                Operator = (Operator)Enum.Parse(typeof(Operator), m.Groups[2].ToString());
                Target = ToAgent(m.Groups[3].ToString());
                ContentList = GetContents(m.Groups[4].ToString());
            }
            else if ((m = regexBecause.Match(trimmed)).Success)
            {
                Topic = Topic.OPERATOR;
                Subject = ToAgent(m.Groups[1].ToString());
                Operator = (Operator)Enum.Parse(typeof(Operator), m.Groups[2].ToString());
                ContentList = GetContents(m.Groups[3].ToString());
                if (Operator == Operator.REQUEST)
                {
                    Target = ContentList[0].Subject == Agent.NONE ? Agent.ANY : ContentList[0].Subject;
                }
            }
            else if ((m = regexDay.Match(trimmed)).Success)
            {
                Topic = Topic.OPERATOR;
                Subject = ToAgent(m.Groups[1].ToString());
                Operator = Operator.DAY;
                Day = int.Parse(m.Groups[2].ToString());
                ContentList = GetContents(m.Groups[3].ToString());
            }
            else
            {
                Topic = Topic.Skip;
            }
            CompleteInnerSubject();
            NormalizeText();
        }

#if JHELP
        /// <summary>
        /// 発話文字列が等しい場合trueを返す
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>発話文字列が等しい場合true</returns>
#else
        /// <summary>
        /// Returns true if text representation of this content equals that of other.
        /// </summary>
        /// <param name="other">Content to compare with this content.</param>
        /// <returns>true if other equals this in text; otherwise, false.</returns>
#endif
        public bool Equals(Content other) => other == null ? false : other.Text == Text;

#if JHELP
        /// <summary>
        /// 発話文字列が等しい場合trueを返す
        /// </summary>
        /// <param name="obj">比較対象</param>
        /// <returns>発話文字列が等しい場合true</returns>
#else
        /// <summary>
        /// Returns true if text representation of this content equals that of other.
        /// </summary>
        /// <param name="obj">Object to compare with this content.</param>
        /// <returns>true if other equals this in text; otherwise, false.</returns>
#endif
        public override bool Equals(object obj) => obj is Content content ? Equals(content) : false;

#if JHELP
        /// <summary>
        /// ハッシュ関数
        /// </summary>
        /// <returns>ハッシュ値</returns>
#else
        /// <summary>
        /// Hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
#endif
        public override int GetHashCode() => Text.GetHashCode();
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
        /// 投票先の報告
        /// </summary>
#else
        /// <summary>
        /// Report of a vote.
        /// </summary>
#endif
        VOTED,

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
        /// 襲撃先の報告
        /// </summary>
#else
        /// <summary>
        /// Report of an Attack.
        /// </summary>
#endif
        ATTACKED,

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
        /// 照会
        /// </summary>
#else
        /// <summary>
        /// Inquiry.
        /// </summary>
#endif
        INQUIRE,

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
        /// 日付
        /// </summary>
#else
        /// <summary>
        /// DATE.
        /// </summary>
#endif
        DAY,

#if JHELP
        /// <summary>
        /// NOT
        /// </summary>
#else
        /// <summary>
        /// NOT.
        /// </summary>
#endif
        NOT,

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
        OR,

#if JHELP
        /// <summary>
        /// XOR
        /// </summary>
#else
        /// <summary>
        /// XOR.
        /// </summary>
#endif
        XOR
    }
}
