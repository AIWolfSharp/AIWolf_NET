using System.Runtime.Serialization;

namespace AIWolf.Common.Data
{
    /// <summary>
    /// Information of vote for execution.
    /// </summary>
    /// <remarks></remarks>
    [DataContract]
    public class Vote
    {
        /// <summary>
        /// The day of this vote.
        /// </summary>
        /// <value>The day of this vote.</value>
        /// <remarks></remarks>
        [DataMember(Name = "day")]
        public int Day { get; }

        /// <summary>
        /// The agent who voted.
        /// </summary>
        /// <value>The agent who voted.</value>
        /// <remarks></remarks>
        [DataMember(Name = "agent")]
        public Agent Agent { get; }

        /// <summary>
        /// The voted agent.
        /// </summary>
        /// <value>The voted agent.</value>
        /// <remarks></remarks>
        [DataMember(Name = "target")]
        public Agent Target { get; }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="day">The day of this vote.</param>
        /// <param name="agent">The agent who voted.</param>
        /// <param name="target">The voted agent.</param>
        /// <remarks></remarks>
        public Vote(int day, Agent agent, Agent target)
        {
            Day = day;
            Agent = agent;
            Target = target;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return Agent + "voted " + Target + "@" + Day;
        }
    }
}
