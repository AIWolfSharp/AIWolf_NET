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

        /// <summary>
        /// The index number of the agent who voted.
        /// </summary>
        [DataMember(Name = "agent")]
        int _Agent { get; }

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

        /// <summary>
        /// The index number of the voted agent.
        /// </summary>
        [DataMember(Name = "target")]
        int _Target { get; }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="day">The day of this vote.</param>
        /// <param name="agent">The agent who voted.</param>
        /// <param name="target">The voted agent.</param>
        Vote(int day, Agent agent, Agent target)
        {
            Day = day;
            if (Day < 0)
            {
                Error.RuntimeError("Invalid day " + Day + ".");
                Error.Warning("Force it to be 0.");
                Day = 0;
            }

            Agent = agent;
            if (Agent == null)
            {
                Error.RuntimeError("Agent must not be null.");
                Error.Warning("Force it to be Agent[00].");
                Agent = Agent.GetAgent(0);
            }
            _Agent = Agent.AgentIdx;

            Target = target;
            if (Target == null)
            {
                Error.RuntimeError("Target must not be null.");
                Error.Warning("Force it to be Agent[00].");
                Target = Agent.GetAgent(0);
            }
            _Target = Target.AgentIdx;
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="day">The day of this vote.</param>
        /// <param name="agent">The index of agent who voted.</param>
        /// <param name="target">The index of voted agent.</param>
        [JsonConstructor]
        Vote(int day, int agent, int target) : this(day, Agent.GetAgent(agent), Agent.GetAgent(target))
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
            return Agent + "voted" + Target + "@" + Day;
        }
    }
}
