using AIWolf.Common.Data;
using System;
using System.Text.RegularExpressions;

namespace AIWolf.Client.Lib
{
    /// <summary>
    /// Result of parsing talk/whisper.
    /// </summary>
    /// <remarks></remarks>
    public class Utterance
    {
        /// <summary>
        /// The raw contents of the talk/whisper. 
        /// </summary>
        /// <value>The raw contents of the talk/whisper.</value>
        /// <remarks></remarks>
        public string Text { get; }

        /// <summary>
        /// The topic of the talk/whisper.
        /// </summary>
        /// <value>The topic of the talk/whisper.</value>
        /// <remarks></remarks>
        public Topic? Topic { get; }

        /// <summary>
        /// The agent whom the talker/whisperer aims at.
        /// </summary>
        /// <value>The agent whom the talker/whisperer aims at.</value>
        /// <remarks></remarks>
        public Agent Target { get; }

        /// <summary>
        /// The type of the talk/whisper.
        /// </summary>
        /// <value>TALK/WHISPER.</value>
        /// <remarks></remarks>
        public TalkType? TalkType { get; }

        /// <summary>
        /// The day of the talk/whisper of which the talker/whisperer agrees/disagrees.
        /// </summary>
        /// <value>The day of the talk/whisper of which the talker/whisperer agrees/disagrees.</value>
        /// <remarks></remarks>
        public int TalkDay { get; }

        /// <summary>
        /// The index number of talk/whisper of which the talker/whisperer agrees/disagrees.
        /// </summary>
        /// <value>The index number of talk/whisper of which the talker/whisperer agrees/disagrees.</value>
        /// <remarks></remarks>
        public int TalkID { get; }

        /// <summary>
        /// The estimated/confessed role.
        /// </summary>
        /// <value>The estimated/confessed role.</value>
        /// <remarks>If the topic is not ESTIMATE or COMINGOUT, null.</remarks>
        public Role? Role { get; } = null;

        /// <summary>
        /// The species which the investigated agent is found to be.
        /// </summary>
        /// <value>The species the investigated agent is found to be.</value>
        /// <remarks>If the topic is not DIVINED or INQUESTED, null.</remarks>
        public Species? Result { get; } = null;

        /// <summary>
        /// Initializes a new instance of Utterance class with the contents of the talk/whisper.
        /// </summary>
        /// <param name="input">The contents of the talk/whisper.</param>
        /// <remarks></remarks>
        public Utterance(string input)
        {
            TalkDay = -1;
            TalkID = -1;
            Text = input;

            string[] split = input.Split();
            Topic = GetTopic(split[0]);
            int agentId = -1;
            if (split.Length >= 2 && split[1].StartsWith("Agent"))
            {
                agentId = GetInt(split[1]);
            }

            switch (Topic)
            {
                case Lib.Topic.SKIP:
                case Lib.Topic.OVER:
                    break;

                case Lib.Topic.AGREE:
                case Lib.Topic.DISAGREE:
                    // ex. Talk day4 ID:38
                    TalkType = ParseTalkType(split[1]);
                    TalkDay = GetInt(split[2]);
                    TalkID = GetInt(split[3]);
                    break;

                case Lib.Topic.ESTIMATE:
                case Lib.Topic.COMINGOUT:
                    // Topic Agent Role
                    Target = Agent.GetAgent(agentId);
                    Role = ParseRole(split[2]);
                    break;

                case Lib.Topic.DIVINED:
                case Lib.Topic.INQUESTED:
                    Target = Agent.GetAgent(agentId);
                    Result = ParseSpecies(split[2]);
                    break;

                case Lib.Topic.GUARDED:
                    Target = Agent.GetAgent(agentId);
                    break;

                case Lib.Topic.ATTACK:
                case Lib.Topic.VOTE:
                    Target = Agent.GetAgent(agentId);
                    break;

                default:
                    return;
            }
            return;
        }

        int GetInt(string text)
        {
            var m = new Regex(@"-?[\d]+").Match(text);
            if (m.Success)
            {
                return int.Parse(m.Value);
            }
            return -1;
        }

        Topic? GetTopic(string s)
        {
            foreach (Topic topic in Enum.GetValues(typeof(Topic)))
            {
                if (topic.ToString().Equals(s, StringComparison.CurrentCultureIgnoreCase))
                {
                    return topic;
                }
            }
            return null;
        }

        TalkType? ParseTalkType(string input)
        {
            if (input.Equals("talk", StringComparison.CurrentCultureIgnoreCase))
            {
                return Lib.TalkType.TALK;
            }
            else if (input.Equals("whisper", StringComparison.CurrentCultureIgnoreCase))
            {
                return Lib.TalkType.WHISPER;
            }
            else
            {
                return null;
            }
        }

        Role? ParseRole(string input)
        {
            foreach (Role r in Enum.GetValues(typeof(Role)))
            {
                if (r.ToString().Equals(input, StringComparison.CurrentCultureIgnoreCase))
                {
                    return r;
                }
            }
            return null;
        }

        Species? ParseSpecies(string input)
        {
            foreach (Species s in Enum.GetValues(typeof(Species)))
            {
                if (s.ToString().Equals(input, StringComparison.CurrentCultureIgnoreCase))
                {
                    return s;
                }
            }
            return null;
        }
    }
}
