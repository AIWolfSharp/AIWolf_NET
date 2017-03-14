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
    class SampleMedium : AbstractMedium
    {
        int comingoutDay;

        bool isCameout;

        List<Judge> declaredJudgedAgentList = new List<Judge>();

        bool isSaidAllInquestResult;

        AdvanceGameInfo agi = new AdvanceGameInfo();

        Agent planningVoteAgent;

        Agent declaredPlanningVoteAgent;

        int readTalkListNum;

        public override void Initialize(GameInfo gameInfo, GameSetting gameSetting)
        {
            base.Initialize(gameInfo, gameSetting);

            comingoutDay = new Random().Next(3) + 1;
            isCameout = false;
        }


        public override void DayStart()
        {
            base.DayStart();

            declaredPlanningVoteAgent = null;
            planningVoteAgent = null;
            setPlanningVoteAgent();
            isSaidAllInquestResult = false;
            readTalkListNum = 0;
        }

        public override string Talk()
        {
            if (!isCameout && Day >= comingoutDay)
            {
                isCameout = true;
                return TemplateTalkFactory.Comingout(Me, (Role)MyRole);
            }
            else if (isCameout && !isSaidAllInquestResult)
            {
                foreach (Judge judge in MyJudgeList)
                {
                    if (!declaredJudgedAgentList.Contains(judge))
                    {
                        declaredJudgedAgentList.Add(judge);
                        return TemplateTalkFactory.Inquested(judge.Target, judge.Result);
                    }
                }
                isSaidAllInquestResult = true;
            }

            if (declaredPlanningVoteAgent != planningVoteAgent)
            {
                declaredPlanningVoteAgent = planningVoteAgent;
                return TemplateTalkFactory.Vote(planningVoteAgent);
            }
            else
            {
                return Common.Data.Talk.OVER;
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
                        if (utterance.Role == MyRole)
                        {
                            setPlanningVoteAgent();
                        }
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
                setPlanningVoteAgent();
            }
        }

        public void setPlanningVoteAgent()
        {
            List<Agent> voteAgentCandidate = new List<Agent>();

            List<Agent> aliveAgentList = LatestDayGameInfo.AliveAgentList;
            aliveAgentList.Remove(Me);

            foreach (Agent agent in aliveAgentList)
            {
                if (agi.ComingoutMap.ContainsKey(agent) && agi.ComingoutMap[agent] == Role.MEDIUM)
                {
                    voteAgentCandidate.Add(agent);
                }
            }

            foreach (Judge myJudge in MyJudgeList)
            {
                foreach (Judge otherJudge in agi.InspectJudgeList)
                {
                    if (!aliveAgentList.Contains(otherJudge.Agent))
                    {
                        continue;
                    }
                    if (myJudge.Target.Equals(otherJudge.Target))
                    {
                        if (myJudge.Result != otherJudge.Result)
                        {
                            voteAgentCandidate.Add(otherJudge.Agent);
                        }
                    }
                }
            }

            if (planningVoteAgent != null && voteAgentCandidate.Contains(planningVoteAgent))
            {
                return;
            }
            else
            {
                if (voteAgentCandidate.Count > 0)
                {
                    planningVoteAgent = voteAgentCandidate.Shuffle().First();
                }
                else
                {
                    List<Agent> subVoteAgentCandidate = new List<Agent>();
                    foreach (Judge judge in agi.InspectJudgeList)
                    {
                        if (aliveAgentList.Contains(judge.Target) && judge.Result == Species.WEREWOLF)
                        {
                            subVoteAgentCandidate.Add(judge.Target);
                        }
                    }

                    if (subVoteAgentCandidate.Count > 0)
                    {
                        planningVoteAgent = subVoteAgentCandidate.Shuffle().First();
                    }
                    else
                    {
                        planningVoteAgent = aliveAgentList.Shuffle().First();
                    }
                }
            }
        }
    }
}
