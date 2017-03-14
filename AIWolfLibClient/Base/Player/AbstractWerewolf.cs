using AIWolf.Common.Data;
using System.Collections.Generic;

namespace AIWolf.Client.Base.Player
{
    /// <summary>
    /// Abstract class for werewolf players.
    /// </summary>
    /// <remarks></remarks>
    public abstract class AbstractWerewolf : AbstractRole
    {
        /// <summary>
        /// The list of werewolves.
        /// </summary>
        /// <value>The list of werewolf agents.</value>
        /// <remarks></remarks>
        protected List<Agent> WolfList
        {
            get
            {
                List<Agent> wolfList = new List<Agent>();
                foreach (var pair in LatestDayGameInfo.RoleMap)
                {
                    if (pair.Value == Role.WEREWOLF)
                    {
                        wolfList.Add(pair.Key);
                    }
                }
                return wolfList;
            }
        }

        /// <summary>
        /// Called when the day started.
        /// </summary>
        /// <remarks></remarks>
        public abstract override void DayStart();

        /// <summary>
        /// Returns this player's talk.
        /// </summary>
        /// <returns>The string representing this player's talk.</returns>
        /// <remarks>
        /// The returned string must be written in aiwolf protocol.
        /// Null means SKIP.
        /// </remarks>
        public abstract override string Talk();

        /// <summary>
        /// Returns this werewolf's whisper.
        /// </summary>
        /// <returns>The string representing this werewolf's whisper.</returns>
        /// <remarks>
        /// The returned string must be written in aiwolf protocol.
        /// Null means SKIP.
        /// </remarks>
        public abstract override string Whisper();

        /// <summary>
        /// Returns the agent this player wants to execute.
        /// </summary>
        /// <returns>The agent this player wants to execute.</returns>
        /// <remarks></remarks>
        public abstract override Agent Vote();

        /// <summary>
        /// Returns the agent this werewolf wants to attack.
        /// </summary>
        /// <returns>The agent this werewolf wants to attack.</returns>
        /// <remarks></remarks>
        public abstract override Agent Attack();

        /// <summary>
        /// Returns the agent this seer wants to divine.
        /// </summary>
        /// <returns>The agent this seer wants to divine.</returns>
        /// <remarks></remarks>
        sealed public override Agent Divine()
        {
            throw new UnexpectedMethodCallException();
        }

        /// <summary>
        /// Returns the agent this bodyguard wants to guard.
        /// </summary>
        /// <returns>The agent this bodyguard wants to guard.</returns>
        /// <remarks></remarks>
        sealed public override Agent Guard()
        {
            throw new UnexpectedMethodCallException();
        }

        /// <summary>
        /// Called when the game finishes.
        /// </summary>
        /// <remarks>Before this method is called, the game information is updated with all information.</remarks>
        public abstract override void Finish();

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <remarks></remarks>
        protected AbstractWerewolf()
        {
            MyRole = Role.WEREWOLF;
        }
    }
}
