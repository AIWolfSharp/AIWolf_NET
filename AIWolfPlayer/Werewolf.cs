//
// Werewolf.cs
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
    /// 人狼プレイヤーの見本
    /// </summary>
#else
    /// <summary>
    /// Sample werewolf player.
    /// </summary>
#endif
    public sealed class Werewolf : BasePlayer
    {
        // 規定人狼数
        int numWolves;
        // 騙る役職
        Role fakeRole;
        // カミングアウトする日
        int comingoutDay;
        // カミングアウトするターン
        int comingoutTurn;
        // カミングアウト済みか否か
        bool isCameout;
        // 偽判定結果を入れるリスト
        List<Judge> fakeJudgeList = new List<Judge>();
        // 偽判定結果を入れる待ち行列
        Queue<Judge> fakeJudgeQueue = new Queue<Judge>();
        // 偽判定済みエージェントのリスト
        List<Agent> judgedAgents = new List<Agent>();
        // 裏切り者エージェントのリスト
        List<Agent> possessedList = new List<Agent>();
        // 裏切り者エージェント
        Agent possessed;
        // 人狼リスト
        List<Agent> werewolves;
        // 人間リスト
        List<Agent> humans;
        // talk()のターン
        int talkTurn;

        /// <summary>
        /// 襲撃先候補を選ぶ
        /// </summary>
        /// <returns>襲撃先候補のエージェント</returns>
        protected override void ChooseAttackVoteCandidate()
        {
            // カミングアウトした村人陣営は襲撃先候補
            List<Agent> villagers = AliveOthers.Where(a => !werewolves.Contains(a) && a != possessed).ToList();
            List<Agent> candidates = villagers.Where(a => IsCo(a)).ToList();
            // 候補がいなければ村人陣営から
            if (candidates.Count() == 0)
            {
                candidates = villagers;
            }
            // 村人陣営がいない場合は裏切り者を襲う
            if (candidates.Count() == 0 && possessed != null)
            {
                candidates.Add(possessed);
            }
            if (candidates.Count() > 0)
            {
                if (!candidates.Contains(attackVoteCandidate))
                {
                    attackVoteCandidate = candidates.Shuffle().First();
                }
            }
            else
            {
                attackVoteCandidate = null;
            }
        }

        /// <summary>
        /// 投票先候補を選ぶ
        /// </summary>
        /// <returns>投票先候補のエージェント</returns>
        protected override void ChooseVoteCandidate()
        {
            List<Agent> villagers = AliveOthers.Where(a => !werewolves.Contains(a) && a != possessed).ToList();
            List<Agent> candidates = villagers; // 村人騙りの場合は村人陣営から
            if (fakeRole != Role.VILLAGER) // 占い師/霊媒師騙りの場合
            {
                // 対抗カミングアウトしたエージェントは投票先候補
                var rivals = villagers.Where(a => GetCoRole(a) == fakeRole);
                // 人狼と判定したエージェントは投票先候補
                var fakeWolves = fakeJudgeList
                    .Where(j => AliveOthers.Contains(j.Target) && j.Result == Species.WEREWOLF)
                    .Select(j => j.Target).Distinct();
                candidates = rivals.Concat(fakeWolves).ToList();
                // 候補がいなければ人間と判定していない村人陣営から
                if (candidates.Count() == 0)
                {
                    candidates = fakeJudgeList
                        .Where(j => AliveOthers.Contains(j.Target) && j.Result != Species.HUMAN)
                        .Select(j => j.Target).Distinct().ToList();
                }
            }
            // 候補がいなければ村人陣営から
            if (candidates.Count() == 0)
            {
                candidates = villagers;
            }
            // 村人陣営がいない場合は裏切り者に投票
            if (candidates.Count() == 0 && possessed != null)
            {
                candidates.Add(possessed);
            }
            if (candidates.Count() > 0)
            {
                if (!candidates.Contains(voteCandidate))
                {
                    voteCandidate = candidates.Shuffle().First();
                    // 以前の投票先から変わる場合，新たに推測発言
                    if (CanTalk)
                    {
                        TalkQueue.Enqueue(new Content(new EstimateContentBuilder(voteCandidate, Role.WEREWOLF)));
                    }
                }
            }
            else
            {
                voteCandidate = null;
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
            werewolves = gameInfo.RoleMap.Keys.ToList();
            humans = AliveOthers.Where(a => !werewolves.Contains(a)).ToList();
            // ランダムに騙る役職を決める
            fakeRole = new Role[] { Role.VILLAGER, Role.SEER, Role.MEDIUM }.
                Where(r => gameInfo.ExistingRoleList.Contains(r)).Shuffle().First();
            WhisperQueue.Enqueue(new Content(new ComingoutContentBuilder(Me, fakeRole)));
            // 1～3日目からランダムにカミングアウトする
            comingoutDay = new int[] { 1, 2, 3 }.Shuffle().First();
            // 第0～4ターンからランダムにカミングアウトする
            comingoutTurn = new int[] { 0, 1, 2, 3, 4 }.Shuffle().First();
            isCameout = false;
            fakeJudgeList.Clear();
            fakeJudgeQueue.Clear();
            judgedAgents.Clear();
            possessedList.Clear();
        }

#if JHELP
        /// <summary>
        /// ゲーム情報更新の際に呼ばれる
        /// </summary>
        /// <param name="gameInfo">最新のゲーム情報</param>
#else
        /// <summary>
        /// Called when the game information is updated.
        /// </summary>
        /// <param name="gameInfo">The current information of this game.</param>
#endif
        public override void Update(GameInfo gameInfo)
        {
            base.Update(gameInfo);
            // 占い/霊媒結果が嘘の場合，裏切り者候補
            possessed = DivinationList.Concat(IdentList)
                .Where(j => !werewolves.Contains(j.Agent)
                && ((humans.Contains(j.Target) && j.Result == Species.WEREWOLF)
                || (werewolves.Contains(j.Target) && j.Result == Species.HUMAN)))
                .Select(j => j.Agent).Distinct().Shuffle().FirstOrDefault();
            if (possessed != null && !possessedList.Contains(possessed))
            {
                possessedList.Add(possessed);
                WhisperQueue.Enqueue(new Content(new EstimateContentBuilder(possessed, Role.POSSESSED)));
            }
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
            talkTurn = -1;
            if (Day == 0)
            {
                WhisperQueue.Enqueue(new Content(new ComingoutContentBuilder(Me, fakeRole)));
            }
            // 偽の判定
            else
            {
                Judge fakeJudge = GetFakeJudge(fakeRole);
                if (fakeJudge != null)
                {
                    fakeJudgeList.Add(fakeJudge);
                    fakeJudgeQueue.Enqueue(fakeJudge);
                    if (fakeRole == Role.SEER)
                    {
                        judgedAgents.Add(fakeJudge.Target);
                    }
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
            talkTurn++;
            if (fakeRole != Role.VILLAGER)
            {
                if (!isCameout)
                {
                    // 他の人狼のカミングアウト状況を調べて騙る役職が重複しないようにする
                    int fakeSeerCO = werewolves.Where(a => a != Me && GetCoRole(a) == Role.SEER).Count();
                    int fakeMediumCO = werewolves.Where(a => a != Me && GetCoRole(a) == Role.MEDIUM).Count();
                    if ((fakeRole == Role.SEER && fakeSeerCO > 0) || (fakeRole == Role.MEDIUM && fakeMediumCO > 0))
                    {
                        fakeRole = Role.VILLAGER; // 潜伏
                        WhisperQueue.Enqueue(new Content(new ComingoutContentBuilder(Me, Role.VILLAGER)));
                    }
                    else
                    {
                        // 対抗カミングアウトがある場合，今日カミングアウトする
                        if (humans.Where(a => GetCoRole(a) == fakeRole).Count() > 0)
                        {
                            comingoutDay = Day;
                        }
                        // カミングアウトするタイミングになったらカミングアウト
                        if (Day >= comingoutDay && talkTurn >= comingoutTurn)
                        {
                            isCameout = true;
                            TalkQueue.Enqueue(new Content(new ComingoutContentBuilder(Me, fakeRole)));
                        }
                    }
                }
                // カミングアウトしたらこれまでの偽判定結果をすべて公開
                else
                {
                    while (fakeJudgeQueue.Count > 0)
                    {
                        Judge judge = fakeJudgeQueue.Dequeue();
                        if (fakeRole == Role.SEER)
                        {
                            TalkQueue.Enqueue(new Content(new DivinedResultContentBuilder(judge.Target, judge.Result)));
                        }
                        else if (fakeRole == Role.MEDIUM)
                        {
                            TalkQueue.Enqueue(new Content(new IdentContentBuilder(judge.Target, judge.Result)));
                        }
                    }
                }
            }
            return base.Talk();
        }

        /// <summary>
        /// 偽判定を返す
        /// </summary>
        /// <param name="fakeRole">偽役職</param>
        /// <returns>偽判定結果</returns>
        Judge GetFakeJudge(Role fakeRole)
        {
            Agent target = null;
            // 占い師騙りの場合
            if (fakeRole == Role.SEER)
            {
                target = AliveOthers.Where(a => !judgedAgents.Contains(a) && GetCoRole(a) != Role.SEER)
                    .Shuffle().FirstOrDefault();
                if (target == null)
                {
                    target = AliveOthers.Shuffle().First();
                }
            }
            // 霊媒師騙りの場合
            else if (fakeRole == Role.MEDIUM)
            {
                target = CurrentGameInfo.ExecutedAgent;
            }
            if (target != null)
            {
                Species result = Species.HUMAN;
                // 人間が偽占い対象の場合
                if (humans.Contains(target))
                {
                    // 偽人狼に余裕があれば
                    if (fakeJudgeList.Where(j => j.Result == Species.WEREWOLF).Count() < numWolves)
                    {
                        // 裏切り者，あるいはまだカミングアウトしていないエージェントの場合，判定は五分五分
                        if ((target == possessed || !IsCo(target)))
                        {
                            if (new Random().NextDouble() < 0.5)
                            {
                                result = Species.WEREWOLF;
                            }
                        }
                        // それ以外は人狼判定
                        else
                        {
                            result = Species.WEREWOLF;
                        }
                    }
                }
                return new Judge(Day, Me, target, result);
            }
            else
            {
                return null;
            }
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
