using AIWolf.Common.Net;

namespace AIWolf.Common.Data
{
    /// <summary>
    /// Defines a generalized methods that a class implements to be a player of AIWolf.
    /// </summary>
    /// <remarks></remarks>
    public interface IPlayer
    {
        /// <summary>
        /// This player's name.
        /// </summary>
        /// <value>This player's name.</value>
        /// <remarks></remarks>
        string Name { get; }

        /// <summary>
        /// Called when the game information is updated.
        /// </summary>
        /// <param name="gameInfo">The current information of this game.</param>
        /// <remarks></remarks>
        void Update(GameInfo gameInfo);

        /// <summary>
        /// Called when the game started.
        /// </summary>
        /// <param name="gameInfo">The current information of this game.</param>
        /// <param name="gameSetting">The setting of this game.</param>
        /// <remarks></remarks>
        void Initialize(GameInfo gameInfo, GameSetting gameSetting);

        /// <summary>
        /// Called when the day started.
        /// </summary>
        /// <remarks></remarks>
        void DayStart();

        /// <summary>
        /// Returns this player's talk.
        /// </summary>
        /// <returns>The string representing this player's talk.</returns>
        /// <remarks>
        /// The returned string must be written in aiwolf protocol.
        /// Null means SKIP.
        /// </remarks>
        string Talk();

        /// <summary>
        /// Returns this werewolf's whisper.
        /// </summary>
        /// <returns>The string representing this werewolf's whisper.</returns>
        /// <remarks>
        /// The returned string must be written in aiwolf protocol.
        /// Null means SKIP.
        /// </remarks>
        string Whisper();

        /// <summary>
        /// Returns the agent this player wants to execute.
        /// </summary>
        /// <returns>The agent this player wants to execute.</returns>
        /// <remarks></remarks>
        Agent Vote();

        /// <summary>
        /// Returns the agent this werewolf wants to attack.
        /// </summary>
        /// <returns>The agent this werewolf wants to attack.</returns>
        /// <remarks></remarks>
        Agent Attack();

        /// <summary>
        /// Returns the agent this seer wants to divine.
        /// </summary>
        /// <returns>The agent this seer wants to divine.</returns>
        /// <remarks></remarks>
        Agent Divine();

        /// <summary>
        /// Returns the agent this bodyguard wants to guard.
        /// </summary>
        /// <returns>The agent this bodyguard wants to guard.</returns>
        /// <remarks></remarks>
        Agent Guard();

        /// <summary>
        /// Called when the game finishes.
        /// </summary>
        /// <remarks>Before this method is called, the game information is updated with all information.</remarks>
        void Finish();
    }
}
