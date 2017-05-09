//
// Guard.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using AIWolf.Lib;

namespace AIWolf.Server
{
    /// <summary>
    /// Guard class.
    /// </summary>
    public class Guard
    {
        public int Day { get; }

        public Agent Agent { get; }

        public Agent Target { get; }

        public Guard(int day, Agent agent, Agent target)
        {
            Day = day;
            Agent = agent;
            Target = target;
        }

        public override string ToString()
        {
            return Agent + " guarded " + Target + "@" + Day;
        }
    }
}