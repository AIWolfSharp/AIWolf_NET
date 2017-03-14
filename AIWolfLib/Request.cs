//
// Request.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

namespace AIWolf.Lib
{
    /// <summary>
    /// Enumeration type for requests.
    /// </summary>
    enum Request
    {
        /// <summary>
        /// No request.
        /// Its integer value is 0.
        /// </summary>
        NO_REQUEST,

        /// <summary>
        /// Request for agent's initialization.
        /// Its integer value is 1.
        /// </summary>
        INITIALIZE,

        /// <summary>
        /// Request for agent's daily initialization.
        /// Its integer value is 2.
        /// </summary>
        DAILY_INITIALIZE,

        /// <summary>
        /// Request for agent's daily finish.
        /// Its integer value is 3.
        /// </summary>
        DAILY_FINISH,

        /// <summary>
        /// Request for agent's finish.
        /// Its integer value is 4.
        /// </summary>
        FINISH,

        /// <summary>
        /// Request for agent's name.
        /// Its integer value is 11.
        /// </summary>
        NAME = 11,

        /// <summary>
        /// Request for agent's role.
        /// Its integer value is 12.
        /// </summary>
        ROLE,

        /// <summary>
        /// Request for agent's talk.
        /// Its integer value is 13.
        /// </summary>
        TALK,

        /// <summary>
        /// Request for agent's whisper.
        /// Its integer value is 14.
        /// </summary>
        WHISPER,

        /// <summary>
        /// Request for agent's vote.
        /// Its integer value is 15.
        /// </summary>
        VOTE,

        /// <summary>
        /// Request for agent's divination.
        /// Its integer value is 16.
        /// </summary>
        DIVINE,

        /// <summary>
        /// Request for agent's guard.
        /// Its integer value is 17.
        /// </summary>
        GUARD,

        /// <summary>
        /// Request for agent's attack.
        /// Its integer value is 18.
        /// </summary>
        ATTACK
    }

    /// <summary>
    /// Defines extension method of enum Request.
    /// </summary>
    static class RequestExtensions
    {
        /// <summary>
        /// Returns whethere or not the request waits for return value.
        /// </summary>
        /// <param name="request">Request.</param>
        /// <returns>True if the request waits for return value, otherwise, false.</returns>
        public static bool HasReturn(this Request request)
        {
            return ((int)request > 10);
        }
    }
}
