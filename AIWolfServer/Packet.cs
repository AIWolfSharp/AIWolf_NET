﻿//
// Packet.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using AIWolf.Lib;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AIWolf.Server
{
#if JHELP
    /// <summary>
    /// エージェントに送るパケット
    /// </summary>
#else
    /// <summary>
    /// Packet for sending data to client.
    /// </summary>
#endif
    [DataContract]
    class Packet : IPacket
    {
#if JHELP
        /// <summary>
        /// リクエスト
        /// </summary>
#else
        /// <summary>
        /// The request from the server.
        /// </summary>
#endif
        [DataMember(Name = "request")]
        public Request Request { get; }

#if JHELP
        /// <summary>
        /// ゲーム情報
        /// </summary>
#else
        /// <summary>
        /// The game information.
        /// </summary>
#endif
        public IGameInfo GameInfo => gameInfo;

        [DataMember(Name = "gameInfo")]
        GameInfo gameInfo;

#if JHELP
        /// <summary>
        /// ゲーム設定
        /// </summary>
#else
        /// <summary>
        /// The setting of game.
        /// </summary>
#endif
        public IGameSetting GameSetting => gameSetting;

        [DataMember(Name = "gameSetting")]
        GameSetting gameSetting;

#if JHELP
        /// <summary>
        /// 前のパケット以後の会話のリスト
        /// </summary>
#else
        /// <summary>
        /// The history of talks.
        /// </summary>
#endif
        [DataMember(Name = "talkHistory")]
        public IList<Talk> TalkHistory { get; }

#if JHELP
        /// <summary>
        /// 前のパケット以後の囁きのリスト
        /// </summary>
#else
        /// <summary>
        /// The history of whispers.
        /// </summary>
#endif
        [DataMember(Name = "whisperHistory")]
        public IList<Whisper> WhisperHistory { get; }

#if JHELP
        /// <summary>
        /// パケットの新しいインスタンスをリクエストを与えて初期化する
        /// </summary>
        /// <param name="request">リクエスト</param>
#else
        /// <summary>
        /// Initializes a new instance of this class with given request.
        /// </summary>
        /// <param name="request">Request given.</param>
#endif
        public Packet(Request request)
        {
            Request = request;
        }

#if JHELP
        /// <summary>
        /// パケットの新しいインスタンスをリクエストとゲーム情報を与えて初期化する
        /// </summary>
        /// <param name="request">リクエスト</param>
        /// <param name="gameInfo">ゲーム情報</param>
#else
        /// <summary>
        /// Initializes a new instance of this class with request and game information given.
        /// </summary>
        /// <param name="request">Request given.</param>
        /// <param name="gameInfo">Game information given.</param>
#endif
        public Packet(Request request, GameInfo gameInfo) : this(request)
        {
            this.gameInfo = gameInfo;
        }

#if JHELP
        /// <summary>
        /// パケットの新しいインスタンスをリクエスト，ゲーム情報，ゲーム設定を与えて初期化する
        /// </summary>
        /// <param name="request">リクエスト</param>
        /// <param name="gameInfo">ゲーム情報</param>
        /// <param name="gameSetting">ゲーム設定</param>
#else
        /// <summary>
        /// Initializes a new instance of this class with request, game information and setting of game given.
        /// </summary>
        /// <param name="request">Request given.</param>
        /// <param name="gameInfo">Game information given.</param>
        /// <param name="gameSetting">GameSetting representation of setting of game given.</param>
#endif
        public Packet(Request request, GameInfo gameInfo, GameSetting gameSetting) : this(request, gameInfo)
        {
            this.gameSetting = gameSetting;
        }

#if JHELP
        /// <summary>
        /// パケットの新しいインスタンスをリクエスト，会話履歴，囁き履歴を与えて初期化する
        /// </summary>
        /// <param name="request">リクエスト</param>
        /// <param name="talkHistoryList">会話履歴</param>
        /// <param name="whisperHistoryList">囁き履歴</param>
#else
        /// <summary>
        /// Initializes a new instance of this class with request, history of talk and whisper given.
        /// </summary>
        /// <param name="request">Request given.</param>
        /// <param name="talkHistoryList">History of talk given.</param>
        /// <param name="whisperHistoryList">History of whisper given.</param>
#endif
        public Packet(Request request, IList<Talk> talkHistoryList, IList<Whisper> whisperHistoryList) : this(request)
        {
            TalkHistory = talkHistoryList;
            WhisperHistory = whisperHistoryList;
        }
    }
}
