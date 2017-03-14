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
    class SamplePossessed : AbstractPossessed
    {
        int comingoutDay;

        bool isCameout;

        List<Judge> declaredFakeJudgedAgentList = new List<Judge>();

        bool isSaidAllFakeResult;

        AdvanceGameInfo agi = new AdvanceGameInfo();

        Agent planningVoteAgent;

        Agent declaredPlanningVoteAgent;

        int readTalkListNum;

        Role fakeRole;

        public List<Judge> MyFakeJudgeList { get; } = new List<Judge>();

        public override void Initialize(GameInfo gameInfo, GameSetting gameSetting)
        {
            base.Initialize(gameInfo, gameSetting);

            List<Role> fakeRoles = gameSetting.RoleNumMap.Keys.ToList();
            List<Role> nonFakeRoleList = new Role[] { Role.BODYGUARD, Role.FREEMASON, Role.POSSESSED, Role.WEREWOLF }.ToList();
            fakeRoles.RemoveAll(role => nonFakeRoleList.Contains(role));

            fakeRole = fakeRoles[new Random().Next(fakeRoles.Count)];

            comingoutDay = new Random().Next(3) + 1;
            if (fakeRole == Role.VILLAGER)
            {
                comingoutDay = 1000;
            }
            isCameout = false;
        }

        public override void DayStart()
        {
            declaredPlanningVoteAgent = null;
            planningVoteAgent = null;
            SetPlanningVoteAgent();

            if (Day >= 1)
            {
                SetFakeResult();
            }
            isSaidAllFakeResult = false;

            readTalkListNum = 0;
        }

        public override string Talk()
        {
            if (!isCameout && Day >= comingoutDay)
            {
                isCameout = true;
                return TemplateTalkFactory.Comingout(Me, fakeRole);
            }
            else if (isCameout && !isSaidAllFakeResult)
            {
                foreach (Judge judge in MyFakeJudgeList)
                {
                    if (!declaredFakeJudgedAgentList.Contains(judge))
                    {
                        if (fakeRole == Role.SEER)
                        {
                            declaredFakeJudgedAgentList.Add(judge);
                            return TemplateTalkFactory.Divined(judge.Target, judge.Result);
                        }
                        else if (fakeRole == Role.MEDIUM)
                        {
                            declaredFakeJudgedAgentList.Add(judge);
                            return TemplateTalkFactory.Inquested(judge.Target, judge.Result);
                        }
                    }
                }
                isSaidAllFakeResult = true;
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

        public void SetPlanningVoteAgent()
        {
            List<Agent> aliveAgentList = LatestDayGameInfo.AliveAgentList;
            aliveAgentList.Remove(Me);

            if (fakeRole == Role.VILLAGER)
            {
                if (aliveAgentList.Contains(planningVoteAgent))
                {
                    return;
                }
                else
                {
                    planningVoteAgent = aliveAgentList.Shuffle().First();
                }
            }

            List<Agent> fakeHumanList = new List<Agent>();
            List<Agent> voteAgentCandidate = new List<Agent>();
            foreach (Agent a in aliveAgentList)
            {
                if (agi.ComingoutMap.ContainsKey(a) && agi.ComingoutMap[a] == fakeRole)
                {
                    voteAgentCandidate.Add(a);
                }
            }
            foreach (Judge judge in MyFakeJudgeList)
            {
                if (judge.Result == Species.HUMAN)
                {
                    fakeHumanList.Add(judge.Target);
                }
                else
                {
                    voteAgentCandidate.Add(judge.Target);
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
                List<Agent> aliveAgentExceptHumanList = LatestDayGameInfo.AliveAgentList;
                aliveAgentExceptHumanList.RemoveAll(a => fakeHumanList.Contains(a));

                if (aliveAgentExceptHumanList.Count > 0)
                {
                    planningVoteAgent = aliveAgentExceptHumanList.Shuffle().First();
                }
                else
                {
                    planningVoteAgent = aliveAgentList.Shuffle().First();
                }
            }
            return;
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
                        if (utterance.Role == fakeRole)
                        {
                            SetPlanningVoteAgent();
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
                SetPlanningVoteAgent();
            }
        }

        public void SetFakeResult()
        {
            Agent fakeGiftTarget = null;

            Species? fakeResult = null;

            if (fakeRole == Role.SEER)
            {
                List<Agent> fakeGiftTargetCandidateList = new List<Agent>();
                List<Agent> aliveAgentList = LatestDayGameInfo.AliveAgentList;
                aliveAgentList.Remove(Me);

                foreach (Agent agent in aliveAgentList)
                {
                    Role? comingoutRole = agi.ComingoutMap.ContainsKey(agent) ? agi.ComingoutMap[agent] : null;
                    if (!IsJudgedAgent(agent) && fakeRole != comingoutRole)
                    {
                        fakeGiftTargetCandidateList.Add(agent);
                    }
                }

                if (fakeGiftTargetCandidateList.Count > 0)
                {
                    fakeGiftTarget = fakeGiftTargetCandidateList.Shuffle().First();
                }
                else
                {
                    aliveAgentList.RemoveAll(a => fakeGiftTargetCandidateList.Contains(a));
                    fakeGiftTarget = aliveAgentList.Shuffle().First();
                }

                if (new Random().NextDouble() < 0.3)
                {
                    fakeResult = Species.WEREWOLF;
                }
                else
                {
                    fakeResult = Species.HUMAN;
                }
            }
            else if (fakeRole == Role.MEDIUM)
            {
                fakeGiftTarget = LatestDayGameInfo.ExecutedAgent;
                if (new Random().NextDouble() < 0.3)
                {
                    fakeResult = Species.WEREWOLF;
                }
                else
                {
                    fakeResult = Species.HUMAN;
                }
            }
            else
            {
                return;
            }

            if (fakeGiftTarget != null)
            {
                MyFakeJudgeList.Add(new Judge(Day, Me, fakeGiftTarget, (Species)fakeResult));
            }
        }

        public bool IsJudgedAgent(Agent agent)
        {
            foreach (Judge judge in MyFakeJudgeList)
            {
                if (judge.Agent == agent)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
