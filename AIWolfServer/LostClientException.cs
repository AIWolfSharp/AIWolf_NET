//
// LostClientException.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using AIWolf.Lib;
using System;

namespace AIWolf.Server
{
    class LostClientException : Exception
    {
        public Agent Agent { get; }

        public LostClientException()
        {
        }

        public LostClientException(string message) : base(message)
        {
        }

        public LostClientException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public LostClientException(string message, Exception innerException, Agent agent) : base(message, innerException)
        {
            Agent = agent;
        }
    }
}