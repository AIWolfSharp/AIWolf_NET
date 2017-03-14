using System.Runtime.Serialization;

namespace AIWolf.Common.Data
{
    /// <summary>
    /// Information of the guard carried out by the bodyguard.
    /// </summary>
    /// <remarks></remarks>
    [DataContract]
    public class Guard
    {
        /// <summary>
        /// The day of the guard.
        /// </summary>
        /// <value>The day of the guard.</value>
        /// <remarks></remarks>
        [DataMember(Name = "day")]
        public int Day { get; }

        /// <summary>
        /// The agent of the bodyguard.
        /// </summary>
        /// <value>The agent of the bodyguard.</value>
        /// <remarks></remarks>
        [DataMember(Name = "agent")]
        public Agent Agent { get; }

        /// <summary>
        /// The agent guarded by the bodyguard.
        /// </summary>
        /// <value>The agent guarded.</value>
        /// <remarks></remarks>
        [DataMember(Name = "target")]
        public Agent Target { get; }

        /// <summary>
        /// Initializes a new instance of Guard class.
        /// </summary>
        /// <param name="day">The day of guard.</param>
        /// <param name="agent">The agent of the bodyguard.</param>
        /// <param name="target">The agent guarded by the bodyguard.</param>
        /// <remarks></remarks>
        public Guard(int day, Agent agent, Agent target)
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
            return Agent + " guarded " + Target + "@" + Day;
        }
    }
}