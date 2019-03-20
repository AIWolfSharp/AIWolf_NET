//
// Enums.cs
//
// Copyright (c) 2017 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using System.Collections.Generic;

namespace AIWolf.Lib
{
#if JHELP
    /// <summary>
    /// 役職
    /// </summary>
#else
    /// <summary>
    /// Enumeration type for role.
    /// </summary>
#endif
    public enum Role
    {
#if JHELP
        /// <summary>
        /// 不明
        /// </summary>
#else
        /// <summary>
        /// Uncertain.
        /// </summary>
#endif
        UNC,

#if JHELP
        /// <summary>
        /// 狩人
        /// </summary>
#else
        /// <summary>
        /// Bodyguard.
        /// </summary>
#endif
        BODYGUARD,

#if JHELP
        /// <summary>
        /// 妖狐
        /// </summary>
#else
        /// <summary>
        /// Fox.
        /// </summary>
#endif
        FOX,

#if JHELP
        /// <summary>
        /// 共有者
        /// </summary>
#else
        /// <summary>
        /// Freemason.
        /// </summary>
#endif
        FREEMASON,

#if JHELP
        /// <summary>
        /// 霊媒師
        /// </summary>
#else
        /// <summary>
        /// Medium.
        /// </summary>
#endif
        MEDIUM,

#if JHELP
        /// <summary>
        /// 裏切り者
        /// </summary>
#else
        /// <summary>
        /// Possessed human.
        /// </summary>
#endif
        POSSESSED,

#if JHELP
        /// <summary>
        /// 占い師
        /// </summary>
#else
        /// <summary>
        /// Seer.
        /// </summary>
#endif
        SEER,

#if JHELP
        /// <summary>
        /// 村人
        /// </summary>
#else
        /// <summary>
        /// Villager.
        /// </summary>
#endif
        VILLAGER,

#if JHELP
        /// <summary>
        /// 人狼
        /// </summary>
#else
        /// <summary>
        /// Werewolf.
        /// </summary>
#endif
        WEREWOLF,

#if JHELP
        /// <summary>
        /// ワイルドカード
        /// </summary>
#else
        /// <summary>
        /// Wildcard.
        /// </summary>
#endif
        ANY = int.MaxValue
    }

#if JHELP
    /// <summary>
    /// 陣営
    /// </summary>
#else
    /// <summary>
    /// Enumeration type for teams.
    /// </summary>
#endif
    public enum Team
    {
#if JHELP
        /// <summary>
        /// 不明
        /// </summary>
#else
        /// <summary>
        /// Uncertain.
        /// </summary>
#endif
        UNC,

#if JHELP
        /// <summary>
        /// 村人陣営
        /// </summary>
#else
        /// <summary>
        /// Villager team.
        /// </summary>
#endif
        VILLAGER,

#if JHELP
        /// <summary>
        /// 人狼陣営
        /// </summary>
#else
        /// <summary>
        /// Werewolf team.
        /// </summary>
#endif
        WEREWOLF,

#if JHELP
        /// <summary>
        /// 第三陣営
        /// </summary>
#else
        /// <summary>
        /// The third team.
        /// </summary>
#endif
        OTHERS
    }

#if JHELP
    /// <summary>
    /// 種族
    /// </summary>
#else
    /// <summary>
    /// Enumeration type for species.
    /// </summary>
#endif
    public enum Species
    {
#if JHELP
        /// <summary>
        /// 不明
        /// </summary>
#else
        /// <summary>
        /// Uncertain.
        /// </summary>
#endif
        UNC,

#if JHELP
        /// <summary>
        /// 人間
        /// </summary>
#else
        /// <summary>
        /// Human.
        /// </summary>
#endif
        HUMAN,

#if JHELP
        /// <summary>
        /// 人狼
        /// </summary>
#else
        /// <summary>
        /// Werewolf.
        /// </summary>
#endif
        WEREWOLF,

#if JHELP
        /// <summary>
        /// ワイルドカード
        /// </summary>
#else
        /// <summary>
        /// Wildcard.
        /// </summary>
#endif
        ANY = int.MaxValue
    }

#if JHELP
    /// <summary>
    /// 列挙型Roleの拡張メソッド定義
    /// </summary>
#else
    /// <summary>
    /// Defines extension method of enum Role.
    /// </summary>
#endif
    public static class RoleExtensions
    {
        static Dictionary<Role, Team> roleTeamMap = new Dictionary<Role, Team>
        {
            [Role.UNC] = Team.UNC,
            [Role.BODYGUARD] = Team.VILLAGER,
            [Role.FOX] = Team.OTHERS,
            [Role.FREEMASON] = Team.VILLAGER,
            [Role.MEDIUM] = Team.VILLAGER,
            [Role.POSSESSED] = Team.WEREWOLF,
            [Role.SEER] = Team.VILLAGER,
            [Role.VILLAGER] = Team.VILLAGER,
            [Role.WEREWOLF] = Team.WEREWOLF,
        };

        static Dictionary<Role, Species> roleSpeciesMap = new Dictionary<Role, Species>
        {
            [Role.UNC] = Species.UNC,
            [Role.BODYGUARD] = Species.HUMAN,
            [Role.FOX] = Species.HUMAN,
            [Role.FREEMASON] = Species.HUMAN,
            [Role.MEDIUM] = Species.HUMAN,
            [Role.POSSESSED] = Species.HUMAN,
            [Role.SEER] = Species.HUMAN,
            [Role.VILLAGER] = Species.HUMAN,
            [Role.WEREWOLF] = Species.WEREWOLF,
        };

#if JHELP
        /// <summary>
        /// 役職に対応する陣営を返す
        /// </summary>
        /// <param name="role">役職</param>
        /// <returns>役職に対応する陣営</returns>
#else
        /// <summary>
        /// Returns the team the role belongs to.
        /// </summary>
        /// <param name="role">Role.</param>
        /// <returns>The team the role belongs to.</returns>
#endif
        public static Team GetTeam(this Role role) => roleTeamMap[role];

#if JHELP
        /// <summary>
        /// 役職に対応する種族を返す
        /// </summary>
        /// <param name="role">役職</param>
        /// <returns>役職に対応する種族</returns>
#else
        /// <summary>
        /// Returns the species the role belongs to.
        /// </summary>
        /// <param name="role">Role.</param>
        /// <returns>The species the role belongs to.</returns>
#endif
        public static Species GetSpecies(this Role role) => roleSpeciesMap[role];
    }
}
