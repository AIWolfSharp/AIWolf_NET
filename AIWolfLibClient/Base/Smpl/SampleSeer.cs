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
    class SampleSeer : AbstractSeer
    {
        int comingoutDay;

        bool isCameout;

        List<Judge> declaredJudgedAgentList = new List<Judge>();

        bool isSaidAllDivineResult;

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
            SetPlanningVoteAgent();

            isSaidAllDivineResult = false;

            readTalkListNum = 0;
        }

        public override string Talk()
        {
            if (!isCameout && Day >= comingoutDay)
            {
                isCameout = true;
                return TemplateTalkFactory.Comingout(Me, (Role)MyRole);
            }
            else if (isCameout && !isSaidAllDivineResult)
            {
                foreach (Judge judge in MyJudgeList)
                {
                    if (!declaredJudgedAgentList.Contains(judge))
                    {
                        declaredJudgedAgentList.Add(judge);
                        return TemplateTalkFactory.Divined(judge.Target, judge.Result);
                    }
                }
                isSaidAllDivineResult = true;
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

        public override Agent Divine()
        {
            List<Agent> nonInspectedAgentList = new List<Agent>();
            foreach (Agent agent in LatestDayGameInfo.AliveAgentList)
            {
                if (!IsJudgedAgent(agent))
                {
                    nonInspectedAgentList.Add(agent);
                }
            }
            if (nonInspectedAgentList.Count == 0)
            {
                return Me;
            }
            else
            {
                return nonInspectedAgentList.Shuffle().First();
            }
        }

        public override void Finish()
        {
        }

        public override void Update(GameInfo gameInfo)
        {
            base.Update(gameInfo);

            List<Talk> talkList = gameInfo.TalkList;
            for (int i = readTalkListNum; i < talkList.Count; i++)
            {
                Talk talk = talkList[i];
                Utterance utterance = new Utterance(talk.Content);
                switch (utterance.Topic)
                {
                    case Topic.COMINGOUT:
                        agi.ComingoutMap[talk.Agent] = (Role)utterance.Role;
                        if (utterance.Role == MyRole)
                        {
                            SetPlanningVoteAgent();
                        }
                        break;
                }
            }
            readTalkListNum = talkList.Count;
        }

        private void SetPlanningVoteAgent()
        {
            List<Agent> aliveAgentList = LatestDayGameInfo.AliveAgentList;
            aliveAgentList.Remove(Me);

            List<Agent> voteAgentCandidate = new List<Agent>();
            foreach (Agent agent in aliveAgentList)
            {
                if (agi.ComingoutMap.ContainsKey(agent) && agi.ComingoutMap[agent] == MyRole)
                {
                    voteAgentCandidate.Add(agent);
                }
                else
                {
                    foreach (Judge judge in MyJudgeList)
                    {
                        if (judge.Target.Equals(agent) && judge.Result == Species.WEREWOLF)
                        {
                            voteAgentCandidate.Add(agent);
                        }
                    }
                }
            }

            if (voteAgentCandidate.Contains(planningVoteAgent))
            {
                return;
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
