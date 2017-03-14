using AIWolf.Common.Data;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AIWolf.Common.Net
{
    /// <summary>
    /// Game information.
    /// </summary>
    /// <remarks></remarks>
    [DataContract]
    public class GameInfo
    {
        /// <summary>
        /// Current day.
        /// </summary>
        /// <value>Current day.</value>
        /// <remarks></remarks>
        [DataMember(Name = "day")]
        public int Day { get; set; }

        /// <summary>
        /// The agent who receives this GameInfo.
        /// </summary>
        /// <value>The agent who receives this GameInfo.</value>
        /// <remarks></remarks>
        [DataMember(Name = "agent")]
        public Agent Agent { get; set; }

        /// <summary>
        /// The role of player who receives this GameInfo.
        /// </summary>
        /// <value>The role of player who receives this GameInfo.</value>
        /// <remarks></remarks>
        [DataMember(Name = "role")]
        public Role? Role
        {
            get
            {
                return Agent != null && RoleMap.ContainsKey(Agent) ? (Role?)RoleMap[Agent] : null;
            }
        }

        /// <summary>
        /// The result of the inquest.
        /// </summary>
        /// <value>The result of the inquest.</value>
        /// <remarks>Medium only.</remarks>
        [DataMember(Name = "mediumResult")]
        public Judge MediumResult { get; set; }

        /// <summary>
        /// The result of the dvination.
        /// </summary>
        /// <value>The result of the dvination.</value>
        /// <remarks>Seer only.</remarks>
        [DataMember(Name = "divineResult")]
        public Judge DivineResult { get; set; }

        /// <summary>
        /// The agent executed last night.
        /// </summary>
        /// <value>The agent executed last night.</value>
        /// <remarks></remarks>
        [DataMember(Name = "executedAgent")]
        public Agent ExecutedAgent { get; set; }

        /// <summary>
        /// The agent attacked last night.
        /// </summary>
        /// <value>The agent attacked last night.</value>
        /// <remarks></remarks>
        [DataMember(Name = "attackedAgent")]
        public Agent AttackedAgent { get; set; }

        /// <summary>
        /// The agent guarded last night.
        /// </summary>
        /// <value>The agent guarded last night.</value>
        /// <remarks></remarks>
        [DataMember(Name = "guardedAgent")]
        public Agent GuardedAgent { get; set; }

        /// <summary>
        /// The list of votes for execution.
        /// </summary>
        /// <value>The list of votes for execution.</value>
        /// <remarks>You can see who votes to who.</remarks>
        [DataMember(Name = "voteList")]
        public List<Vote> VoteList { get; set; }

        /// <summary>
        /// The list of votes for attack.
        /// </summary>
        /// <value>The list of votes for attack.</value>
        /// <remarks>Werewolf only.</remarks>
        [DataMember(Name = "attackVoteList")]
        public List<Vote> AttackVoteList { get; set; }

        /// <summary>
        /// The list of today's talks.
        /// </summary>
        /// <value>The list of today's talks.</value>
        /// <remarks></remarks>
        [DataMember(Name = "talkList")]
        public List<Talk> TalkList { get; set; }

        /// <summary>
        /// The list of today's whispers.
        /// </summary>
        /// <value>The list of today's whispers.</value>
        /// <remarks>Werewolf only.</remarks>
        [DataMember(Name = "whisperList")]
        public List<Talk> WhisperList { get; set; }

        /// <summary>
        /// The statuses of all agents.
        /// </summary>
        /// <value>The dictionary storing the statuses of all agents.</value>
        /// <remarks></remarks>
        [DataMember(Name = "statusMap")]
        public Dictionary<Agent, Status> StatusMap { get; set; }

        /// <summary>
        /// The known roles of agents.
        /// </summary>
        /// <value>The dictionary storing the known roles of agents.</value>
        /// <remarks>
        /// If you are human, you know only yourself.
        /// If you are werewolf, you know other werewolves.
        /// </remarks>
        [DataMember(Name = "roleMap")]
        public Dictionary<Agent, Role> RoleMap { get; set; }

        /// <summary>
        /// The list of agents.
        /// </summary>
        /// <value>The list of agents.</value>
        /// <remarks></remarks>
        [DataMember(Name = "agentList")]
        public List<Agent> AgentList
        {
            get
            {
                return new List<Agent>(StatusMap.Keys);
            }
        }

        /// <summary>
        /// The list of alive agents.
        /// </summary>
        /// <value>The list of alive agents.</value>
        /// <remarks></remarks>
        [DataMember(Name = "aliveAgentList")]
        public List<Agent> AliveAgentList
        {
            get
            {
                List<Agent> aliveAgentList = new List<Agent>();
                if (AgentList != null)
                {
                    foreach (Agent target in AgentList)
                    {
                        if (StatusMap[target] == Status.ALIVE)
                        {
                            aliveAgentList.Add(target);
                        }
                    }
                }
                return aliveAgentList;
            }
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <remarks></remarks>
        public GameInfo()
        {
            VoteList = new List<Vote>();
            AttackVoteList = new List<Vote>();
            TalkList = new List<Talk>();
            WhisperList = new List<Talk>();
            StatusMap = new Dictionary<Agent, Status>();
            RoleMap = new Dictionary<Agent, Role>();
        }
    }
}
