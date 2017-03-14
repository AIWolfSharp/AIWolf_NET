//
// BasePlayer.cs
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
    /// プレイヤー用基本クラス
    /// </summary>
#else
    /// <summary>
    /// Base class for player.
    /// </summary>
#endif
    public class BasePlayer : IPlayer
    {
        /// <summary>
        /// このエージェント
        /// </summary>
        protected Agent Me { get; private set; }
        /// <summary>
        /// 日付
        /// </summary>
        protected int Day { get; private set; }
        /// <summary>
        /// Talk()できるか否か
        /// </summary>
        protected bool CanTalk { get; private set; }
        /// <summary>
        /// Whisper()できるか否か
        /// </summary>
        protected bool CanWhisper { get; private set; }
        /// <summary>
        /// 最新のゲーム情報
        /// </summary>
        protected GameInfo CurrentGameInfo { get; private set; }
        /// <summary>
        /// 自分以外の生存エージェントのリスト
        /// </summary>
        protected List<Agent> AliveOthers { get; private set; }
        /// <summary>
        /// 追放されたエージェントのリスト
        /// </summary>
        protected List<Agent> ExecutedAgents { get; private set; } = new List<Agent>();
        /// <summary>
        /// 殺されたエージェントのリスト
        /// </summary>
        protected List<Agent> KilledAgents { get; private set; } = new List<Agent>();
        /// <summary>
        /// 占い報告リスト
        /// </summary>
        protected List<Judge> DivinationList { get; private set; } = new List<Judge>();
        /// <summary>
        /// 霊媒報告リスト
        /// </summary>
        protected List<Judge> IdentList { get; private set; } = new List<Judge>();
        /// <summary>
        /// 発言の待ち行列
        /// </summary>
        protected Queue<Content> TalkQueue { get; private set; } = new Queue<Content>();
        /// <summary>
        /// 囁きの待ち行列
        /// </summary>
        protected Queue<Content> WhisperQueue { get; private set; } = new Queue<Content>();

        /// <summary>
        /// 投票先候補
        /// </summary>
        protected Agent voteCandidate;
        // 宣言した投票先
        Agent declaredVoteCandidate;
        /// <summary>
        /// 襲撃投票先候補
        /// </summary>
        protected Agent attackVoteCandidate;
        // 宣言した襲撃投票先
        Agent declaredAttackVoteCandidate;

        // カミングアウト状況
        Dictionary<Agent, Role> comingoutMap = new Dictionary<Agent, Role>();
        int talkListHead; // GameInfo.TalkList読み込みのヘッド

        /// <summary>
        /// プレイヤー名
        /// </summary>
        public virtual string Name
        {
            get
            {
                return GetType().ToString();
            }
        }

        /// <summary>
        /// ゲーム開始時に呼ばれる
        /// </summary>
        /// <param name="gameInfo">最新のゲーム情報</param>
        /// <param name="gameSetting">ゲーム設定</param>
        public virtual void Initialize(GameInfo gameInfo, GameSetting gameSetting)
        {
            Day = -1;
            Me = gameInfo.Agent;
            AliveOthers = gameInfo.AliveAgentList.Where(a => a != Me).ToList();
            ExecutedAgents.Clear();
            KilledAgents.Clear();
            DivinationList.Clear();
            IdentList.Clear();
            comingoutMap.Clear();
        }

        /// <summary>
        /// 新しい日が始まるときに呼ばれる
        /// </summary>
        public virtual void DayStart()
        {
            CanTalk = true;
            CanWhisper = false;
            if (CurrentGameInfo.Role == Role.WEREWOLF)
            {
                CanWhisper = true;
            }
            TalkQueue.Clear();
            WhisperQueue.Clear();
            declaredVoteCandidate = null;
            voteCandidate = null;
            declaredAttackVoteCandidate = null;
            attackVoteCandidate = null;
            talkListHead = 0;
            // 前日に追放されたエージェントを登録
            AddExecutedAgent(CurrentGameInfo.ExecutedAgent);
            // 昨夜に死亡した（襲撃された）エージェントを登録
            if (CurrentGameInfo.LastDeadAgentList.Count > 0)
            {
                AddKilledAgent(CurrentGameInfo.LastDeadAgentList[0]);
            }
        }

        /// <summary>
        /// ゲーム情報更新の際に呼ばれる
        /// </summary>
        /// <param name="gameInfo">最新のゲーム情報</param>
        public virtual void Update(GameInfo gameInfo)
        {
            CurrentGameInfo = gameInfo;
            // 1日の最初の呼び出しはDayStart()の前なので何もしない
            if (CurrentGameInfo.Day == Day + 1)
            {
                Day = gameInfo.Day;
                return;
            }
            // 2回目の呼び出し以降
            // （夜フェーズ限定）追放されたエージェントを登録
            AddExecutedAgent(CurrentGameInfo.LatestExecutedAgent);
            // GameInfo.TalkListからカミングアウト，占い報告，霊媒報告，推定を抽出
            for (int i = talkListHead; i < CurrentGameInfo.TalkList.Count; i++)
            {
                Talk talk = CurrentGameInfo.TalkList[i];
                Agent talker = talk.Agent;
                if (talker == Me)
                {
                    continue;
                }
                Content content = new Content(talk.Text);
                switch (content.Topic)
                {
                    case Topic.COMINGOUT:
                        SetCoRole(talker, content.Role);
                        break;
                    case Topic.DIVINED:
                        DivinationList.Add(new Judge(Day, talker, content.Target, content.Result));
                        break;
                    case Topic.IDENTIFIED:
                        IdentList.Add(new Judge(Day, talker, content.Target, content.Result));
                        break;
                    default:
                        break;
                }
            }
            talkListHead = CurrentGameInfo.TalkList.Count;
        }

        /// <summary>
        /// プレイヤーの発言を返す
        /// </summary>
        /// <returns>発話文字列</returns>
        public virtual string Talk()
        {
            ChooseVoteCandidate();
            if (voteCandidate != null && voteCandidate != declaredVoteCandidate)
            {
                TalkQueue.Enqueue(new Content(new VoteContentBuilder(voteCandidate)));
                declaredVoteCandidate = voteCandidate;
            }
            return TalkQueue.Count > 0 ? TalkQueue.Dequeue().Text : Utterance.SKIP;
        }

        /// <summary>
        /// プレイヤーの囁きを返す
        /// </summary>
        /// <returns>発話文字列</returns>
        public virtual string Whisper()
        {
            ChooseAttackVoteCandidate();
            if (attackVoteCandidate != null && attackVoteCandidate != declaredAttackVoteCandidate)
            {
                WhisperQueue.Enqueue(new Content(new AttackContentBuilder(attackVoteCandidate)));
                declaredAttackVoteCandidate = attackVoteCandidate;
            }
            return WhisperQueue.Count > 0 ? WhisperQueue.Dequeue().Text : Utterance.SKIP;
        }

        /// <summary>
        /// このプレイヤーが追放したいエージェントを返す
        /// </summary>
        /// <returns>このプレイヤーが追放したいエージェント</returns>
        /// <remarks>nullを返した場合エージェントはランダムに決められる</remarks>
        public virtual Agent Vote()
        {
            CanTalk = false;
            ChooseVoteCandidate();
            //CanTalk = true;
            return voteCandidate;
        }

        /// <summary>
        /// この人狼が襲撃したいエージェントを返す
        /// </summary>
        /// <returns>この人狼が襲撃したいエージェント</returns>
        /// <remarks>nullは襲撃なしを意味する</remarks>
        public virtual Agent Attack()
        {
            CanWhisper = false;
            ChooseAttackVoteCandidate();
            CanWhisper = true;
            return attackVoteCandidate;
        }

        /// <summary>
        /// この占い師が占いたいエージェントを返す
        /// </summary>
        /// <returns>この占い師が占いたいエージェント</returns>
        /// <remarks>nullは占いなしを意味する</remarks>
        public virtual Agent Divine()
        {
            return null;
        }

        /// <summary>
        /// この狩人が護衛したいエージェントを返す
        /// </summary>
        /// <returns>この狩人が護衛したいエージェント</returns>
        /// <remarks>nullは護衛なしを意味する</remarks>
        public virtual Agent Guard()
        {
            return null;
        }

        /// <summary>
        /// ゲーム終了時に呼ばれる
        /// </summary>
        /// <remarks>このメソッドが呼ばれる前に，ゲーム情報は補完される（役職公開）</remarks>
        public virtual void Finish()
        {
        }

        /// <summary>
        /// 投票先候補を選ぶ
        /// </summary>
        /// <returns>投票先候補のエージェント</returns>
        protected virtual void ChooseVoteCandidate()
        {
        }

        /// <summary>
        /// 襲撃先候補を選ぶ
        /// </summary>
        /// <returns>襲撃先候補のエージェント</returns>
        protected virtual void ChooseAttackVoteCandidate()
        {
        }

        /// <summary>
        /// エージェントを追放されたエージェントのリストに追加する
        /// </summary>
        void AddExecutedAgent(Agent executedAgent)
        {
            if (executedAgent != null)
            {
                AliveOthers.Remove(executedAgent);
                if (!ExecutedAgents.Contains(executedAgent))
                {
                    ExecutedAgents.Add(executedAgent);
                }
            }
        }

        /// <summary>
        /// エージェントを殺されたエージェントのリストに追加する
        /// </summary>
        void AddKilledAgent(Agent killedAgent)
        {
            if (killedAgent != null)
            {
                AliveOthers.Remove(killedAgent);
                if (!KilledAgents.Contains(killedAgent))
                {
                    KilledAgents.Add(killedAgent);
                }
            }
        }

        /// <summary>
        /// エージェントが生存しているかどうかを返す
        /// </summary>
        /// <param name="agent">エージェント</param>
        /// <returns>生存の場合true</returns>
        protected bool Alive(Agent agent)
        {
            return CurrentGameInfo.StatusMap[agent] == Status.ALIVE;
        }

        /// <summary>
        /// エージェントが殺されたかどうかを返す
        /// </summary>
        /// <param name="agent">エージェント</param>
        /// <returns>殺された場合true</returns>
        protected bool Killed(Agent agent)
        {
            return KilledAgents.Contains(agent);
        }

        /// <summary>
        /// エージェントのカミングアウトした役職をセットする
        /// </summary>
        /// <param name="agent">エージェント</param>
        /// <param name="role">カミングアウトした役職</param>
        protected void SetCoRole(Agent agent, Role role)
        {
            comingoutMap[agent] = role;
        }

        /// <summary>
        /// エージェントのカミングアウトした役職を返す
        /// </summary>
        /// <param name="agent">エージェント</param>
        /// <returns>カミングアウトした役職．していない場合はRole.UNC</returns>
        protected Role GetCoRole(Agent agent)
        {
            if (comingoutMap.ContainsKey(agent))
            {
                return comingoutMap[agent];
            }
            return Role.UNC;
        }

        /// <summary>
        /// エージェントがカミングアウトしたかどうかを返す
        /// </summary>
        /// <param name="agent">エージェント</param>
        /// <returns>カミングアウトしていればtrue</returns>
        protected bool IsCo(Agent agent)
        {
            return comingoutMap.ContainsKey(agent);
        }

        /// <summary>
        /// 役職がカミングアウトされたかどうかを返す
        /// </summary>
        /// <param name="role">役職</param>
        /// <returns>カミングアウトされていればtrue</returns>
        protected bool IsCo(Role role)
        {
            return comingoutMap.ContainsValue(role);
        }
    }
}
