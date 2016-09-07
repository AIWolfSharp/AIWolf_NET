﻿//
// Agent.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AIWolf.Lib
{
    /// <summary>
    /// Agent class.
    /// </summary>
    [DataContract]
    public sealed class Agent
    {
        static Dictionary<int, Agent> agentMap = new Dictionary<int, Agent>();

        /// <summary>
        /// Returns the agent of given index.
        /// </summary>
        /// <param name="idx">Agent's index number.</param>
        /// <returns>The agent of given index number.</returns>
        /// <remarks>If idx is negative, this returns null.</remarks>
        public static Agent GetAgent(int idx)
        {
            if (idx < 0)
            {
                return null;
            }
            if (idx == 0)
            {
                Error.Warning("Agent.GetAgent(): Agent index is 0.");
            }
            if (!agentMap.ContainsKey(idx))
            {
                agentMap[idx] = new Agent(idx);
            }
            return agentMap[idx];
        }

        /// <summary>
        /// The index number of this agent.
        /// </summary>
        [DataMember(Name = "agentIdx")]
        public int AgentIdx { get; }

        /// <summary>
        /// Initializes a new instance of Agent class with given index number.
        /// </summary>
        /// <param name="idx">The index number of this agent.</param>
        Agent(int idx)
        {
            AgentIdx = idx;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("Agent[{0:00}]", AgentIdx);
        }
    }
}
