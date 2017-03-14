//
// Species.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

namespace AIWolf.Lib
{
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
        WEREWOLF
    }
}
