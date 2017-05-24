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
#if JHELP
    /// <summary>
    /// サーバからエージェントへのリクエスト
    /// </summary>
#else
    /// <summary>
    /// Enumeration type for requests.
    /// </summary>
#endif
    enum Request
    {
#if JHELP
        /// <summary>
        /// リクエストなし．
        /// 整数値は 0
        /// </summary>
#else
        /// <summary>
        /// No request.
        /// Its integer value is 0.
        /// </summary>
#endif
        NO_REQUEST,

#if JHELP
        /// <summary>
        /// 初期化リクエスト．
        /// 整数値は 1
        /// </summary>
#else
        /// <summary>
        /// Request for agent's initialization.
        /// Its integer value is 1.
        /// </summary>
#endif
        INITIALIZE,

#if JHELP
        /// <summary>
        /// その日の初期化リクエスト．
        /// 整数値は 2
        /// </summary>
#else
        /// <summary>
        /// Request for agent's daily initialization.
        /// Its integer value is 2.
        /// </summary>
#endif
        DAILY_INITIALIZE,

#if JHELP
        /// <summary>
        /// その日の終了処理リクエスト．
        /// 整数値は 3
        /// </summary>
#else
        /// <summary>
        /// Request for agent's daily finish.
        /// Its integer value is 3.
        /// </summary>
#endif
        DAILY_FINISH,

#if JHELP
        /// <summary>
        /// 終了処理リクエスト．
        /// 整数値は 4
        /// </summary>
#else
        /// <summary>
        /// Request for agent's finish.
        /// Its integer value is 4.
        /// </summary>
#endif
        FINISH,

#if JHELP
        /// <summary>
        /// 名前リクエスト．
        /// 整数値は 11
        /// </summary>
#else
        /// <summary>
        /// Request for agent's name.
        /// Its integer value is 11.
        /// </summary>
#endif
        NAME = 11,

#if JHELP
        /// <summary>
        /// 役職リクエスト．
        /// 整数値は 12
        /// </summary>
#else
        /// <summary>
        /// Request for agent's role.
        /// Its integer value is 12.
        /// </summary>
#endif
        ROLE,

#if JHELP
        /// <summary>
        /// 会話リクエスト．
        /// 整数値は 13
        /// </summary>
#else
        /// <summary>
        /// Request for agent's talk.
        /// Its integer value is 13.
        /// </summary>
#endif
        TALK,

#if JHELP
        /// <summary>
        /// 囁きリクエスト．
        /// 整数値は 14
        /// </summary>
#else
        /// <summary>
        /// Request for agent's whisper.
        /// Its integer value is 14.
        /// </summary>
#endif
        WHISPER,

#if JHELP
        /// <summary>
        /// 追放投票リクエスト．
        /// 整数値は 15
        /// </summary>
#else
        /// <summary>
        /// Request for agent's vote.
        /// Its integer value is 15.
        /// </summary>
#endif
        VOTE,

#if JHELP
        /// <summary>
        /// 占い先リクエスト．
        /// 整数値は 16
        /// </summary>
#else
        /// <summary>
        /// Request for agent's divination.
        /// Its integer value is 16.
        /// </summary>
#endif
        DIVINE,

#if JHELP
        /// <summary>
        /// 護衛先リクエスト．
        /// 整数値は 17
        /// </summary>
#else
        /// <summary>
        /// Request for agent's guard.
        /// Its integer value is 17.
        /// </summary>
#endif
        GUARD,


#if JHELP
        /// <summary>
        /// 襲撃投票先リクエスト．
        /// 整数値は 18
        /// </summary>
#else
        /// <summary>
        /// Request for agent's attack.
        /// Its integer value is 18.
        /// </summary>
#endif
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
        public static bool HasReturn(this Request request) => ((int)request > 10);
    }
}
