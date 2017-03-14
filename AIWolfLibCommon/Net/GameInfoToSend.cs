using AIWolf.Common.Data;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AIWolf.Common.Net
{
    /// <summary>
    ///  The game information to be sent to each player.
    /// </summary>
    /// <remarks></remarks>
    [DataContract]
    public class GameInfoToSend
    {
        /// <summary>
        /// Current day.
        /// </summary>
        /// <value>Current day.</value>
        /// <remarks></remarks>
        [DataMember(Name = "day")]
        public int Day { get; set; }

        /// <summary>
        /// The index number of agent who receives this game information.
        /// </summary>
        /// <value>The index number of agent who receives this game information.</value>
        /// <remarks></remarks>
        [DataMember(Name = "agent")]
        public int Agent { get; set; }

        /// <summary>
        /// The result of the inquest.
        /// </summary>
        /// <value>JudgeToSend representing the result of inquest.</value>
        /// <remarks>Medium only.</remarks>
        [DataMember(Name = "mediumResult")]
        public JudgeToSend MediumResult { get; set; }

        /// <summary>
        /// The result of the divination.
        /// </summary>
        /// <value>JudgeToSend representating the result of divination.</value>
        /// <remarks>Seer only.</remarks>
        [DataMember(Name = "divineResult")]
        public JudgeToSend DivineResult { get; set; }

        /// <summary>
        /// The index number of the agent executed last night.
        /// </summary>
        /// <value>The index number of the agent executed last night.</value>
        /// <remarks></remarks>
        [DataMember(Name = "executedAgent")]
        public int ExecutedAgent { get; set; } = -1;

        /// <summary>
        /// The index number of the agent attacked last night.
        /// </summary>
        /// <value>The index number of the agent attacked last night.</value>
        /// <remarks></remarks>
        [DataMember(Name = "attackedAgent")]
        public int AttackedAgent { get; set; } = -1;

        /// <summary>
        /// The index number of the agent guarded last night.
        /// </summary>
        /// <value>The index number of the agent guarded last night.</value>
        /// <remarks></remarks>
        [DataMember(Name = "guardedAgent")]
        public int GuardedAgent { get; set; } = -1;

        /// <summary>
        /// The list of votes for execution.
        /// </summary>
        /// <value>The list of votes for execution.</value>
        /// <remarks>You can see who votes to who.</remarks>
        [DataMember(Name = "voteList")]
        public List<VoteToSend> VoteList { get; set; }

        /// <summary>
        /// The list of votes for attack.
        /// </summary>
        /// <value>The list of votes for attack.</value>
        /// <remarks>Werewolf only.</remarks>
        [DataMember(Name = "attackVoteList")]
        public List<VoteToSend> AttackVoteList { get; set; }

        /// <summary>
        /// The list of today's talks.
        /// </summary>
        /// <value>The list of today's talks.</value>
        /// <remarks></remarks>
        [DataMember(Name = "talkList")]
        public List<TalkToSend> TalkList { get; set; }

        /// <summary>
        /// The list of today's whispers.
        /// </summary>
        /// <value>The list of today's whispers.</value>
        /// <remarks>Werewolf only.</remarks>
        [DataMember(Name = "whisperList")]
        public List<TalkToSend> WhisperList { get; set; }

        /// <summary>
        /// The statuses of all agents.
        /// </summary>
        /// <value>Dictionary storing the statuses of all agents.</value>
        /// <remarks></remarks>
        [DataMember(Name = "statusMap")]
        public Dictionary<int, string> StatusMap { get; set; }

        /// <summary>
        /// The known roles of agents.
        /// </summary>
        /// <value>Dictionary storing the known roles of agents.</value>
        /// <remarks>
        /// If you are human, you know only yourself.
        /// If you are werewolf, you know other werewolves.
        /// </remarks>
        [DataMember(Name = "roleMap")]
        public Dictionary<int, string> RoleMap { get; set; }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <remarks></remarks>
        public GameInfoToSend()
        {
            VoteList = new List<VoteToSend>();
            AttackVoteList = new List<VoteToSend>();
            StatusMap = new Dictionary<int, string>();
            RoleMap = new Dictionary<int, string>();
            TalkList = new List<TalkToSend>();
            WhisperList = new List<TalkToSend>();
        }

        /// <summary>
        /// Returns the instance of GameInfo class equivalent to this.
        /// </summary>
        /// <returns>The instance of GameInfo class equivalent to this.</returns>
        /// <remarks></remarks>
        public GameInfo ToGameInfo()
        {
            GameInfo gi = new GameInfo();
            gi.Day = Day;
            gi.Agent = Data.Agent.GetAgent(Agent);

            if (MediumResult != null)
            {
                gi.MediumResult = MediumResult.ToJudge();
            }
            if (DivineResult != null)
            {
                gi.DivineResult = DivineResult.ToJudge();
            }
            gi.ExecutedAgent = Data.Agent.GetAgent(ExecutedAgent);
            gi.AttackedAgent = Data.Agent.GetAgent(AttackedAgent);
            gi.GuardedAgent = Data.Agent.GetAgent(GuardedAgent);

            gi.VoteList = new List<Vote>();
            foreach (VoteToSend vote in VoteList)
            {
                gi.VoteList.Add(vote.ToVote());
            }
            gi.AttackVoteList = new List<Vote>();
            foreach (VoteToSend vote in AttackVoteList)
            {
                gi.AttackVoteList.Add(vote.ToVote());
            }

            gi.TalkList = new List<Talk>();
            foreach (TalkToSend talk in TalkList)
            {
                gi.TalkList.Add(talk.ToTalk());
            }
            gi.WhisperList = new List<Talk>();
            foreach (TalkToSend whisper in WhisperList)
            {
                gi.WhisperList.Add(whisper.ToTalk());
            }

            gi.StatusMap = new Dictionary<Agent, Status>();
            foreach (int agent in StatusMap.Keys)
            {
                gi.StatusMap.Add(Data.Agent.GetAgent(agent), (Status)Enum.Parse(typeof(Status), StatusMap[agent]));
            }
            gi.RoleMap = new Dictionary<Agent, Role>();
            foreach (int agent in RoleMap.Keys)
            {
                gi.RoleMap.Add(Data.Agent.GetAgent(agent), (Role)Enum.Parse(typeof(Role), RoleMap[agent]));
            }

            return gi;
        }
    }
}
