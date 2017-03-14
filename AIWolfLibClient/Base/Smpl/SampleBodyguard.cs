using AIWolf.Client.Base.Player;
using AIWolf.Client.Lib;
using AIWolf.Common.Data;
using AIWolf.Common.Net;
using AIWolf.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AIWolf.Client.Base.Smpl
{
    /// <summary>
    /// Sample class of bodyguard player.
    /// </summary>
    /// <remarks></remarks>
    class SampleBodyguard : AbstractBodyguard
    {
        AdvanceGameInfo agi = new AdvanceGameInfo();

        // The agent I will vote today.
        Agent planningVoteAgent;

        // The last agent whom I declare that I will vote.
        Agent declaredPlanningVoteAgent;

        int readTalkListNum;

        /// <summary>
        /// Called when the day started.
        /// </summary>
        /// <remarks></remarks>
        public override void DayStart()
        {
            declaredPlanningVoteAgent = null;
            planningVoteAgent = null;
            SetPlanningVoteAgent();
            readTalkListNum = 0;
        }

        /// <summary>
        /// Returns this player's talk.
        /// </summary>
        /// <returns>The string representing this player's talk.</returns>
        /// <remarks>
        /// The returned string must be written in aiwolf protocol.
        /// Null means SKIP.
        /// </remarks>
        public override string Talk()
        {
            if (declaredPlanningVoteAgent != planningVoteAgent)
            {
                declaredPlanningVoteAgent = planningVoteAgent;
                return TemplateTalkFactory.Vote(planningVoteAgent);
            }
            else
            {
                return TemplateTalkFactory.Over();
            }
        }

        /// <summary>
        /// Returns the agent this player wants to execute.
        /// </summary>
        /// <returns>The agent this player wants to execute.</returns>
        /// <remarks></remarks>
        public override Agent Vote()
        {
            return planningVoteAgent;
        }

        /// <summary>
        /// Returns the agent this bodyguard wants to guard.
        /// </summary>
        /// <returns>The agent this bodyguard wants to guard.</returns>
        /// <remarks></remarks>
        public override Agent Guard()
        {
            List<Agent> guardAgentCandidate = new List<Agent>();
            List<Agent> aliveAgentList = LatestDayGameInfo.AliveAgentList;
            aliveAgentList.Remove(Me);

            foreach (Agent agent in aliveAgentList)
            {
                if (agi.ComingoutMap.ContainsKey(agent))
                {
                    List<Role?> guardRoleList = new List<Role?>();
                    guardRoleList.Add(Role.SEER);
                    guardRoleList.Add(Role.MEDIUM);
                    if (guardRoleList.Contains(agi.ComingoutMap[agent]))
                    {
                        guardAgentCandidate.Add(agent);
                    }
                }
            }

            Agent guardAgent;

            if (guardAgentCandidate.Count > 0 && new Random().NextDouble() < 0.8)
            {
                guardAgent = guardAgentCandidate.Shuffle().First();
            }
            else
            {
                guardAgent = aliveAgentList.Shuffle().First();
            }
            return guardAgent;
        }

        /// <summary>
        /// Called when the game finishes.
        /// </summary>
        /// <remarks>Before this method is called, the game information is updated with all information.</remarks>
        public override void Finish()
        {
        }

        /// <summary>
        /// Called when the game information is updated.
        /// </summary>
        /// <param name="gameInfo">The current information of this game.</param>
        /// <remarks></remarks>
        public override void Update(GameInfo gameInfo)
        {
            base.Update(gameInfo);

            List<Talk> talkList = gameInfo.TalkList;
            bool existInspectResult = false;

            for (int i = readTalkListNum; i < talkList.Count; i++)
            {
                Talk talk = talkList[i];
                Utterance utterance = new Utterance(talk.Content);
                switch (utterance.Topic)
                {
                    case Topic.COMINGOUT:
                        agi.ComingoutMap[talk.Agent] = utterance.Role;
                        break;
                    case Topic.DIVINED:
                        Agent seerAgent = talk.Agent;
                        Agent inspectedAgent = utterance.Target;
                        Species inspectResult = (Species)utterance.Result;
                        Judge judge = new Judge(Day, seerAgent, inspectedAgent, inspectResult);
                        agi.AddInspectJudgeList(judge);
                        existInspectResult = true;
                        break;
                }
            }
            readTalkListNum = talkList.Count;

            if (existInspectResult)
            {
                SetPlanningVoteAgent();
            }
        }

        /// <summary>
        /// Decides agent to be voted for execution.
        /// </summary>
        /// <remarks></remarks>
        public void SetPlanningVoteAgent()
        {
            if (planningVoteAgent != null)
            {
                foreach (Judge judge in agi.InspectJudgeList)
                {
                    if (judge.Target.Equals(planningVoteAgent))
                    {
                        return;
                    }
                }
            }

            List<Agent> voteAgentCandidate = new List<Agent>();
            List<Agent> aliveAgentList = LatestDayGameInfo.AliveAgentList;
            aliveAgentList.Remove(Me);
            foreach (Judge judge in agi.InspectJudgeList)
            {
                if (aliveAgentList.Contains(judge.Target) && judge.Result == Species.WEREWOLF)
                {
                    voteAgentCandidate.Add(judge.Target);
                }
            }

            if (voteAgentCandidate.Count > 0)
            {
                planningVoteAgent = voteAgentCandidate.Shuffle().First();
            }
            else
            {
                planningVoteAgent = aliveAgentList.Shuffle().First();
            }
            return;
        }

        /// <summary>
        /// Initializes a new instance of SampleBodyguard class.
        /// </summary>
        /// <remarks></remarks>
        public SampleBodyguard() { }
    }
}
