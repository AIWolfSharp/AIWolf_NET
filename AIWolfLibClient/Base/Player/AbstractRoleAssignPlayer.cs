using AIWolf.Client.Base.Smpl;
using AIWolf.Common.Data;
using AIWolf.Common.Net;

namespace AIWolf.Client.Base.Player
{
    /// <summary>
    /// Abstract player class which assigns special player according to its role.
    /// </summary>
    /// <remarks></remarks>
    public abstract class AbstractRoleAssignPlayer : IPlayer
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <remarks></remarks>
        protected AbstractRoleAssignPlayer() { }

        /// <summary>
        /// The instance of AbstractRole class which acts as a villager.
        /// </summary>
        /// <value>The instance of AbstractRole which acts as a villager.</value>
        /// <remarks></remarks>
        protected AbstractRole VillagerPlayer { get; set; } = new SampleVillager();

        /// <summary>
        /// The instance of AbstractRole class which acts as a seer.
        /// </summary>
        /// <value>The instance of AbstractRole which acts as a seer.</value>
        /// <remarks></remarks>
        protected AbstractRole SeerPlayer { get; set; } = new SampleSeer();

        /// <summary>
        /// The instance of AbstractRole which acts as a medium.
        /// </summary>
        /// <value>The instance of AbstractRole which acts as a medium.</value>
        /// <remarks></remarks>
        protected AbstractRole MediumPlayer { get; set; } = new SampleMedium();

        /// <summary>
        /// The instance of AbstractRole which acts as a bodyguard.
        /// </summary>
        /// <value>The instance of AbstractRole which acts as a bodyguard.</value>
        /// <remarks></remarks>
        protected AbstractRole BodyguardPlayer { get; set; } = new SampleBodyguard();

        /// <summary>
        /// The instance of AbstractRole which acts as a possessed person.
        /// </summary>
        /// <value>The instance of AbstractRole which acts as a possessed person.</value>
        /// <remarks></remarks>
        protected AbstractRole PossessedPlayer { get; set; } = new SamplePossessed();

        /// <summary>
        /// The instance of AbstractRole which acts as a werewolf.
        /// </summary>
        /// <value>The instance of AbstractRole which acts as a werewolf.</value>
        /// <remarks></remarks>
        protected AbstractRole WerewolfPlayer { get; set; } = new SampleWerewolf();

        AbstractRole rolePlayer;

        /// <summary>
        /// This player's name.
        /// </summary>
        /// <value>This player's name.</value>
        /// <remarks></remarks>
        public abstract string Name { get; }

        /// <summary>
        /// Called when the game information is updated.
        /// </summary>
        /// <param name="gameInfo">The current information of this game.</param>
        /// <remarks></remarks>
        void IPlayer.Update(GameInfo gameInfo)
        {
            rolePlayer.Update(gameInfo);
        }

        /// <summary>
        /// Called when the game started.
        /// </summary>
        /// <param name="gameInfo">The current information of this game.</param>
        /// <param name="gameSetting">The setting of this game.</param>
        /// <remarks></remarks>
        void IPlayer.Initialize(GameInfo gameInfo, GameSetting gameSetting)
        {
            Role? myRole = gameInfo.Role;
            switch (myRole)
            {
                case Role.VILLAGER:
                    rolePlayer = VillagerPlayer;
                    break;
                case Role.SEER:
                    rolePlayer = SeerPlayer;
                    break;
                case Role.MEDIUM:
                    rolePlayer = MediumPlayer;
                    break;
                case Role.BODYGUARD:
                    rolePlayer = BodyguardPlayer;
                    break;
                case Role.POSSESSED:
                    rolePlayer = PossessedPlayer;
                    break;
                case Role.WEREWOLF:
                    rolePlayer = WerewolfPlayer;
                    break;
                default:
                    rolePlayer = VillagerPlayer;
                    break;
            }
            rolePlayer.Initialize(gameInfo, gameSetting);
        }

        /// <summary>
        /// Called when the day started.
        /// </summary>
        /// <remarks></remarks>
        void IPlayer.DayStart()
        {
            rolePlayer.DayStart();
        }

        /// <summary>
        /// Returns this player's talk.
        /// </summary>
        /// <returns>The string representing this player's talk.</returns>
        /// <remarks>
        /// The returned string must be written in aiwolf protocol.
        /// Null means SKIP.
        /// </remarks>
        string IPlayer.Talk()
        {
            return rolePlayer.Talk();
        }

        /// <summary>
        /// Returns this werewolf's whisper.
        /// </summary>
        /// <returns>The string representing this werewolf's whisper.</returns>
        /// <remarks>
        /// The returned string must be written in aiwolf protocol.
        /// Null means SKIP.
        /// </remarks>
        string IPlayer.Whisper()
        {
            return rolePlayer.Whisper();
        }

        /// <summary>
        /// Returns the agent this player wants to execute.
        /// </summary>
        /// <returns>The agent this player wants to execute.</returns>
        /// <remarks></remarks>
        Agent IPlayer.Vote()
        {
            return rolePlayer.Vote();
        }

        /// <summary>
        /// Returns the agent this werewolf wants to attack.
        /// </summary>
        /// <returns>The agent this werewolf wants to attack.</returns>
        /// <remarks></remarks>
        Agent IPlayer.Attack()
        {
            return rolePlayer.Attack();
        }

        /// <summary>
        /// Returns the agent this seer wants to divine.
        /// </summary>
        /// <returns>The agent this seer wants to divine.</returns>
        /// <remarks></remarks>
        Agent IPlayer.Divine()
        {
            return rolePlayer.Divine();
        }

        /// <summary>
        /// Returns the agent this bodyguard wants to guard.
        /// </summary>
        /// <returns>The agent this bodyguard wants to guard.</returns>
        /// <remarks></remarks>
        Agent IPlayer.Guard()
        {
            return rolePlayer.Guard();
        }

        /// <summary>
        /// Called when the game finishes.
        /// </summary>
        /// <remarks>Before this method is called, gameinfo is updated with all information.</remarks>
        void IPlayer.Finish()
        {
            rolePlayer.Finish();
        }
    }
}
