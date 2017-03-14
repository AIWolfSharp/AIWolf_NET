//
// Seer.cs
//
// Copyright (c) 2017 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using AIWolf.Lib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AIWolf.Player.Sample
{
#if JHELP
    /// <summary>
    /// 占い師プレイヤーの見本
    /// </summary>
#else
    /// <summary>
    /// Sample seer player.
    /// </summary>
#endif
    public sealed class Seer : BasePlayer
    {
        // カミングアウトする日
        int comingoutDay;
        // カミングアウト済みか否か
        bool isCameout;
        // 占い結果を入れる待ち行列
        Queue<Judge> divinationQueue = new Queue<Judge>();
        // 占い結果マップ
        Dictionary<Agent, Species> myDivinationMap = new Dictionary<Agent, Species>();
        // 人間リスト
        List<Agent> whiteList = new List<Agent>();
        // 人狼リスト
        List<Agent> blackList = new List<Agent>();
        // グレイリスト
        List<Agent> grayList;
        // 人狼候補リスト
        List<Agent> semiWolves = new List<Agent>();
        // 裏切り者リスト
        List<Agent> possessedList = new List<Agent>();

        /// <summary>
        /// 投票先候補を選ぶ
        /// </summary>
        /// <returns>投票先候補のエージェント</returns>
        protected override void ChooseVoteCandidate()
        {
            // 生存人狼がいれば当然投票
            var aliveWolves = blackList.Where(a => Alive(a));
            if (aliveWolves.Count() > 0)
            {
                // 既定の投票先が生存人狼でない場合投票先を変える
                if (!aliveWolves.Contains(voteCandidate))
                {
                    voteCandidate = aliveWolves.Shuffle().First();
                    if (CanTalk)
                    {
                        TalkQueue.Enqueue(new Content(new RequestContentBuilder(null, new Content(new VoteContentBuilder(voteCandidate)))));
                    }
                }
                return;
            }
            // 確定人狼がいない場合は推測する
            // 偽占い師
            var fakeSeers = AliveOthers.Where(a => GetCoRole(a) == Role.SEER);
            // 偽霊媒師
            var fakeMediums = IdentList.Where(j => myDivinationMap.ContainsKey(j.Target)
                && j.Result != myDivinationMap[j.Target]).Select(j => j.Agent);
            var candidates = fakeSeers.Concat(fakeMediums).Where(a => Alive(a)).Distinct();
            // 人狼候補なのに人間⇒裏切り者
            foreach (Agent possessed in candidates.Where(a => whiteList.Contains(a)))
            {
                if (!possessedList.Contains(possessed))
                {
                    TalkQueue.Enqueue(new Content(new EstimateContentBuilder(possessed, Role.POSSESSED)));
                    possessedList.Add(possessed);
                }
            }
            semiWolves = candidates.Where(a => !whiteList.Contains(a)).ToList();
            if (semiWolves.Count() > 0)
            {
                if (!semiWolves.Contains(voteCandidate))
                {
                    voteCandidate = semiWolves.Shuffle().First();
                    // 以前の投票先から変わる場合，新たに推測発言をする
                    if (CanTalk)
                    {
                        TalkQueue.Enqueue(new Content(new EstimateContentBuilder(voteCandidate, Role.WEREWOLF)));
                    }
                }
            }
            else
            {
                // 人狼候補がいない場合はグレイからランダム
                if (grayList.Count != 0)
                {
                    if (!grayList.Contains(voteCandidate))
                    {
                        voteCandidate = grayList.Shuffle().First();
                    }
                }
                // グレイがいない場合ランダム
                else
                {
                    if (!AliveOthers.Contains(voteCandidate))
                    {
                        voteCandidate = AliveOthers.Shuffle().First();
                    }
                }
            }
        }

#if JHELP
        /// <summary>
        /// ゲーム開始時に呼ばれる
        /// </summary>
        /// <param name="gameInfo">最新のゲーム情報</param>
        /// <param name="gameSetting">ゲーム設定</param>
#else
        /// <summary>
        /// Called when the game started.
        /// </summary>
        /// <param name="gameInfo">The current information of this game.</param>
        /// <param name="gameSetting">The setting of this game.</param>
#endif
        public override void Initialize(GameInfo gameInfo, GameSetting gameSetting)
        {
            base.Initialize(gameInfo, gameSetting);
            comingoutDay = new int[] { 1, 2, 3 }.Shuffle().First();
            isCameout = false;
            divinationQueue.Clear();
            myDivinationMap.Clear();
            whiteList.Clear();
            blackList.Clear();
            grayList = new List<Agent>(AliveOthers);
            semiWolves.Clear();
            possessedList.Clear();
        }

#if JHELP
        /// <summary>
        /// 新しい日が始まるときに呼ばれる
        /// </summary>
#else
        /// <summary>
        /// Called when the day started.
        /// </summary>
#endif
        public override void DayStart()
        {
            base.DayStart();
            // 占い結果を待ち行列に入れる
            var divination = CurrentGameInfo.DivineResult;
            if (divination != null)
            {
                divinationQueue.Enqueue(divination);
                grayList.Remove(divination.Target);
                if (divination.Result == Species.HUMAN)
                {
                    whiteList.Add(divination.Target);
                }
                else
                {
                    blackList.Add(divination.Target);
                }
                myDivinationMap[divination.Target] = divination.Result;
            }
        }

#if JHELP
        /// <summary>
        /// プレイヤーの発言を返す
        /// </summary>
        /// <returns>発言の文字列</returns>
        /// <remarks>
        /// nullはSkipを意味する
        /// </remarks>
#else
        /// <summary>
        /// Returns this player's talk.
        /// </summary>
        /// <returns>The string representing this player's talk.</returns>
        /// <remarks>
        /// Null means Skip.
        /// </remarks>
#endif
        public override string Talk()
        {
            // カミングアウトする日になったら，あるいは占い結果が人狼だったら
            // あるいは占い師カミングアウトが出たらカミングアウト
            if (!isCameout && (Day >= comingoutDay
                    || (divinationQueue.Count > 0 && divinationQueue.Peek().Result == Species.WEREWOLF)
                    || IsCo(Role.SEER)))
            {
                TalkQueue.Enqueue(new Content(new ComingoutContentBuilder(Me, Role.SEER)));
                isCameout = true;
            }
            // カミングアウトしたらこれまでの占い結果をすべて公開
            if (isCameout)
            {
                while (divinationQueue.Count > 0)
                {
                    Judge divination = divinationQueue.Dequeue();
                    TalkQueue.Enqueue(new Content(new DivinedResultContentBuilder(divination.Target, divination.Result)));
                }
            }
            return base.Talk();
        }

#if JHELP
        /// <summary>
        /// この占い師が占いたいエージェントを返す
        /// </summary>
        /// <returns>この占い師が占いたいエージェント</returns>
        /// <remarks>nullは占いなしを意味する</remarks>
#else
        /// <summary>
        /// Returns the agent this seer wants to divine.
        /// </summary>
        /// <returns>The agent this seer wants to divine.</returns>
        /// <remarks>No divination in case of null.</remarks>
#endif
        public override Agent Divine()
        {
            // 人狼候補がいればそれらからランダムに占う
            if (semiWolves.Count > 0)
            {
                return semiWolves.Shuffle().First();
            }
            // 人狼候補がいない場合，まだ占っていない生存者からランダムに占う
            List<Agent> candidates = AliveOthers.Where(a => !myDivinationMap.ContainsKey(a)).ToList();
            if (candidates.Count == 0)
            {
                return null;
            }
            return candidates.Shuffle().First();
        }

#if JHELP
        /// <summary>
        /// プレイヤーの囁きを返す
        /// </summary>
        /// <returns>囁きの文字列</returns>
        /// <remarks>
        /// nullはSkipを意味する
        /// </remarks>
#else
        /// <summary>
        /// Returns this werewolf's whisper.
        /// </summary>
        /// <returns>The string representing this werewolf's whisper.</returns>
        /// <remarks>
        /// Null means Skip.
        /// </remarks>
#endif
        public override string Whisper()
        {
            throw new NotImplementedException();
        }

#if JHELP
        /// <summary>
        /// この人狼が襲撃したいエージェントを返す
        /// </summary>
        /// <returns>この人狼が襲撃したいエージェント</returns>
        /// <remarks>nullは襲撃なしを意味する</remarks>
#else
        /// <summary>
        /// Returns the agent this werewolf wants to attack.
        /// </summary>
        /// <returns>The agent this werewolf wants to attack.</returns>
        /// <remarks>No attack in case of null.</remarks>
#endif
        public override Agent Attack()
        {
            throw new NotImplementedException();
        }

#if JHELP
        /// <summary>
        /// この狩人が護衛したいエージェントを返す
        /// </summary>
        /// <returns>この狩人が護衛したいエージェント</returns>
        /// <remarks>nullは護衛なしを意味する</remarks>
#else
        /// <summary>
        /// Returns the agent this bodyguard wants to guard.
        /// </summary>
        /// <returns>The agent this bodyguard wants to guard.</returns>
        /// <remarks>No guard in case of null.</remarks>
#endif
        public override Agent Guard()
        {
            throw new NotImplementedException();
        }
    }
}
