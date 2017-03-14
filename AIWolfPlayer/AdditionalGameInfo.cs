//
// AdditionalGameInfo.cs
//
// Copyright (c) 2017 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using AIWolf.Lib;
using System.Collections.Generic;
using System.Linq;

namespace AIWolf.Player.Sample
{
#if JHELP
    /// <summary>
    /// 追加ゲーム情報
    /// </summary>
#else
    /// <summary>
    /// Additional game information.
    /// </summary>
#endif
    public class AdditionalGameInfo
    {
        /// <summary>
        /// 日付
        /// </summary>
        int day;

        /// <summary>
        /// 自分以外の生存エージェント
        /// </summary>
        public List<Agent> AliveOthers { get; }

        /// <summary>
        /// 追放されたエージェント
        /// </summary>
        public List<Agent> ExecutedAgents { get; } = new List<Agent>();

        /// <summary>
        /// 殺されたエージェント
        /// </summary>
        public List<Agent> KilledAgents { get; } = new List<Agent>();

        /// <summary>
        /// 昨日殺されたエージェント
        /// </summary>
        public Agent LastKilledAgent { get; set; }

        /// <summary>
        /// カミングアウト状況
        /// </summary>
        public Dictionary<Agent, Role> ComingoutMap { get; } = new Dictionary<Agent, Role>();

        /// <summary>
        /// 推定宣言状況
        /// </summary>
        public Dictionary<Agent, List<Talk>> EstimateMap { get; } = new Dictionary<Agent, List<Talk>>();

        /// <summary>
        /// 占い報告リスト
        /// </summary>
        public List<Judge> DivinationList { get; } = new List<Judge>();

        /// <summary>
        /// 霊媒結果リスト
        /// </summary>
        public List<Judge> IdentList { get; } = new List<Judge>();

        int talkListHead; // GameInfo.TalkList読み込みのヘッド

        /// <summary>
        /// Initializes a new instance of AdditionalGameInfo.
        /// </summary>
        /// <param name="gameInfo">Game information.</param>
        public AdditionalGameInfo(GameInfo gameInfo)
        {
            day = -1;
            AliveOthers = gameInfo.AliveAgentList.Where(a => a != gameInfo.Agent).ToList();
        }

        /// <summary>
        /// Update AdditionalGameinfo.
        /// </summary>
        /// <param name="gameInfo">Game information.</param>
        public void Update(GameInfo gameInfo)
        {
            // 1日の最初の呼び出しではその日の初期化などを行う
            if (gameInfo.Day != day)
            {
                day = gameInfo.Day;
                talkListHead = 0;
                AddExecutedAgent(gameInfo.ExecutedAgent); // 前日に追放されたエージェントを登録
                if (gameInfo.LastDeadAgentList.Count != 0)
                {
                    LastKilledAgent = gameInfo.LastDeadAgentList[0]; // 妖狐がいないので長さ最大1
                }
                if (LastKilledAgent != null)
                {
                    if (!KilledAgents.Contains(LastKilledAgent))
                    {
                        KilledAgents.Add(LastKilledAgent);
                    }
                    AliveOthers.Remove(LastKilledAgent);
                }
            }
            // （夜フェーズ限定）追放されたエージェントを登録
            AddExecutedAgent(gameInfo.LatestExecutedAgent);
            // talkListからカミングアウト，占い結果，霊媒結果を抽出
            for (int i = talkListHead; i < gameInfo.TalkList.Count; i++)
            {
                Talk talk = gameInfo.TalkList[i];
                Agent talker = talk.Agent;
                Content content = new Content(talk.Text);
                Agent target = content.Target;
                switch (content.Topic)
                {
                    case Topic.COMINGOUT:
                        ComingoutMap[talker] = content.Role;
                        break;
                    case Topic.DIVINED:
                        DivinationList.Add(new Judge(day, talker, content.Target, content.Result));
                        break;
                    case Topic.IDENTIFIED:
                        IdentList.Add(new Judge(day, talker, target, content.Result));
                        break;
                    case Topic.ESTIMATE:
                        if (target != null)
                        {
                            if (EstimateMap[target] == null)
                            {
                                EstimateMap[target] = new List<Talk>();
                            }
                            EstimateMap[target].Add(talk);
                        }
                        break;
                    default:
                        break;
                }
            }
            talkListHead = gameInfo.TalkList.Count;
        }

        /// <summary>
        /// エージェントを追放されたエージェントのリストに追加する．死亡者・生存者リストも更新する
        /// </summary>
        void AddExecutedAgent(Agent executedAgent)
        {
            if (executedAgent != null)
            {
                if (!ExecutedAgents.Contains(executedAgent))
                {
                    ExecutedAgents.Add(executedAgent);
                }
                AliveOthers.Remove(executedAgent);
            }
        }
    }

}
