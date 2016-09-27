//
// UnexpectedMethodCallException.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

﻿using AIWolf.Common;
using System;
using System.Runtime.Serialization;

namespace AIWolf.Client.Base.Player
{
    [Serializable]
    class UnexpectedMethodCallException : AIWolfRuntimeException
    {
        public UnexpectedMethodCallException()
        {
        }

        public UnexpectedMethodCallException(string message) : base(message)
        {
        }

        public UnexpectedMethodCallException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnexpectedMethodCallException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}