//
// IUtterance.cs
//
// Copyright (c) 2018 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

namespace AIWolf.Lib
{
#if JHELP
    /// <summary>
    /// 発話クラスが実装すべきプロパティ
    /// </summary>
#else
    /// <summary>
    /// The properties to be in the class for utterances.
    /// </summary>
#endif
    public interface IUtterance
    {
#if JHELP
        /// <summary>
        /// 発話のインデックス
        /// </summary>
#else
        /// <summary>
        /// The index number of this utterance.
        /// </summary>
#endif
        int Idx { get; }

#if JHELP
        /// <summary>
        /// 発話日
        /// </summary>
#else
        /// <summary>
        /// The day of this utterance.
        /// </summary>
#endif
        int Day { get; }

#if JHELP
        /// <summary>
        /// 発話ターン
        /// </summary>
#else
        /// <summary>
        /// The turn of this utterance.
        /// </summary>
#endif
        int Turn { get; }

#if JHELP
        /// <summary>
        /// 発話エージェント
        /// </summary>
#else
        /// <summary>
        /// The agent who uttered.
        /// </summary>
#endif
        Agent Agent { get; }

#if JHELP
        /// <summary>
        /// 発話テキスト
        /// </summary>
#else
        /// <summary>
        /// The contents of this utterance.
        /// </summary>
#endif
        string Text { get; }
    }
}
