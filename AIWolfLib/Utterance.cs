//
// Utterance.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace AIWolf.Lib
{
#if JHELP
    /// <summary>
    /// 発話抽象クラス
    /// </summary>
#else
    /// <summary>
    /// Abstract utterance class.
    /// </summary>
#endif
    [DataContract]
    public abstract class Utterance
    {
#if JHELP
        /// <summary>
        /// 発話することがない
        /// </summary>
#else
        /// <summary>
        /// There is nothing to utter.
        /// </summary>
#endif
        public const string OVER = "Over";

#if JHELP
        /// <summary>
        /// 発話することはあるがこのターンはスキップ
        /// </summary>
#else
        /// <summary>
        /// Skip this turn though there is something to utter.
        /// </summary>
#endif
        public const string SKIP = "Skip";

#if JHELP
        /// <summary>
        /// この発話のインデックス番号
        /// </summary>
#else
        /// <summary>
        /// The index number of this utterance.
        /// </summary>
#endif
        [DataMember(Name = "idx")]
        public int Idx { get; }

#if JHELP
        /// <summary>
        /// この発話の日
        /// </summary>
#else
        /// <summary>
        /// The day of this utterance.
        /// </summary>
#endif
        [DataMember(Name = "day")]
        public int Day { get; }

#if JHELP
        /// <summary>
        /// この発話のターン（オプション，未指定の場合-1）
        /// </summary>
#else
        /// <summary>
        /// The turn of this utterance(optional). -1 if not specified.
        /// </summary>
#endif
        [DataMember(Name = "turn")]
        public int Turn { get; } = -1;

#if JHELP
        /// <summary>
        /// 発話したエージェント
        /// </summary>
#else
        /// <summary>
        /// The agent who uttered.
        /// </summary>
#endif
        public Agent Agent { get; }

        /// <summary>
        /// The index number of the agent who uttered.
        /// </summary>
        [DataMember(Name = "agent")]
        int _Agent { get; }

#if JHELP
        /// <summary>
        /// この発話の内容文字列
        /// </summary>
#else
        /// <summary>
        /// The contents of this utterance.
        /// </summary>
#endif
        [DataMember(Name = "text")]
        public string Text { get; }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="idx">The index of this utterance.</param>
        /// <param name="day">The day of this utterance.</param>
        protected Utterance(int idx, int day)
        {
            Idx = idx;
            if (Idx < 0)
            {
                Error.RuntimeError("Invalid idx " + Idx + ".");
                Idx = 0;
                Error.Warning("Force it to be " + Idx + ".");
            }

            Day = day;
            if (Day < 0)
            {
                Error.RuntimeError("Invalid day " + Day + ".");
                Day = 0;
                Error.Warning("Force it to be " + Day + ".");
            }
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="idx">The index of this utterance.</param>
        /// <param name="day">The day of this utterance.</param>
        /// <param name="turn">The turn of this utterance.</param>
        protected Utterance(int idx, int day, int turn) : this(idx, day)
        {
            Turn = turn;
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="idx">The index of this utterance.</param>
        /// <param name="day">The day of this utterance.</param>
        /// <param name="turn">The turn of this utterance.</param>
        /// <param name="agent">The agent who uttered.</param>
        /// <param name="text">The text of this utterance.</param>
        protected Utterance(int idx, int day, int turn, Agent agent, string text) : this(idx, day, turn)
        {
            Agent = agent;
            if (Agent == null)
            {
                Error.RuntimeError("Agent must not be null.");
                Agent = Agent.GetAgent(0);
                Error.Warning("Force it to be " + Agent + ".");
            }
            _Agent = Agent.AgentIdx;

            Text = text;
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="idx">The index of this utterance.</param>
        /// <param name="day">The day of this utterance.</param>
        /// <param name="turn">The turn of this utterance.</param>
        /// <param name="agent">The index of agent who uttered.</param>
        /// <param name="text">The text of this utterance.</param>
        [JsonConstructor]
        protected Utterance(int idx, int day, int turn, int agent, string text) : this(idx, day, turn, Agent.GetAgent(agent), text)
        {
        }

#if JHELP
        /// <summary>
        /// このオブジェクトを表す文字列を返す
        /// </summary>
        /// <returns>このオブジェクトを表す文字列</returns>
#else
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
#endif
        public abstract override string ToString();
    }

#if JHELP
    /// <summary>
    /// 会話クラス
    /// </summary>
#else
    /// <summary>
    /// Talk class.
    /// </summary>
#endif
    public class Talk : Utterance
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="idx">The index of this talk.</param>
        /// <param name="day">The day of this talk.</param>
        internal Talk(int idx, int day) : base(idx, day)
        {
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="idx">The index of this talk.</param>
        /// <param name="day">The day of this talk.</param>
        /// <param name="turn">The turn of this talk.</param>
        /// <param name="agent">The agent who talked.</param>
        /// <param name="text">The text of this talk.</param>
        Talk(int idx, int day, int turn, Agent agent, string text) : base(idx, day, turn, agent, text)
        {
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="idx">The index of this talk.</param>
        /// <param name="day">The day of this talk.</param>
        /// <param name="turn">The turn of this talk.</param>
        /// <param name="agent">The index of agent who talked.</param>
        /// <param name="text">The text of this talk.</param>
        [JsonConstructor]
        Talk(int idx, int day, int turn, int agent, string text) : base(idx, day, turn, agent, text)
        {
        }

#if JHELP
        /// <summary>
        /// このオブジェクトを表す文字列を返す
        /// </summary>
        /// <returns>このオブジェクトを表す文字列</returns>
#else
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
#endif
        public override string ToString()
        {
            return string.Format("Talk: Day{0:D2} {1:D2}[{2:D3}]\t{3}\t{4}", Day, Turn, Idx, Agent, Text);
        }
    }

#if JHELP
    /// <summary>
    /// 囁きクラス
    /// </summary>
#else
    /// <summary>
    /// Whisper class.
    /// </summary>
#endif
    public class Whisper : Utterance
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="idx">The index of this whisper.</param>
        /// <param name="day">The day of this whisper.</param>
        internal Whisper(int idx, int day) : base(idx, day)
        {
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="idx">The index of this whisper.</param>
        /// <param name="day">The day of this whisper.</param>
        /// <param name="turn">The turn of this whisper.</param>
        /// <param name="agent">The agent who whispered.</param>
        /// <param name="text">The text of this whisper.</param>
        Whisper(int idx, int day, int turn, Agent agent, string text) : base(idx, day, turn, agent, text)
        {
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="idx">The index of this whisper.</param>
        /// <param name="day">The day of this whisper.</param>
        /// <param name="turn">The turn of this whisper.</param>
        /// <param name="agent">The index of agent who whispered.</param>
        /// <param name="text">The text of this whisper.</param>
        [JsonConstructor]
        Whisper(int idx, int day, int turn, int agent, string text) : base(idx, day, turn, agent, text)
        {
        }

#if JHELP
        /// <summary>
        /// このオブジェクトを表す文字列を返す
        /// </summary>
        /// <returns>このオブジェクトを表す文字列</returns>
#else
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
#endif
        public override string ToString()
        {
            return string.Format("Whisper: Day{0:D2} {1:D2}[{2:D3}]\t{3}\t{4}", Day, Turn, Idx, Agent, Text);
        }
    }

}
