//
// Topic.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

﻿namespace AIWolf.Client.Lib
{
    /// <summary>
    /// Enum class for topic of talk/whisper.
    /// </summary>
    /// <remarks></remarks>
    public enum Topic
    {
        /// <summary>
        /// Estimation.
        /// </summary>
        /// <remarks></remarks>
        ESTIMATE,

        /// <summary>
        /// Comingout.
        /// </summary>
        /// <remarks></remarks>
        COMINGOUT,

        /// <summary>
        /// Divination.
        /// </summary>
        /// <remarks></remarks>
        DIVINED,

        /// <summary>
        /// Inquest.
        /// </summary>
        /// <remarks></remarks>
        INQUESTED,

        /// <summary>
        /// Guard.
        /// </summary>
        /// <remarks></remarks>
        GUARDED,

        /// <summary>
        /// Vote.
        /// </summary>
        /// <remarks></remarks>
        VOTE,

        /// <summary>
        /// Attack.
        /// </summary>
        /// <remarks></remarks>
        ATTACK,

        /// <summary>
        /// Agreement.
        /// </summary>
        /// <remarks></remarks>
        AGREE,

        /// <summary>
        /// Disagreement.
        /// </summary>
        /// <remarks></remarks>
        DISAGREE,

        /// <summary>
        /// There is nothing to talk/whisper.
        /// </summary>
        /// <remarks></remarks>
        OVER,

        /// <summary>
        /// Skip this turn though there is something to talk/whisper.
        /// </summary>
        /// <remarks></remarks>
        SKIP
    }
}
