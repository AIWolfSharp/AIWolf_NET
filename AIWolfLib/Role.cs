//
// Role.cs
//
// Copyright (c) 2016 Takashi OTSUKI
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
        WEREWOLF
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
        static Dictionary<Role, Species> roleSpeciesMap = new Dictionary<Role, Species>();

        static RoleExtensions()
        {
            roleSpeciesMap[Role.UNC] = Species.UNC;
            roleSpeciesMap[Role.BODYGUARD] = Species.HUMAN;
            roleSpeciesMap[Role.FOX] = Species.HUMAN;
            roleSpeciesMap[Role.FREEMASON] = Species.HUMAN;
            roleSpeciesMap[Role.MEDIUM] = Species.HUMAN;
            roleSpeciesMap[Role.POSSESSED] = Species.HUMAN;
            roleSpeciesMap[Role.SEER] = Species.HUMAN;
            roleSpeciesMap[Role.VILLAGER] = Species.HUMAN;
            roleSpeciesMap[Role.WEREWOLF] = Species.WEREWOLF;
        }

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
        public static Species GetSpecies(this Role role)
        {
            return roleSpeciesMap[role];
        }
    }
}
