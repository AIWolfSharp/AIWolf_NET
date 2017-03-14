using AIWolf.Common.Data;
using System.Runtime.Serialization;

namespace AIWolf.Common.Net
{
    /// <summary>
    /// The talk/whisper to be sent to each player.
    /// </summary>
    /// <remarks></remarks>
    [DataContract]
    public class TalkToSend
    {
        /// <summary>
        /// The index number of this talk/whisper.
        /// </summary>
        /// <value>The index number of this talk/whisper.</value>
        /// <remarks></remarks>
        [DataMember(Name = "idx")]
        public int Idx { get; set; }

        /// <summary>
        /// The day of this talk/whisper.
        /// </summary>
        /// <value>The day of this talk/whisper.</value>
        /// <remarks></remarks>
        [DataMember(Name = "day")]
        public int Day { get; set; }

        /// <summary>
        /// The index number of the agent who talked/whispered.
        /// </summary>
        /// <value>The index number of the agent who talked/whispered.</value>
        /// <remarks></remarks>
        [DataMember(Name = "agent")]
        public int Agent { get; set; }

        /// <summary>
        /// The contents of this talk/whisper.
        /// </summary>
        /// <value>The contents of this talk/whisper.</value>
        /// <remarks></remarks>
        [DataMember(Name = "content")]
        public string Content { get; set; }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <remarks></remarks>
        public TalkToSend()
        {
        }

        /// <summary>
        /// Initializes a new instance of this class with talk given.
        /// </summary>
        /// <param name="talk">Talk given.</param>
        /// <remarks></remarks>
        public TalkToSend(Talk talk)
        {
            Idx = talk.Idx;
            Day = talk.Day;
            Agent = talk.Agent.AgentIdx;
            Content = talk.Content;
        }

        /// <summary>
        /// Returns the instance of Talk class equivalent to this.
        /// </summary>
        /// <returns>The instance of Talk class equivalent to this.</returns>
        /// <remarks></remarks>
        public Talk ToTalk()
        {
            return new Talk(Idx, Day, Data.Agent.GetAgent(Agent), Content);
        }
    }
}
