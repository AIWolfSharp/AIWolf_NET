//
// Packet.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using AIWolf.Lib;
using AIWolf.Server;
using System.Collections.Generic;

namespace AIWolf.Client
{
#if JHELP
    /// <summary>
    /// サーバから送られてきたパケット
    /// </summary>
#else
    /// <summary>
    /// Packet sent from server.
    /// </summary>
#endif
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
        public IGameInfo GameInfo { get; }

#if JHELP
        /// <summary>
        /// ゲーム設定
        /// </summary>
#else
        /// <summary>
        /// The setting of game.
        /// </summary>
#endif
        public IGameSetting GameSetting { get; }

#if JHELP
        /// <summary>
        /// 前のパケット以後の会話のリスト
        /// </summary>
#else
        /// <summary>
        /// The history of talks.
        /// </summary>
#endif
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
        public Packet(Request request) => Request = request;

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
        public Packet(Request request, IGameInfo gameInfo) : this(request) => GameInfo = gameInfo;

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
        public Packet(Request request, IGameInfo gameInfo, IGameSetting gameSetting) : this(request, gameInfo) => GameSetting = gameSetting;

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
