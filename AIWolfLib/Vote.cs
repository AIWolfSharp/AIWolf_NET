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
        int _agent;

        /// <summary>
        /// The index number of the voted agent.
        /// </summary>
        [DataMember(Name = "target")]
        int _target;

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
        public Agent Agent => Agent.GetAgent(_agent);

#if JHELP
        /// <summary>
        /// 投票されたエージェント
        /// </summary>
#else
        /// <summary>
        /// The voted agent.
        /// </summary>
#endif
        public Agent Target => Agent.GetAgent(_target);

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="day">The day of this vote.</param>
        /// <param name="agent">The agent who voted.</param>
        /// <param name="target">The voted agent.</param>
        public Vote(int day, Agent agent, Agent target)
        {
            Day = day;
            if (Day < 0)
            {
                Error.RuntimeError("Invalid day " + Day + ".");
                Error.Warning("Force it to be 0.");
                Day = 0;
            }

            if (agent == null)
            {
                Error.RuntimeError("Agent must not be null.");
                Error.Warning("Force it to be Agent[00].");
                _agent = 0;
            }
            else
            {
                _agent = agent.AgentIdx;
            }

            if (target == null)
            {
                Error.RuntimeError("Target must not be null.");
                Error.Warning("Force it to be Agent[00].");
                _target = 0;
            }
            else
            {
                _target = target.AgentIdx;
            }
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
