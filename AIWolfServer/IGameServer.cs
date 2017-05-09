//
// IGameServer.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using AIWolf.Lib;
using System.Collections.Generic;

namespace AIWolf.Server
{
    public interface IGameServer
    {
        /// <summary>
        /// The list of agents connecting to this server.
        /// </summary>
        List<Agent> ConnectedAgentList { get; }

        /// <summary>
        /// The setting of the game.
        /// </summary>
        GameSetting GameSetting { set; }

        /// <summary>
        /// The game data.
        /// </summary>
        GameData GameData { set; }

        /// <summary>
        /// Initializes the agents.
        /// </summary>
        /// <param name="agent">The agent to be initialized.</param>
        void Init(Agent agent);

        /// <summary>
        /// Requests agent's name.
        /// </summary>
        /// <param name="agent">The requested agent.</param>
        /// <returns>The name of the agent.</returns>
        string RequestName(Agent agent);

        /// <summary>
        /// Requests the role requested by the agent.
        /// </summary>
        /// <param name="agent">The requested agent.</param>
        /// <returns>The role the agent requests.</returns>
        Role RequestRequestRole(Agent agent);

        /// <summary>
        /// Requests the agent's talk.
        /// </summary>
        /// <param name="agent">The requested agent.</param>
        /// <returns>The talk uttered by the agent.</returns>
        string RequestTalk(Agent agent);

        /// <summary>
        /// Requests the agent's whisper.
        /// </summary>
        /// <param name="agent">The requested agent.</param>
        /// <returns>The whisper uttered by the agent.</returns>
        string RequestWhisper(Agent agent);

        /// <summary>
        /// Requests the agent's vote.
        /// </summary>
        /// <param name="agent">The requested agent.</param>
        /// <returns>The agent's vote.</returns>
        Agent RequestVote(Agent agent);

        /// <summary>
        /// Requests the target of the agent's divination.
        /// </summary>
        /// <param name="agent">The requested agent.</param>
        /// <returns>The target of the agent's divination.</returns>
        Agent RequestDivineTarget(Agent agent);

        /// <summary>
        /// Requests the target of the agent's guard.
        /// </summary>
        /// <param name="agent">The requested agent.</param>
        /// <returns>The target of the agent's guard.</returns>
        Agent RequestGuardTarget(Agent agent);

        /// <summary>
        /// Requests the target of the agent's attack.
        /// </summary>
        /// <param name="agent">The requested agent.</param>
        /// <returns>The target of the agent's attack.</returns>
        Agent RequestAttackTarget(Agent agent);

        /// <summary>
        /// Informs tha agent that the day starts.
        /// </summary>
        /// <param name="agent">The informed agent.</param>
        void DayStart(Agent agent);

        /// <summary>
        /// Informs the agent that the day has finished.
        /// </summary>
        /// <param name="agent">The informed agent.</param>
        void DayFinish(Agent agent);

        /// <summary>
        /// Informs the agent that the game has finished.
        /// </summary>
        /// <param name="agent">The informed agent.</param>
        void Finish(Agent agent);

        /// <summary>
        /// Closes all connections.
        /// </summary>
        void Close();
    }
}
