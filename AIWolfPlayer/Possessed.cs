//
// Possessed.cs
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
    /// 裏切り者プレイヤーの見本
    /// </summary>
#else
    /// <summary>
    /// Sample possessed player.
    /// </summary>
#endif
    public sealed class Possessed : BasePlayer
    {
        // 規定人狼数
        int numWolves;
        // カミングアウト済みか否か
        bool isCameout;
        // 偽占い結果リスト
        List<Judge> fakeDivinationList = new List<Judge>();
        // 偽占い結果を入れる待ち行列
        Queue<Judge> fakeDivinationQueue = new Queue<Judge>();
        // 偽占い済みエージェントのリスト
        List<Agent> divinedAgents = new List<Agent>();

        /// <summary>
        /// 投票先候補を選ぶ
        /// </summary>
        /// <returns>投票先候補のエージェント</returns>
        protected override void ChooseVoteCandidate()
        {
            // 自分や殺されたエージェントを人狼と判定していて，生存している占い師は人狼候補
            var werewolves = DivinationList
                .Where(j => j.Result == Species.WEREWOLF && (j.Target == Me || Killed(j.Target))).Select(j => j.Agent);
            // 対抗カミングアウトのエージェントは投票先候補
            var rivals = AliveOthers.Where(a => !werewolves.Contains(a) && GetCoRole(a) == Role.SEER);
            // 人狼と判定したエージェントは投票先候補
            var fakeHumans = fakeDivinationQueue.Where(j => j.Result == Species.HUMAN).Select(j => j.Target).Distinct();
            var fakeWerewolves = fakeDivinationQueue.Where(j => j.Result == Species.WEREWOLF).Select(j => j.Target).Distinct();
            var candidates = rivals.Concat(fakeWerewolves).Distinct();
            // 候補がいなければ人間と判定していない村人陣営から
            if (candidates.Count() == 0)
            {
                candidates = AliveOthers.Where(a => !werewolves.Contains(a) && !fakeHumans.Contains(a));
                // それでも候補がいなければ村人陣営から
                if (candidates.Count() == 0)
                {
                    candidates = AliveOthers.Where(a => !werewolves.Contains(a));
                }
            }
            if (!candidates.Contains(voteCandidate))
            {
                voteCandidate = candidates.Shuffle().First();
                // 以前の投票先から変わる場合，新たに推測発言と占い要請をする
                if (CanTalk)
                {
                    TalkQueue.Enqueue(new Content(new EstimateContentBuilder(voteCandidate, Role.WEREWOLF)));
                    TalkQueue.Enqueue(new Content(new RequestContentBuilder(null, new Content(new DivinationContentBuilder(voteCandidate)))));
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
            numWolves = gameSetting.RoleNumMap[Role.WEREWOLF];
            isCameout = false;
            fakeDivinationList.Clear();
            fakeDivinationQueue.Clear();
            divinedAgents.Clear();
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
            // 偽の判定
            if (Day > 0)
            {
                Judge judge = GetFakeDivination();
                if (judge != null)
                {
                    fakeDivinationList.Add(judge);
                    fakeDivinationQueue.Enqueue(judge);
                    divinedAgents.Add(judge.Target);
                }
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
            // 即占い師カミングアウト
            if (!isCameout)
            {
                TalkQueue.Enqueue(new Content(new ComingoutContentBuilder(Me, Role.SEER)));
                isCameout = true;
            }
            // カミングアウトしたらこれまでの偽判定結果をすべて公開
            if (isCameout)
            {
                while (fakeDivinationQueue.Count > 0)
                {
                    Judge judge = fakeDivinationQueue.Dequeue();
                    TalkQueue.Enqueue(new Content(new DivinedResultContentBuilder(judge.Target, judge.Result)));
                }
            }
            return base.Talk();
        }

        /// <summary>
        /// 偽占い結果を返す
        /// </summary>
        /// <returns>偽占い結果</returns>
        Judge GetFakeDivination()
        {
            Agent target = null;
            var candidates = AliveOthers.Where(a => !divinedAgents.Contains(a) && GetCoRole(a) != Role.SEER);
            if (candidates.Count() > 0)
            {
                target = candidates.Shuffle().First();
            }
            else
            {
                target = AliveOthers.Shuffle().First();
            }
            // 偽人狼に余裕があれば，人狼と人間の割合を勘案して，30%の確率で人狼と判定
            Species result = Species.HUMAN;
            if (fakeDivinationList.Where(j => j.Result == Species.WEREWOLF).Count() < numWolves && new Random().NextDouble() < 0.3)
            {
                result = Species.WEREWOLF;
            }
            return new Judge(Day, Me, target, result);
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
