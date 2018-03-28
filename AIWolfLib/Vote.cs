//
// Vote.cs
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
    /// 追放投票情報
    /// </summary>
#else
    /// <summary>
    /// Information of vote for execution.
    /// </summary>
#endif
    [DataContract]
    public class Vote
    {
        /// <summary>
        /// The index number of the agent who voted.
        /// </summary>
        [DataMember(Name = "agent")]
        int agent;

        /// <summary>
        /// The index number of the voted agent.
        /// </summary>
        [DataMember(Name = "target")]
        int target;

#if JHELP
        /// <summary>
        /// この投票の日
        /// </summary>
#else
        /// <summary>
        /// The day of this vote.
        /// </summary>
#endif
        [DataMember(Name = "day")]
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
        /// <remarks>agent/targetがnullの場合null参照例外</remarks>
#else
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="day">The day of this vote.</param>
        /// <param name="agent">The agent who voted.</param>
        /// <param name="target">The voted agent.</param>
        /// <remarks>NullReferenceException is thrown in case of null agent/target.</remarks>
#endif
        public Vote(int day, Agent agent, Agent target)
        {
            Day = day;
            Agent = agent;
            Target = target;
            // NullReferenceException is thrown in case of null agent/target.
            this.agent = agent.AgentIdx;
            this.target = target.AgentIdx;
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="day">The day of this vote.</param>
        /// <param name="agent">The index of agent who voted.</param>
        /// <param name="target">The index of voted agent.</param>
        [JsonConstructor]
        Vote(int day, int agent, int target)
        {
            Day = day;
            this.agent = agent;
            this.target = target;
            Agent = Agent.GetAgent(agent);
            Target = Agent.GetAgent(target);
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
        public override string ToString() => $"{Agent}voted{Target}@{Day}";
    }
}
