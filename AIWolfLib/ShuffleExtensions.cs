//
// ShuffleExtensions.cs
//
// Copyright (c) 2017 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using System;
using System.Collections.Generic;
using System.Linq;

namespace AIWolf.Lib
{
#if JHELP
    /// <summary>
    /// IEnumerableインターフェースを実装したクラスに対するShuffle拡張メソッド定義
    /// </summary>
#else
    /// <summary>
    /// Defines extension method to shuffle what implements IEnumerable interface.
    /// </summary>
#endif
    public static class ShuffleExtensions
    {
#if JHELP
        /// <summary>
        /// IEnumerableをシャッフルしたものを返す
        /// </summary>
        /// <typeparam name="T">IEnumerableの要素の型</typeparam>
        /// <param name="s">TのIEnumerable</param>
        /// <returns>シャッフルされたIEnumerable</returns>
#else
        /// <summary>
        /// Returns shuffled IEnumerable of T.
        /// </summary>
        /// <typeparam name="T">Type of element of IEnumerable.</typeparam>
        /// <param name="s">IEnumerable of T.</param>
        /// <returns>Shuffled IEnumerable of T.</returns>
#endif
        public static IOrderedEnumerable<T> Shuffle<T>(this IEnumerable<T> s)
        {
            return s.OrderBy(x => Guid.NewGuid());
        }
    }
}
