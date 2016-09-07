﻿//
// GameInfo.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace AIWolf.Lib
{
    /// <summary>
    /// Game information.
    /// </summary>
    [DataContract]
    public class GameInfo
    {
        /// <summary>
        /// Current day.
        /// </summary>
        [DataMember(Name = "day")]
        public int Day { get; }

        /// <summary>
        /// The agent who receives this GameInfo.
        /// </summary>
        public Agent Agent { get; }

        /// <summary>
        /// The index number of agent who receives this game information.
        /// </summary>
        [DataMember(Name = "agent")]
        public int _Agent { get; }

        /// <summary>
        /// The role of player who receives this GameInfo.
        /// </summary>
        public Role Role
        {
            get
            {
                return _Agent != 0 && RoleMap.ContainsKey(Agent) ? RoleMap[Agent] : Role.UNC;
            }
        }

        /// <summary>
        /// The result of the inquest.
        /// </summary>
        /// <remarks>Medium only.</remarks>
        [DataMember(Name = "mediumResult")]
        public Judge MediumResult { get; }

        /// <summary>
        /// The result of the dvination.
        /// </summary>
        /// <remarks>Seer only.</remarks>
        [DataMember(Name = "divineResult")]
        public Judge DivineResult { get; }

        /// <summary>
        /// The agent executed last night.
        /// </summary>
        public Agent ExecutedAgent { get; }

        /// <summary>
        /// The index number of the agent executed last night.
        /// </summary>
        [DataMember(Name = "executedAgent")]
        public int _ExecutedAgent { get; }

        /// <summary>
        /// The agent attacked last night.
        /// </summary>
        public Agent AttackedAgent { get; }

        /// <summary>
        /// The index number of the agent attacked last night.
        /// </summary>
        [DataMember(Name = "attackedAgent")]
        public int _AttackedAgent { get; }

        /// <summary>
        /// The agent guarded last night.
        /// </summary>
        public Agent GuardedAgent { get; }

        /// <summary>
        /// The index number of the agent guarded last night.
        /// </summary>
        [DataMember(Name = "guardedAgent")]
        public int _GuardedAgent { get; }

        /// <summary>
        /// The list of votes for execution.
        /// </summary>
        /// <remarks>You can see who votes to who.</remarks>
        [DataMember(Name = "voteList")]
        public List<Vote> VoteList { get; }

        /// <summary>
        /// The list of votes for attack.
        /// </summary>
        /// <remarks>Werewolf only.</remarks>
        [DataMember(Name = "attackVoteList")]
        public List<Vote> AttackVoteList { get; }

        /// <summary>
        /// The list of today's talks.
        /// </summary>
        [DataMember(Name = "talkList")]
        public List<Talk> TalkList { get; }

        /// <summary>
        /// The list of today's whispers.
        /// </summary>
        /// <remarks>Werewolf only.</remarks>
        [DataMember(Name = "whisperList")]
        public List<Talk> WhisperList { get; }

        /// <summary>
        /// The statuses of all agents.
        /// </summary>
        public Dictionary<Agent, Status> StatusMap { get; }

        /// <summary>
        /// The statuses of all agents.
        /// </summary>
        [DataMember(Name = "statusMap")]
        public Dictionary<int, string> _StatusMap { get; }

        /// <summary>
        /// The known roles of agents.
        /// </summary>
        /// <remarks>
        /// If you are human, you know only yourself.
        /// If you are werewolf, you know other werewolves.
        /// </remarks>
        public Dictionary<Agent, Role> RoleMap { get; }

        /// <summary>
        /// The known roles of agents.
        /// </summary>
        /// <remarks>
        /// If you are human, you know only yourself.
        /// If you are werewolf, you know other werewolves.
        /// </remarks>
        [DataMember(Name = "roleMap")]
        public Dictionary<int, string> _RoleMap { get; }

        /// <summary>
        /// The list of agents.
        /// </summary>
        public List<Agent> AgentList
        {
            get
            {
                return StatusMap.Keys.ToList();
            }
        }

        /// <summary>
        /// The list of alive agents.
        /// </summary>
        /// <remarks>If all agents are dead, this returns an empty list, not null.</remarks>
        public List<Agent> AliveAgentList
        {
            get
            {
                return AgentList.Where(a => StatusMap[a] == Status.ALIVE).ToList();
            }
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        [JsonConstructor]
        public GameInfo(int day, int agent, Judge mediumResult, Judge divineResult, int executedAgent, int attackedAgent, int guardedAgent,
            List<Vote> voteList, List<Vote> attackVoteList, List<Talk> talkList, List<Talk> whisperList,
            Dictionary<int, string> statusMap, Dictionary<int, string> roleMap)
        {
            Day = day;
            if (Day < 0)
            {
                Error.RuntimeError(GetType() + "(): Invalid day " + Day + ".", "Force it to be 0.");
                Day = 0;
            }

            Agent = Agent.GetAgent(agent);
            if (Agent == null)
            {
                Error.RuntimeError(GetType() + "(): Agent must not be null.", "Force it to be Agent[00].");
                Agent = Agent.GetAgent(0);
            }
            _Agent = Agent.AgentIdx;

            MediumResult = mediumResult;
            DivineResult = divineResult;

            ExecutedAgent = Agent.GetAgent(executedAgent);
            _ExecutedAgent = ExecutedAgent == null ? -1 : ExecutedAgent.AgentIdx;

            AttackedAgent = Agent.GetAgent(attackedAgent);
            _AttackedAgent = AttackedAgent == null ? -1 : AttackedAgent.AgentIdx;

            GuardedAgent = Agent.GetAgent(guardedAgent);
            _GuardedAgent = GuardedAgent == null ? -1 : GuardedAgent.AgentIdx;

            VoteList = voteList == null ? new List<Vote>() : voteList;
            AttackVoteList = attackVoteList == null ? new List<Vote>() : attackVoteList;
            TalkList = talkList == null ? new List<Talk>() : talkList;
            WhisperList = whisperList == null ? new List<Talk>() : whisperList;

            StatusMap = new Dictionary<Agent, Status>();
            if (statusMap != null)
            {
                foreach (var p in statusMap)
                {
                    Status status;
                    if (!Enum.TryParse(p.Value, out status))
                    {
                        Error.RuntimeError(GetType() + "(): Invalid status string " + p.Value + ".", "Force it to be Status.ALIVE.");
                        status = Status.ALIVE;
                    }
                    StatusMap[Agent.GetAgent(p.Key)] = status;
                }
            }
            _StatusMap = StatusMap.ToDictionary(p => p.Key.AgentIdx, p => p.Value.ToString());

            RoleMap = new Dictionary<Agent, Role>();
            if (roleMap != null)
            {
                foreach (var p in roleMap)
                {
                    Role role;
                    if (!Enum.TryParse(p.Value, out role) || role == Role.UNC)
                    {
                        Error.RuntimeError(GetType() + "(): Invalid role string " + p.Value + ".", "It is removed from role map.");
                    }
                    else
                    {
                        RoleMap[Agent.GetAgent(p.Key)] = role;
                    }
                }
            }
            _RoleMap = RoleMap.Where(m => m.Value != Role.UNC).ToDictionary(m => m.Key.AgentIdx, m => m.Value.ToString());
        }
    }
}
