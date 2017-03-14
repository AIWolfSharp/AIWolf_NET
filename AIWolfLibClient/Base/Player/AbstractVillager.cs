using AIWolf.Common.Data;

namespace AIWolf.Client.Base.Player
{
    /// <summary>
    /// Abstract class for villager players.
    /// </summary>
    /// <remarks></remarks>
    public abstract class AbstractVillager : AbstractRole
    {
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
        sealed public override string Whisper()
        {
            throw new UnexpectedMethodCallException();
        }

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
        sealed public override Agent Attack()
        {
            throw new UnexpectedMethodCallException();
        }

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
        protected AbstractVillager()
        {
            MyRole = Role.VILLAGER;
        }
    }
}
