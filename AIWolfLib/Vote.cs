﻿//
// Vote.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using Newtonsoft.Json;

namespace AIWolf.Lib
{
#if JHELP
    /// <summary>
    /// 追放投票情報
    /// </summary>
#else
    /// <summary>
    /// Information of vote for execution.
    /// </summary>
#endif
    public class Vote
    {
#if JHELP
        /// <summary>
        /// この投票の日
        /// </summary>
#else
        /// <summary>
        /// The day of this vote.
        /// </summary>
#endif
        public int Day { get; }

#if JHELP
        /// <summary>
        /// 投票したエージェント
        /// </summary>
#else
        /// <summary>
        /// The agent who voted.
        /// </summary>
#endif
        public Agent Agent { get; }

#if JHELP
        /// <summary>
        /// 投票されたエージェント
        /// </summary>
#else
        /// <summary>
        /// The voted agent.
        /// </summary>
#endif
        public Agent Target { get; }

#if JHELP
        /// <summary>
        /// 追放投票情報の新しいインスタンスを初期化する
        /// </summary>
        /// <param name="day">投票日</param>
        /// <param name="agent">投票したエージェント</param>
        /// <param name="target">投票されたエージェント</param>
#else
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="day">The day of this vote.</param>
        /// <param name="agent">The agent who voted.</param>
        /// <param name="target">The voted agent.</param>
#endif
        public Vote(int day = 0, Agent agent = null, Agent target = null)
        {
            Day = day;
            Agent = agent;
            Target = target;
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="day">The day of this vote.</param>
        /// <param name="agent">The index of agent who voted.</param>
        /// <param name="target">The index of voted agent.</param>
        [JsonConstructor]
        Vote(int day, int agent, int target)
            : this(day, Agent.GetAgent(agent), Agent.GetAgent(target)) { }

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
        public override string ToString() => $"{Agent}voted{Target}@{Day}";
    }
}
