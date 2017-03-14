using System.Collections.Generic;

namespace AIWolf.Common.Data
{
    /// <summary>
    /// Enum class for requests.
    /// </summary>
    /// <remarks></remarks>
    public enum Request
    {
        /// <summary>
        /// Request for agent's name.
        /// </summary>
        NAME,

        /// <summary>
        /// Request for agent's role.
        /// </summary>
        ROLE,

        /// <summary>
        /// Request for agent's talk.
        /// </summary>
        TALK,

        /// <summary>
        /// Request for agent's whisper.
        /// </summary>
        WHISPER,

        /// <summary>
        /// Request for agent's vote.
        /// </summary>
        VOTE,

        /// <summary>
        /// Request for agent's divination.
        /// </summary>
        DIVINE,

        /// <summary>
        /// Request for agent's guard.
        /// </summary>
        GUARD,

        /// <summary>
        /// Request for agent's attack.
        /// </summary>
        ATTACK,

        /// <summary>
        /// Request for agent's initialization.
        /// </summary>
        INITIALIZE,

        /// <summary>
        /// Request for agent's daily initialization.
        /// </summary>
        DAILY_INITIALIZE,

        /// <summary>
        /// Request for agent's daily finish.
        /// </summary>
        DAILY_FINISH,

        //	UPDATE;

        /// <summary>
        /// Request for agent's finish.
        /// </summary>
        FINISH,

        /// <summary>
        /// Dummy request.
        /// </summary>
        DUMMY
    }

    /// <summary>
    /// Defines extension method of enum Request.
    /// </summary>
    /// <remarks></remarks>
    public static class RequestExtensions
    {
        static Dictionary<Request, bool> hasReturnMap = new Dictionary<Request, bool>();

        static RequestExtensions()
        {
            hasReturnMap[Request.NAME] = true;
            hasReturnMap[Request.ROLE] = true;
            hasReturnMap[Request.TALK] = true;
            hasReturnMap[Request.WHISPER] = true;
            hasReturnMap[Request.VOTE] = true;
            hasReturnMap[Request.DIVINE] = true;
            hasReturnMap[Request.GUARD] = true;
            hasReturnMap[Request.ATTACK] = true;
            hasReturnMap[Request.INITIALIZE] = false;
            hasReturnMap[Request.DAILY_INITIALIZE] = false;
            hasReturnMap[Request.DAILY_FINISH] = false;
            //hasReturnMap[Request.UPDATE] = false;
            hasReturnMap[Request.FINISH] = false;
            hasReturnMap[Request.DUMMY] = false;
        }

        /// <summary>
        /// Returns whethere or not the request waits for return value.
        /// </summary>
        /// <param name="request">Request.</param>
        /// <returns>True if the request waits for return value, otherwise, false.</returns>
        /// <remarks></remarks>
        public static bool HasReturn(this Request request)
        {
            return hasReturnMap[request];
        }
    }
}
