//
// IPacket.cs
//
// Copyright (c) 2018 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using AIWolf.Lib;
using System.Collections.Generic;

namespace AIWolf.Server
{
    public interface IPacket
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
        Request Request { get; }

#if JHELP
        /// <summary>
        /// ゲーム情報
        /// </summary>
#else
        /// <summary>
        /// The game information.
        /// </summary>
#endif
        IGameInfo GameInfo { get; }

#if JHELP
        /// <summary>
        /// ゲーム設定
        /// </summary>
#else
        /// <summary>
        /// The setting of game.
        /// </summary>
#endif
        IGameSetting GameSetting { get; }

#if JHELP
        /// <summary>
        /// 前のパケット以後の会話のリスト
        /// </summary>
#else
        /// <summary>
        /// The history of talks.
        /// </summary>
#endif
        IList<Talk> TalkHistory { get; }

#if JHELP
        /// <summary>
        /// 前のパケット以後の囁きのリスト
        /// </summary>
#else
        /// <summary>
        /// The history of whispers.
        /// </summary>
#endif
        IList<Whisper> WhisperHistory { get; }

    }
}
