//
// Judge.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace AIWolf.Lib
{
#if JHELP
    /// <summary>
    /// プレイヤーが人間か人狼かの判定
    /// </summary>
#else
    /// <summary>
    /// The judge whether the player is human or werewolf.
    /// </summary>
#endif
    public class Judge
    {
#if JHELP
        /// <summary>
        /// 判定した日
        /// </summary>
#else
        /// <summary>
        /// The day of this judge.
        /// </summary>
#endif
        public int Day { get; }

#if JHELP
        /// <summary>
        /// 判定を下したエージェント
        /// </summary>
#else
        /// <summary>
        /// The agent who judged.
        /// </summary>
#endif
        public Agent Agent { get; }

#if JHELP
        /// <summary>
        /// 判定されたエージェント
        /// </summary>
#else
        /// <summary>
        /// The judged agent.
        /// </summary>
#endif
        public Agent Target { get; }

#if JHELP
        /// <summary>
        /// 判定結果
        /// </summary>
#else
        /// <summary>
        /// The result of this judge.
        /// </summary>
#endif
        public Species Result { get; }

#if JHELP
        /// <summary>
        /// Judgeクラスの新しいインスタンスを初期化する
        /// </summary>
        /// <param name="day">判定の日</param>
        /// <param name="agent">判定を下したエージェント</param>
        /// <param name="target">判定されたエージェント</param>
        /// <param name="result">判定結果</param>
#else
        /// <summary>
        /// Initializes a new instance of Judge class.
        /// </summary>
        /// <param name="day">The day of this judge.</param>
        /// <param name="agent">The agent who judged.</param>
        /// <param name="target">The judged agent.</param>
        /// <param name="result">The result of this judge.</param>
#endif
        public Judge(int day = 0, Agent agent = null, Agent target = null, Species result = Species.UNC)
        {
            Day = day;
            Agent = agent;
            Target = target;
            Result = result;
        }

        /// <summary>
        /// Initializes a new instance of Judge class.
        /// </summary>
        /// <param name="day">The day of this judge.</param>
        /// <param name="agent">The index of agent who judged.</param>
        /// <param name="target">The index of judged agent.</param>
        /// <param name="result">The result of this judge.</param>
        [JsonConstructor]
        Judge(int day, int agent, int target, string result)
            : this(day, Agent.GetAgent(agent), Agent.GetAgent(target), (Species)Enum.Parse(typeof(Species), result)) { }

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
        public override string ToString() => $"{Agent}->{Target}@{Day}:{Result}";
    }
}
