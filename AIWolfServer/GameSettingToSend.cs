//
// GameSettingToSend.cs
//
// Copyright (c) 2017 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using AIWolf.Lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace AIWolf.Server
{
#if JHELP
    /// <summary>
    /// ゲームの設定
    /// </summary>
#else
    /// <summary>
    /// The settings of the game.
    /// </summary>
#endif
    [DataContract]
    public class GameSetting
    {
        /// <summary>
        /// The number of agents acting as each role.
        /// </summary>
        static readonly Dictionary<int, Dictionary<Role, int>> defaultRoleNumMap = new Dictionary<int, Dictionary<Role, int>>
        {
            [3] = new Dictionary<Role, int> { [Role.BODYGUARD] = 0, [Role.MEDIUM] = 0, [Role.POSSESSED] = 0, [Role.SEER] = 1, [Role.VILLAGER] = 1, [Role.WEREWOLF] = 1, [Role.FREEMASON] = 0, [Role.FOX] = 0, },
            [4] = new Dictionary<Role, int> { [Role.BODYGUARD] = 0, [Role.MEDIUM] = 0, [Role.POSSESSED] = 0, [Role.SEER] = 1, [Role.VILLAGER] = 2, [Role.WEREWOLF] = 1, [Role.FREEMASON] = 0, [Role.FOX] = 0, },
            [5] = new Dictionary<Role, int> { [Role.BODYGUARD] = 0, [Role.MEDIUM] = 0, [Role.POSSESSED] = 1, [Role.SEER] = 1, [Role.VILLAGER] = 2, [Role.WEREWOLF] = 1, [Role.FREEMASON] = 0, [Role.FOX] = 0, },
            [6] = new Dictionary<Role, int> { [Role.BODYGUARD] = 0, [Role.MEDIUM] = 0, [Role.POSSESSED] = 1, [Role.SEER] = 1, [Role.VILLAGER] = 3, [Role.WEREWOLF] = 1, [Role.FREEMASON] = 0, [Role.FOX] = 0, },
            [7] = new Dictionary<Role, int> { [Role.BODYGUARD] = 0, [Role.MEDIUM] = 0, [Role.POSSESSED] = 0, [Role.SEER] = 1, [Role.VILLAGER] = 4, [Role.WEREWOLF] = 2, [Role.FREEMASON] = 0, [Role.FOX] = 0, },
            [8] = new Dictionary<Role, int> { [Role.BODYGUARD] = 0, [Role.MEDIUM] = 1, [Role.POSSESSED] = 0, [Role.SEER] = 1, [Role.VILLAGER] = 4, [Role.WEREWOLF] = 2, [Role.FREEMASON] = 0, [Role.FOX] = 0, },
            [9] = new Dictionary<Role, int> { [Role.BODYGUARD] = 0, [Role.MEDIUM] = 1, [Role.POSSESSED] = 0, [Role.SEER] = 1, [Role.VILLAGER] = 5, [Role.WEREWOLF] = 2, [Role.FREEMASON] = 0, [Role.FOX] = 0, },
           [10] = new Dictionary<Role, int> { [Role.BODYGUARD] = 1, [Role.MEDIUM] = 1, [Role.POSSESSED] = 1, [Role.SEER] = 1, [Role.VILLAGER] = 4, [Role.WEREWOLF] = 2, [Role.FREEMASON] = 0, [Role.FOX] = 0, },
           [11] = new Dictionary<Role, int> { [Role.BODYGUARD] = 1, [Role.MEDIUM] = 1, [Role.POSSESSED] = 1, [Role.SEER] = 1, [Role.VILLAGER] = 5, [Role.WEREWOLF] = 2, [Role.FREEMASON] = 0, [Role.FOX] = 0, },
           [12] = new Dictionary<Role, int> { [Role.BODYGUARD] = 1, [Role.MEDIUM] = 1, [Role.POSSESSED] = 1, [Role.SEER] = 1, [Role.VILLAGER] = 5, [Role.WEREWOLF] = 3, [Role.FREEMASON] = 0, [Role.FOX] = 0, },
           [13] = new Dictionary<Role, int> { [Role.BODYGUARD] = 1, [Role.MEDIUM] = 1, [Role.POSSESSED] = 1, [Role.SEER] = 1, [Role.VILLAGER] = 6, [Role.WEREWOLF] = 3, [Role.FREEMASON] = 0, [Role.FOX] = 0, },
           [14] = new Dictionary<Role, int> { [Role.BODYGUARD] = 1, [Role.MEDIUM] = 1, [Role.POSSESSED] = 1, [Role.SEER] = 1, [Role.VILLAGER] = 7, [Role.WEREWOLF] = 3, [Role.FREEMASON] = 0, [Role.FOX] = 0, },
           [15] = new Dictionary<Role, int> { [Role.BODYGUARD] = 1, [Role.MEDIUM] = 1, [Role.POSSESSED] = 1, [Role.SEER] = 1, [Role.VILLAGER] = 8, [Role.WEREWOLF] = 3, [Role.FREEMASON] = 0, [Role.FOX] = 0, },
           [16] = new Dictionary<Role, int> { [Role.BODYGUARD] = 1, [Role.MEDIUM] = 1, [Role.POSSESSED] = 1, [Role.SEER] = 1, [Role.VILLAGER] = 9, [Role.WEREWOLF] = 3, [Role.FREEMASON] = 0, [Role.FOX] = 0, },
           [17] = new Dictionary<Role, int> { [Role.BODYGUARD] = 1, [Role.MEDIUM] = 1, [Role.POSSESSED] = 1, [Role.SEER] = 1, [Role.VILLAGER] =10, [Role.WEREWOLF] = 3, [Role.FREEMASON] = 0, [Role.FOX] = 0, },
           [18] = new Dictionary<Role, int> { [Role.BODYGUARD] = 1, [Role.MEDIUM] = 1, [Role.POSSESSED] = 1, [Role.SEER] = 1, [Role.VILLAGER] =11, [Role.WEREWOLF] = 3, [Role.FREEMASON] = 0, [Role.FOX] = 0, },
        };

#if JHELP
        /// <summary>
        /// デフォルトのゲーム設定を返す
        /// </summary>
        /// <param name="agentNum">エージェント数</param>
        /// <returns>エージェント数におけるデフォルト設定</returns>
#else
        /// <summary>
        /// Returns the default GameSetting.
        /// </summary>
        /// <param name="agentNum">The number of agents.</param>
        /// <returns>The default GameSetting for the given number of agents.</returns>
#endif
        public static GameSetting GetDefaultGameSetting(int agentNum)
        {
            if (!defaultRoleNumMap.ContainsKey(agentNum))
            {
                throw new ArgumentException("Invalid agentNum in GetDefaultGameSetting(agentNum).");
            }
            return new GameSetting()
            {
                RoleNumMap = defaultRoleNumMap[agentNum],
                MaxTalk = 10,
                MaxTalkTurn = 20,
                MaxWhisper = 10,
                MaxWhisperTurn = 20,
                MaxSkip = 2,
                EnableNoAttack = false,
                VoteVisible = true,
                VotableOnFirstDay = false,
                EnableNoExecution = false,
                TalkOnFirstDay = false,
                ValidateUtterance = true,
                WhisperBeforeRevote = false,
                TimeLimit = -1,
                MaxRevote = 1,
                MaxAttackRevote = 1,
            };
        }

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
        public Dictionary<Role, int> RoleNumMap { get; set; }

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
        public int MaxTalk { get; set; }

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
        public int MaxTalkTurn { get; set; }

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
        public int MaxWhisper { get; set; }

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
        public int MaxWhisperTurn { get; set; }

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
        public int MaxSkip { get; set; }

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
        public int MaxRevote { get; set; }

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
        public int MaxAttackRevote { get; set; }

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
        public bool EnableNoAttack { get; set; }

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
        public bool VoteVisible { get; set; }

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
        public bool VotableOnFirstDay { get; set; }

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
        public bool EnableNoExecution { get; set; }

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
        public bool TalkOnFirstDay { get; set; }

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
        public bool ValidateUtterance { get; set; }

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
        public bool WhisperBeforeRevote { get; set; }

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
        public long RandomSeed { get; set; }

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
        public int TimeLimit { get; set; }

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
        GameSetting() { }

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
