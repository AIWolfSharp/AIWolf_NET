using System;
using System.Runtime.Serialization;

namespace AIWolf.Common.Data
{
    /// <summary>
    /// Talk/whisper.
    /// </summary>
    /// <remarks></remarks>
    [DataContract]
    public class Talk
    {
        /// <summary>
        /// There is nothing to talk/whisper.
        /// </summary>
        /// <remarks></remarks>
        public const string OVER = "Over";

        /// <summary>
        /// Skip this turn though there is something to talk/whisper.
        /// </summary>
        /// <remarks></remarks>
        public const string SKIP = "Skip";

        /// <summary>
        /// The index number of this talk/whisper.
        /// </summary>
        /// <value>The index number of this talk/whisper.</value>
        /// <remarks></remarks>
        [DataMember(Name = "idx")]
        public int Idx { get; }

        /// <summary>
        /// The day of this talk/whisper.
        /// </summary>
        /// <value>The day of this talk/whisper.</value>
        /// <remarks></remarks>
        [DataMember(Name = "day")]
        public int Day { get; }

        /// <summary>
        /// The agent who talked/whispered.
        /// </summary>
        /// <value>The agent who talked/whispered.</value>
        /// <remarks></remarks>
        [DataMember(Name = "agent")]
        public Agent Agent { get; }

        /// <summary>
        /// The contents of this talk/whisper.
        /// </summary>
        /// <value>The contents of this talk/whisper.</value>
        /// <remarks></remarks>
        [DataMember(Name = "content")]
        public string Content { get; }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="idx">The index of this talk/whisper.</param>
        /// <param name="day">The day of this talk/whisper.</param>
        /// <param name="agent">The agent who talked/whispered.</param>
        /// <param name="content">The contents of this talk/whisper.</param>
        /// <remarks></remarks>
        public Talk(int idx, int day, Agent agent, string content)
        {
            Idx = idx;
            Day = day;
            Agent = agent;
            Content = content;
        }

        /// <summary>
        /// Returns whether or not this talk/whisper is SKIP.
        /// </summary>
        /// <value>True if this talk/whisper is SKIP, otherwise, false.</value>
        /// <remarks></remarks>
        [DataMember(Name = "skip")]
        public bool Skip
        {
            get { return Content.Equals(SKIP); }
        }

        /// <summary>
        /// Returns whether or not this talk/whisper is OVER.
        /// </summary>
        /// <value>True if this talk/whisper is OVER, otherwise, false.</value>
        /// <remarks></remarks>
        [DataMember(Name = "over")]
        public bool Over
        {
            get { return Content.Equals(OVER); }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return String.Format("Day{0:D2}[{1:D3}]\t{2}\t{3}", Day, Idx, Agent, Content);
        }
    }
}
