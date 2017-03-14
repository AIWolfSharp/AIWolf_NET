using AIWolf.Common.Data;
using System.Runtime.Serialization;

namespace AIWolf.Common.Net
{
    /// <summary>
    /// The vote information to be sent to each player.
    /// </summary>
    /// <remarks></remarks>
    [DataContract]
    public class VoteToSend
    {
        /// <summary>
        /// The day of this vote.
        /// </summary>
        /// <value>The day of this vote.</value>
        /// <remarks></remarks>
        [DataMember(Name = "day")]
        public int Day { get; set; }

        /// <summary>
        /// The index number of the agent who voted.
        /// </summary>
        /// <value>The index number of the agent who voted.</value>
        /// <remarks></remarks>
        [DataMember(Name = "agent")]
        public int Agent { get; set; }

        /// <summary>
        /// The index number of the voted agent.
        /// </summary>
        /// <value>The index number of the voted agent.</value>
        /// <remarks></remarks>
        [DataMember(Name = "target")]
        public int Target { get; set; }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <remarks></remarks>
        public VoteToSend()
        {
        }

        /// <summary>
        /// Initializes a new instance of this class with vote information.
        /// </summary>
        /// <param name="vote">Vote.</param>
        /// <remarks></remarks>
        public VoteToSend(Vote vote)
        {
            Day = vote.Day;
            Agent = vote.Agent.AgentIdx;
            Target = vote.Target.AgentIdx;
        }

        /// <summary>
        /// Returns the instance of Vote class equivalent to this.
        /// </summary>
        /// <returns>The instance of Vote class equivalent to this.</returns>
        /// <remarks></remarks>
        public Vote ToVote()
        {
            return new Vote(Day, Data.Agent.GetAgent(Agent), Data.Agent.GetAgent(Target));
        }
    }
}
