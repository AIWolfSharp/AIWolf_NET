using AIWolf.Common.Data;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AIWolf.Common.Net
{
    /// <summary>
    /// Packet for sending data to client.
    /// </summary>
    /// <remarks></remarks>
    [DataContract]
    public class Packet
    {
        /// <summary>
        /// The request from the server.
        /// </summary>
        /// <value>Request.</value>
        /// <remarks></remarks>
        [DataMember(Name = "request")]
        public Request Request { get; set; }

        /// <summary>
        /// The game information.
        /// </summary>
        /// <value>The instance of GameInfoToSend class representating the game information.</value>
        /// <remarks></remarks>
        [DataMember(Name = "gameInfo")]
        public GameInfoToSend GameInfo { get; set; }

        /// <summary>
        /// The setting of game.
        /// </summary>
        /// <value>The instance of GameSetting class representating the game setting.</value>
        /// <remarks></remarks>
        [DataMember(Name = "gameSetting")]
        public GameSetting GameSetting { get; }

        /// <summary>
        /// The history of talks.
        /// </summary>
        /// <value>The list of TalkToSend representating the history of talks.</value>
        /// <remarks></remarks>
        [DataMember(Name = "talkHistory")]
        public List<TalkToSend> TalkHistory { get; set; }

        /// <summary>
        /// The history of whispers.
        /// </summary>
        /// <value>The list of TalkToSend representating the history of whispers.</value>
        /// <remarks></remarks>
        [DataMember(Name = "whisperHistory")]
        public List<TalkToSend> WhisperHistory { get; set; }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <remarks></remarks>
        public Packet()
        {
        }

        /// <summary>
        /// Initializes a new instance of this class with given request.
        /// </summary>
        /// <param name="request">Request given.</param>
        /// <remarks></remarks>
        public Packet(Request request)
        {
            Request = request;
        }

        /// <summary>
        /// Initializes a new instance of this class with request and game information given.
        /// </summary>
        /// <param name="request">Request given.</param>
        /// <param name="gameInfoToSend">GemeInfoToSend representation of game information given.</param>
        /// <remarks></remarks>
        public Packet(Request request, GameInfoToSend gameInfoToSend)
        {
            Request = request;
            GameInfo = gameInfoToSend;
        }

        /// <summary>
        /// Initializes a new instance of this class with request, game information and setting of game given.
        /// </summary>
        /// <param name="request">Request given.</param>
        /// <param name="gameInfoToSend">GemeInfoToSend representation of game information given.</param>
        /// <param name="gameSetting">GameSetting representation of setting of game given.</param>
        /// <remarks></remarks>
        public Packet(Request request, GameInfoToSend gameInfoToSend, GameSetting gameSetting)
        {
            Request = request;
            GameInfo = gameInfoToSend;
            GameSetting = gameSetting;
        }

        /// <summary>
        /// Initializes a new instance of this class with request, history of talk and whisper given.
        /// </summary>
        /// <param name="request">Request given.</param>
        /// <param name="talkHistoryList">History of talk given.</param>
        /// <param name="whisperHistoryList">History of whisper given.</param>
        /// <remarks></remarks>
        public Packet(Request request, List<TalkToSend> talkHistoryList, List<TalkToSend> whisperHistoryList)
        {
            Request = request;
            TalkHistory = talkHistoryList;
            WhisperHistory = whisperHistoryList;
        }
    }
}
