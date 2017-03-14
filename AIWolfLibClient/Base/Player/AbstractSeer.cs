using AIWolf.Common.Data;
using System.Collections.Generic;

namespace AIWolf.Client.Base.Player
{
    /// <summary>
    /// Abstract class for seer players.
    /// </summary>
    /// <remarks></remarks>
    public abstract class AbstractSeer : AbstractRole
    {
        /// <summary>
        /// The list of divinations ever done.
        /// </summary>
        /// <value>The list of divinations this seer has ever done.</value>
        /// <remarks></remarks>
        protected List<Judge> MyJudgeList { get; set; } = new List<Judge>();

        /// <summary>
        /// Called when the day started.
        /// </summary>
        /// <remarks></remarks>
        public override void DayStart()
        {
            if (GameInfoMap[Day].DivineResult != null)
            {
                MyJudgeList.Add(LatestDayGameInfo.DivineResult);
            }
        }

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
        public abstract override Agent Divine();

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
        protected AbstractSeer()
        {
            MyRole = Role.SEER;
        }

        /// <summary>
        /// Returns whether or not the agent has been already divined.
        /// </summary>
        /// <param name="agent">The agent to be checked.</param>
        /// <returns>True if the agent has been already divined, otherwise, false.</returns>
        /// <remarks></remarks>
        protected bool IsJudgedAgent(Agent agent)
        {
            foreach (Judge judge in MyJudgeList)
            {
                if (judge.Target == agent)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
