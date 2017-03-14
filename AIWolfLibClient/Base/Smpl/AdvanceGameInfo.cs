using AIWolf.Common.Data;
using System.Collections.Generic;

namespace AIWolf.Client.Base.Smpl
{
    /// <summary>
    /// Additional game information used in sample players.
    /// </summary>
    /// <remarks></remarks>
    class AdvanceGameInfo
    {
        /// <summary>
        /// Initializes a new instance of AdvanceGameInfo class.
        /// </summary>
        /// <remarks></remarks>
        public AdvanceGameInfo() { }

        /// <summary>
        /// The list of divination made from talks.
        /// </summary>
        /// <value>The list of divination made from talks.</value>
        /// <remarks></remarks>
        public List<Judge> InspectJudgeList { get; set; } = new List<Judge>();

        /// <summary>
        /// The list of inquests made from talks.
        /// </summary>
        /// <value>The list of inquests made from talks.</value>
        /// <remarks></remarks>
        public List<Judge> MediumJudgeList { get; set; } = new List<Judge>();

        /// <summary>
        /// Dictionary which provides the mapping from the agents to the roles confessed by comingouts.
        /// </summary>
        /// <value>Dictionary which provides the mapping from the agents to the roles confessed by comingouts.</value>
        /// <remarks></remarks>
        public Dictionary<Agent, Role?> ComingoutMap { get; set; } = new Dictionary<Agent, Role?>();

        /// <summary>
        /// Adds the agent who confessed his role.
        /// </summary>
        /// <param name="agent">The agent who confessed his role.</param>
        /// <param name="role">The confessed role.</param>
        /// <remarks></remarks>
        public void PutComingoutMap(Agent agent, Role role)
        {
            ComingoutMap[agent] = role;
        }

        /// <summary>
        /// Add a judge to the list of divination.
        /// </summary>
        /// <param name="judge">The judge to be added .</param>
        /// <remarks></remarks>
        public void AddInspectJudgeList(Judge judge)
        {
            InspectJudgeList.Add(judge);
        }

        /// <summary>
        /// Add a judge to the list of inquests.
        /// </summary>
        /// <param name="judge">The judge to be added .</param>
        /// <remarks></remarks>
        public void AddMediumJudgeList(Judge judge)
        {
            MediumJudgeList.Add(judge);
        }
    }
}