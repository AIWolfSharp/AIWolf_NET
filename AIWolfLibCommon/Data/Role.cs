using System.Collections.Generic;

namespace AIWolf.Common.Data
{
    /// <summary>
    /// Enum class for roles of player.
    /// </summary>
    /// <remarks></remarks>
    public enum Role
    {
        /// <summary>
        /// Bodyguard.
        /// </summary>
        BODYGUARD,

        /// <summary>
        /// Freemason. This is not used for now.
        /// </summary>
        FREEMASON,

        /// <summary>
        /// Medium.
        /// </summary>
        MEDIUM,

        /// <summary>
        /// Possessed human.
        /// </summary>
        POSSESSED,

        /// <summary>
        /// Seer.
        /// </summary>
        SEER,

        /// <summary>
        /// Villager.
        /// </summary>
        VILLAGER,

        /// <summary>
        /// Werewolf.
        /// </summary>
        WEREWOLF
    }

    /// <summary>
    /// Defines extension method of enum Role.
    /// </summary>
    /// <remarks></remarks>
    public static class RoleExtensions
    {
        static Dictionary<Role, Team> roleTeamMap = new Dictionary<Role, Team>();
        static Dictionary<Role, Species> roleSpeciesMap = new Dictionary<Role, Species>();

        static RoleExtensions()
        {
            roleTeamMap[Role.BODYGUARD] = Team.VILLAGER;
            roleSpeciesMap[Role.BODYGUARD] = Species.HUMAN;

            // This is not used for now.
            roleTeamMap[Role.FREEMASON] = Team.VILLAGER;
            roleSpeciesMap[Role.FREEMASON] = Species.HUMAN;

            roleTeamMap[Role.MEDIUM] = Team.VILLAGER;
            roleSpeciesMap[Role.MEDIUM] = Species.HUMAN;

            roleTeamMap[Role.POSSESSED] = Team.WEREWOLF;
            roleSpeciesMap[Role.POSSESSED] = Species.HUMAN;

            roleTeamMap[Role.SEER] = Team.VILLAGER;
            roleSpeciesMap[Role.SEER] = Species.HUMAN;

            roleTeamMap[Role.VILLAGER] = Team.VILLAGER;
            roleSpeciesMap[Role.VILLAGER] = Species.HUMAN;

            roleTeamMap[Role.WEREWOLF] = Team.WEREWOLF;
            roleSpeciesMap[Role.WEREWOLF] = Species.WEREWOLF;
        }

        /// <summary>
        /// Returns the team the role belongs to.
        /// </summary>
        /// <param name="role">Role.</param>
        /// <returns>The team the role belongs to.</returns>
        /// <remarks></remarks>
        public static Team GetTeam(this Role role)
        {
            return roleTeamMap[role];
        }

        /// <summary>
        /// Returns the species the role belongs to.
        /// </summary>
        /// <param name="role">Role.</param>
        /// <returns>The species the role belongs to.</returns>
        /// <remarks></remarks>
        public static Species GetSpecies(this Role role)
        {
            return roleSpeciesMap[role];
        }
    }
}
