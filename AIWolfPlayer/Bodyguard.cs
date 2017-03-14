//
// Bodyguard.cs
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
    /// 狩人プレイヤーの見本
    /// </summary>
#else
    /// <summary>
    /// Sample bodyguard player.
    /// </summary>
#endif
    public sealed class Bodyguard : BasePlayer
    {
        // 人狼候補リスト
        List<Agent> werewolves = new List<Agent>();
        // 前日護衛したエージェント
        Agent guardedAgent;

        /// <summary>
        /// 投票先候補を選ぶ
        /// </summary>
        /// <returns>投票先候補のエージェント</returns>
        protected override void ChooseVoteCandidate()
        {
            // 自分や殺されたエージェントを人狼と判定していて，生存している占い師を投票先候補とする
            var candidates = DivinationList
                .Where(j => j.Result == Species.WEREWOLF && (j.Target == Me || Killed(j.Target)) && Alive(j.Agent))
                .Select(j => j.Agent).Distinct();
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
            werewolves.Clear();
            guardedAgent = null;
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
            Agent candidate = null;
            // 前日の護衛が成功しているようなら同じエージェントを護衛
            if (guardedAgent != null && Alive(guardedAgent) && CurrentGameInfo.LastDeadAgentList.Count == 0)
            {
                candidate = guardedAgent;
            }
            // 新しい護衛先の選定
            else
            {
                // 占い師をカミングアウトしていて，かつ人狼候補になっていないエージェントを探す
                var candidates = AliveOthers.Where(a => GetCoRole(a) == Role.SEER && !werewolves.Contains(a));
                // 見つからなければ霊媒師をカミングアウトしていて，かつ人狼候補になっていないエージェントを探す
                if (candidates.Count() == 0)
                {
                    candidates = AliveOthers.Where(a => GetCoRole(a) == Role.MEDIUM && !werewolves.Contains(a));
                }
                // それでも見つからなければ自分と人狼候補以外から護衛
                if (candidates.Count() == 0)
                {
                    candidates = AliveOthers.Where(a => a != Me && !werewolves.Contains(a));
                }
                // それでもいなければ自分以外から護衛
                if (candidates.Count() == 0)
                {
                    candidates = AliveOthers;
                }
                // 護衛候補からランダムに護衛
                candidate = candidates.Shuffle().First();
            }
            guardedAgent = candidate;
            return candidate;
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
    }
}
