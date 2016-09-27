//
// ShuffleExtensions.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AIWolf.Common.Util
{
    /// <summary>
    /// Defines extension method to shuffle what implements IEnumerable interface.
    /// </summary>
    /// <remarks></remarks>
    public static class ShuffleExtensions
    {
        /// <summary>
        /// Returns randomized sequence of T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s">Sequence of T.</param>
        /// <returns>Randomized sequence of T.</returns>
        /// <remarks></remarks>
        public static IOrderedEnumerable<T> Shuffle<T>(this IEnumerable<T> s)
        {
            return s.OrderBy(x => Guid.NewGuid());
        }
    }
}
