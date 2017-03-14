using AIWolf.Common.Data;
using System;
using System.Runtime.Serialization;

namespace AIWolf.Common.Net
{
    /// <summary>
    /// The judge information to be sent to each player.
    /// </summary>
    /// <remarks></remarks>
    [DataContract]
    public class JudgeToSend
    {
        /// <summary>
        /// The day of this judge.
        /// </summary>
        /// <value>The day of this judge.</value>
        /// <remarks></remarks>
        [DataMember(Name = "day")]
        public int Day { get; set; }

        /// <summary>
        /// The index number of the agent who judged.
        /// </summary>
        /// <value>The index number of the agent who judged.</value>
        /// <remarks></remarks>
        [DataMember(Name = "agent")]
        public int Agent { get; set; }

        /// <summary>
        /// The index nunmber of the judged agent.
        /// </summary>
        /// <value>The index number of the judged agent.</value>
        /// <remarks></remarks>
        [DataMember(Name = "target")]
        public int Target { get; set; }

        /// <summary>
        /// The result of this judge in string.
        /// </summary>
        /// <value>"HUMAN" or "WEREWOLF".</value>
        /// <remarks></remarks>
        [DataMember(Name = "result")]
        public string Result { get; set; }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <remarks></remarks>
        public JudgeToSend()
        {
        }

        /// <summary>
        /// Initializes a new instance of this class equivalent to the instance of Judge given.
        /// </summary>
        /// <param name="judge">The instance of Judge given.</param>
        /// <remarks></remarks>
        public JudgeToSend(Judge judge)
        {
            Day = judge.Day;
            Agent = judge.Agent.AgentIdx;
            Target = judge.Target.AgentIdx;
            Result = judge.Result.ToString();
            if (Result == null)
            {
                throw new AIWolfRuntimeException("judge result = null");
            }
        }

        /// <summary>
        /// Returns an instance of Judge class equivalent to this.
        /// </summary>
        /// <returns>An instance of Judge equivalent to this.</returns>
        /// <remarks></remarks>
        public Judge ToJudge()
        {
            return new Judge(Day, Data.Agent.GetAgent(Agent), Data.Agent.GetAgent(Target), (Species)Enum.Parse(typeof(Species), Result));
        }
    }
}
