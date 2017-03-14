//
// Medium.cs
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
    /// 霊媒師プレイヤーの見本
    /// </summary>
#else
    /// <summary>
    /// Sample medium player.
    /// </summary>
#endif
    public sealed class Medium : BasePlayer
    {
        // カミングアウトする日
        int comingoutDay;
        // カミングアウト済みか否か
        bool isCameout;
        // 霊媒結果を入れる待ち行列
        Queue<Judge> identQueue = new Queue<Judge>();
        // 霊媒結果マップ
        Dictionary<Agent, Species> myIdentMap = new Dictionary<Agent, Species>();

        /// <summary>
        /// 投票先候補を選ぶ
        /// </summary>
        /// <returns>投票先候補のエージェント</returns>
        protected override void ChooseVoteCandidate()
        {
            // 霊媒師をカミングアウトしている他のエージェントは人狼候補
            var fakeMediums = AliveOthers.Where(a => GetCoRole(a) == Role.MEDIUM);
            // 自分や殺されたエージェントを人狼と判定，あるいは自分と異なる判定の占い師は人狼候補
            var fakeSeers = DivinationList
                .Where(j => (j.Result == Species.WEREWOLF && (j.Target == Me || Killed(j.Target)))
               || (myIdentMap.ContainsKey(j.Target) && j.Result != myIdentMap[j.Target])).Select(j => j.Agent);
            var candidates = fakeMediums.Concat(fakeSeers).Where(a => Alive(a)).Distinct();
            if (candidates.Count() > 0)
            {
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
            // 人狼候補がいない場合はランダム
            else
            {
                if (!AliveOthers.Contains(voteCandidate))
                {
                    voteCandidate = AliveOthers.Shuffle().First();
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
            identQueue.Clear();
            myIdentMap.Clear();
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
            // 霊媒結果を待ち行列に入れる
            if (CurrentGameInfo.MediumResult != null)
            {
                identQueue.Enqueue(CurrentGameInfo.MediumResult);
                myIdentMap[CurrentGameInfo.MediumResult.Target] = CurrentGameInfo.MediumResult.Result;
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
            // カミングアウトする日になったら，あるいは霊媒結果が人狼だったら
            // あるいは霊媒師カミングアウトが出たらカミングアウト
            if (!isCameout && (Day >= comingoutDay
                    || (identQueue.Count > 0 && identQueue.Peek().Result == Species.WEREWOLF)
                    || IsCo(Role.MEDIUM)))
            {
                TalkQueue.Enqueue(new Content(new ComingoutContentBuilder(Me, Role.MEDIUM)));
                isCameout = true;
            }
            // カミングアウトしたらこれまでの霊媒結果をすべて公開
            if (isCameout)
            {
                while (identQueue.Count > 0)
                {
                    Judge ident = identQueue.Dequeue();
                    TalkQueue.Enqueue(new Content(new IdentContentBuilder(ident.Target, ident.Result)));
                }
            }
            return base.Talk();
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
