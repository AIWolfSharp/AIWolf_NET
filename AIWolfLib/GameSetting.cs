//
// GameSetting.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace AIWolf.Lib
{
#if JHELP
    /// <summary>
    /// ゲーム設定
    /// </summary>
#else
    /// <summary>
    /// Settings of game.
    /// </summary>
#endif
    [DataContract]
    public class GameSetting
    {
#if JHELP
        /// <summary>
        /// 各役職の人数
        /// </summary>
#else
        /// <summary>
        /// The number of each role.
        /// </summary>
#endif
        [DataMember(Name = "roleNumMap")]
        public Dictionary<Role, int> RoleNumMap { get; } //= new Dictionary<Role, int>();

#if JHELP
        /// <summary>
        /// １日に許される最大会話回数
        /// </summary>
#else
        /// <summary>
        /// The maximum number of talks.
        /// </summary>
#endif
        [DataMember(Name = "maxTalk")]
        public int MaxTalk { get; }

#if JHELP
        /// <summary>
        /// １日に許される最大会話ターン数
        /// </summary>
#else
        /// <summary>
        /// The maximum number of turns of talk.
        /// </summary>
#endif
        [DataMember(Name = "maxTalkTurn")]
        public int MaxTalkTurn { get; }

#if JHELP
        /// <summary>
        /// １日に許される最大囁き回数
        /// </summary>
#else
        /// <summary>
        /// The maximum number of whispers a day.
        /// </summary>
#endif
        [DataMember(Name = "maxWhisper")]
        public int MaxWhisper { get; }

#if JHELP
        /// <summary>
        /// １日に許される最大囁きターン数
        /// </summary>
#else
        /// <summary>
        /// The maximum number of turns of whisper.
        /// </summary>
#endif
        [DataMember(Name = "maxWhisperTurn")]
        public int MaxWhisperTurn { get; }

#if JHELP
        /// <summary>
        /// 連続スキップの最大許容長
        /// </summary>
#else
        /// <summary>
        /// The maximum permissible length of the succession of SKIPs.
        /// </summary>
#endif
        [DataMember(Name = "maxSkip")]
        public int MaxSkip { get; }

#if JHELP
        /// <summary>
        /// 最大再投票回数
        /// </summary>
#else
        /// <summary>
        /// The maximum number of revotes.
        /// </summary>
#endif
        [DataMember(Name = "maxRevote")]
        public int MaxRevote { get; }

#if JHELP
        /// <summary>
        /// 最大再襲撃投票回数
        /// </summary>
#else
        /// <summary>
        /// The maximum number of revotes for attack.
        /// </summary>
#endif
        [DataMember(Name = "maxAttackRevote")]
        public int MaxAttackRevote { get; }

#if JHELP
        /// <summary>
        /// 誰も襲撃しないことを許すか否か
        /// </summary>
#else
        /// <summary>
        /// Whether or not the game permit to attack no one.
        /// </summary>
#endif
        [DataMember(Name = "enableNoAttack")]
        public bool EnableNoAttack { get; }

#if JHELP
        /// <summary>
        /// 誰が誰に投票したかわかるか否か
        /// </summary>
#else
        /// <summary>
        /// Whether or not agent can see who vote to who.
        /// </summary>
#endif
        [DataMember(Name = "voteVisible")]
        public bool VoteVisible { get; }

#if JHELP
        /// <summary>
        /// 初日に投票があるか否か
        /// </summary>
#else
        /// <summary>
        /// Whether or not there is vote on the first day.
        /// </summary>
#endif
        [DataMember(Name = "votableInFirstDay")]
        public bool VotableOnFirstDay { get; }

#if JHELP
        /// <summary>
        /// 同票数の場合追放なしとするか否か
        /// </summary>
#else
        /// <summary>
        /// Whether or not executing nobody is allowed.
        /// </summary>
#endif
        [DataMember(Name = "enableNoExecution")]
        public bool EnableNoExecution { get; }

#if JHELP
        /// <summary>
        /// 初日にトークがあるか否か
        /// </summary>
#else
        /// <summary>
        /// Whether or not there are talks on the first day.
        /// </summary>
#endif
        [DataMember(Name = "talkOnFirstDay")]
        public bool TalkOnFirstDay { get; }

#if JHELP
        /// <summary>
        /// 発話文字列のチェックをするか否か
        /// </summary>
#else
        /// <summary>
        /// Whether or not the uttered text is validated.
        /// </summary>
#endif
        [DataMember(Name = "validateUtterance")]
        public bool ValidateUtterance { get; }

#if JHELP
        /// <summary>
        /// 再襲撃投票前に囁きがあるか否か
        /// </summary>
#else
        /// <summary>
        /// Whether or not werewolf can whisper before the revote for attack.
        /// </summary>
#endif
        [DataMember(Name = "whisperBeforeRevote")]
        public bool WhisperBeforeRevote { get; }

#if JHELP
        /// <summary>
        /// 乱数の種
        /// </summary>
#else
        /// <summary>
        /// The random seed.
        /// </summary>
#endif
        [DataMember(Name = "randomSeed")]
        public long RandomSeed { get; }

#if JHELP
        /// <summary>
        /// リクエスト応答時間の上限
        /// </summary>
#else
        /// <summary>
        /// The upper limit for the response time to the request.
        /// </summary>
#endif
        [DataMember(Name = "timeLimit")]
        public int TimeLimit { get; }

#if JHELP
        /// <summary>
        /// プレイヤーの数
        /// </summary>
#else
        /// <summary>
        /// The number of players.
        /// </summary>
#endif
        [DataMember(Name = "playerNum")]
        public int PlayerNum
        {
            get
            {
                return RoleNumMap == null ? 0 : RoleNumMap.Values.Sum();
            }
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        [JsonConstructor]
        GameSetting(Dictionary<Role, int> roleNumMap, int maxTalk, int maxTalkTurn, int maxWhisper,
            int maxWhisperTurn, int maxSkip, int maxRevote, int maxAttackRevote, bool enableNoAttack,
            bool voteVisible, bool votableInFirstDay, bool enableNoExecution, bool talkOnFirstDay,
            bool validateUtterance, bool whisperBeforeRevote, long randomSeed, int timeLimit)
        {
            RoleNumMap = roleNumMap;
            MaxTalk = maxTalk;
            MaxTalkTurn = maxTalkTurn;
            MaxWhisper = maxWhisper;
            MaxWhisperTurn = maxWhisperTurn;
            MaxSkip = maxSkip;
            MaxRevote = maxRevote;
            MaxAttackRevote = maxAttackRevote;
            EnableNoAttack = enableNoAttack;
            VoteVisible = voteVisible;
            VotableOnFirstDay = votableInFirstDay;
            EnableNoExecution = enableNoExecution;
            TalkOnFirstDay = talkOnFirstDay;
            ValidateUtterance = validateUtterance;
            WhisperBeforeRevote = whisperBeforeRevote;
            RandomSeed = randomSeed;
            TimeLimit = timeLimit;
        }
    }
}
