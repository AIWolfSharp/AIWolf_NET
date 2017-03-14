using AIWolf.Client.Base.Player;
using AIWolf.Client.Lib;
using AIWolf.Common.Data;
using AIWolf.Common.Net;
using AIWolf.Common.Util;
using System.Collections.Generic;
using System.Linq;

namespace AIWolf.Client.Base.Smpl
{
    class SampleVillager : AbstractVillager
    {
        AdvanceGameInfo agi = new AdvanceGameInfo();

        Agent planningVoteAgent;

        Agent declaredPlanningVoteAgent;

        int readTalkListNum;

        public override void DayStart()
        {
            declaredPlanningVoteAgent = null;
            planningVoteAgent = null;
            SetPlanningVoteAgent();
            readTalkListNum = 0;
        }

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

        public override Agent Vote()
        {
            return planningVoteAgent;
        }

        public override void Finish()
        {
        }

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
    }
}
