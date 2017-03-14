using AIWolf.Common.Data;
using AIWolf.Common.Net;
using System.Collections.Generic;

namespace AIWolf.Client.Base.Player
{
    /// <summary>
    /// Abstract class which each role's abstract class inherits.
    /// </summary>
    /// <remarks></remarks>
    public abstract class AbstractRole
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <remarks></remarks>
        protected AbstractRole() { }

        /// <summary>
        /// Game information map.
        /// </summary>
        /// <value>The dictionary storing the game information of each day.</value>
        /// <remarks></remarks>
        protected Dictionary<int, GameInfo> GameInfoMap { get; set; } = new Dictionary<int, GameInfo>();

        /// <summary>
        /// The current day.
        /// </summary>
        /// <value>The current day.</value>
        /// <remarks></remarks>
        protected int Day { get; set; }

        /// <summary>
        /// This player itself.
        /// </summary>
        /// <value>The agent representing this player.</value>
        /// <remarks></remarks>
        protected Agent Me { get; set; }

        /// <summary>
        /// This player's role.
        /// </summary>
        /// <value>The role which this player acts as.</value>
        /// <remarks></remarks>
        protected Role? MyRole { get; set; }

        /// <summary>
        /// Game setting.
        /// </summary>
        /// <value>The setting of this game.</value>
        /// <remarks></remarks>
        protected GameSetting GameSetting { get; set; }

        /// <summary>
        /// This player's name.
        /// </summary>
        /// <value>This player's name.</value>
        /// <remarks></remarks>
        protected string Name
        {
            get
            {
                return MyRole.ToString() + "Player:ID=" + Me.AgentIdx;
            }
        }

        /// <summary>
        /// The latest game information.
        /// </summary>
        /// <value>This game's information on the current day.</value>
        /// <remarks></remarks>
        protected GameInfo LatestDayGameInfo
        {
            get
            {
                return GameInfoMap[Day];
            }
        }

        /// <summary>
        /// Called when the game information is updated.
        /// </summary>
        /// <param name="gameInfo">The current information of this game.</param>
        /// <remarks></remarks>
        public virtual void Update(GameInfo gameInfo)
        {
            Day = gameInfo.Day;
            GameInfoMap[Day] = gameInfo;
        }

        /// <summary>
        /// Returns the game information on the specified day.
        /// </summary>
        /// <param name="day">The day specified.</param>
        /// <returns>The game informatrion on the specified day.</returns>
        /// <remarks></remarks>
        protected GameInfo GetGameInfo(int day)
        {
            return GameInfoMap.ContainsKey(day) ? GameInfoMap[day] : null;
        }

        /// <summary>
        /// Called when the game started.
        /// </summary>
        /// <param name="gameInfo">The current information of this game.</param>
        /// <param name="gameSetting">The setting of this game.</param>
        /// <remarks></remarks>
        public virtual void Initialize(GameInfo gameInfo, GameSetting gameSetting)
        {
            GameInfoMap.Clear();
            GameSetting = gameSetting;
            Day = gameInfo.Day;
            GameInfoMap[Day] = gameInfo;
            MyRole = gameInfo.Role;
            Me = gameInfo.Agent;
            return;
        }

        /// <summary>
        /// Called when the day started.
        /// </summary>
        /// <remarks></remarks>
        public abstract void DayStart();

        /// <summary>
        /// Returns this player's talk.
        /// </summary>
        /// <returns>The string representing this player's talk.</returns>
        /// <remarks>
        /// The returned string must be written in aiwolf protocol.
        /// Null means SKIP.
        /// </remarks>
        public abstract string Talk();

        /// <summary>
        /// Returns this werewolf's whisper.
        /// </summary>
        /// <returns>The string representing this werewolf's whisper.</returns>
        /// <remarks>
        /// The returned string must be written in aiwolf protocol.
        /// Null means SKIP.
        /// </remarks>
        public abstract string Whisper();

        /// <summary>
        /// Returns the agent this player wants to execute.
        /// </summary>
        /// <returns>The agent this player wants to execute.</returns>
        /// <remarks></remarks>
        public abstract Agent Vote();

        /// <summary>
        /// Returns the agent this werewolf wants to attack.
        /// </summary>
        /// <returns>The agent this werewolf wants to attack.</returns>
        /// <remarks></remarks>
        public abstract Agent Attack();

        /// <summary>
        /// Returns the agent this seer wants to divine.
        /// </summary>
        /// <returns>The agent this seer wants to divine.</returns>
        /// <remarks></remarks>
        public abstract Agent Divine();

        /// <summary>
        /// Returns the agent this bodyguard wants to guard.
        /// </summary>
        /// <returns>The agent this bodyguard wants to guard.</returns>
        /// <remarks></remarks>
        public abstract Agent Guard();

        /// <summary>
        /// Called when the game finishes.
        /// </summary>
        /// <remarks>Before this method is called, the game information is updated with all information.</remarks>
        public abstract void Finish();
    }
}