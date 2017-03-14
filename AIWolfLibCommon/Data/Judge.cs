using System.Runtime.Serialization;

namespace AIWolf.Common.Data
{
    /// <summary>
    /// The judge whether the player is human or werewolf.
    /// </summary>
    /// <remarks></remarks>
    [DataContract]
    public class Judge
    {
        /// <summary>
        /// The day of this judge.
        /// </summary>
        /// <value>The day of this judge.</value>
        /// <remarks></remarks>
        [DataMember(Name = "day")]
        public int Day { get; }

        /// <summary>
        /// The agent who judged.
        /// </summary>
        /// <value>The agent who judged.</value>
        /// <remarks></remarks>
        [DataMember(Name = "agent")]
        public Agent Agent { get; }

        /// <summary>
        /// The judged agent.
        /// </summary>
        /// <value>The judged agent.</value>
        /// <remarks></remarks>
        [DataMember(Name = "target")]
        public Agent Target { get; }

        /// <summary>
        /// The result of this judge.
        /// </summary>
        /// <value>Whether the judged agent is human or werewolf.</value>
        /// <remarks></remarks>
        [DataMember(Name = "result")]
        public Species Result { get; }

        /// <summary>
        /// Initializes a new instance of Judge class.
        /// </summary>
        /// <param name="day">The day of this judge.</param>
        /// <param name="agent">The agent who judged.</param>
        /// <param name="target">The judged agent.</param>
        /// <param name="result">The result of this judge.</param>
        /// <remarks></remarks>
        public Judge(int day, Agent agent, Agent target, Species result)
        {
            Day = day;
            Agent = agent;
            Target = target;
            Result = result;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return Agent + "->" + Target + "@" + Day + ":" + Result;
        }
    }
}
