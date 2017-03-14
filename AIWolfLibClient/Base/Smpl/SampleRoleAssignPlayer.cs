using AIWolf.Client.Base.Player;

namespace AIWolf.Client.Base.Smpl
{
    /// <summary>
    /// Sample player class which assigns special player according to its role.
    /// </summary>
    /// <remarks></remarks>
    public class SampleRoleAssignPlayer : AbstractRoleAssignPlayer
    {
        /// <summary>
        /// Initializes a new instance of SampleRoleAssignPlayer class.
        /// </summary>
        /// <remarks></remarks>
        public SampleRoleAssignPlayer()
        {
            VillagerPlayer = new SampleVillager();
            SeerPlayer = new SampleSeer();
            MediumPlayer = new SampleMedium();
            BodyguardPlayer = new SampleBodyguard();
            PossessedPlayer = new SamplePossessed();
            WerewolfPlayer = new SampleWerewolf();
        }

        /// <summary>
        /// This player's name.
        /// </summary>
        /// <value>This player's name.</value>
        /// <remarks></remarks>
        public override string Name
        {
            get { return typeof(SampleRoleAssignPlayer).Name; }
        }
    }
}
