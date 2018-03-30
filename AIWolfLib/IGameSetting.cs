//
// IGameSetting.cs
//
// Copyright (c) 2018 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using System.Collections.Generic;

namespace AIWolf.Lib
{
#if JHELP
    /// <summary>
    /// ゲーム設定クラスが実装すべきプロパティ
    /// </summary>
#else
    /// <summary>
    /// Defines the properties to be in the class for game settings.
    /// </summary>
#endif
    public interface IGameSetting
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
        IDictionary<Role, int> RoleNumMap { get; }

#if JHELP
        /// <summary>
        /// １日に許される最大会話回数
        /// </summary>
#else
        /// <summary>
        /// The maximum number of talks.
        /// </summary>
#endif
        int MaxTalk { get; }

#if JHELP
        /// <summary>
        /// １日に許される最大会話ターン数
        /// </summary>
#else
        /// <summary>
        /// The maximum number of turns of talk.
        /// </summary>
#endif
        int MaxTalkTurn { get; }

#if JHELP
        /// <summary>
        /// １日に許される最大囁き回数
        /// </summary>
#else
        /// <summary>
        /// The maximum number of whispers a day.
        /// </summary>
#endif
        int MaxWhisper { get; }

#if JHELP
        /// <summary>
        /// １日に許される最大囁きターン数
        /// </summary>
#else
        /// <summary>
        /// The maximum number of turns of whisper.
        /// </summary>
#endif
        int MaxWhisperTurn { get; }

#if JHELP
        /// <summary>
        /// 連続スキップの最大許容長
        /// </summary>
#else
        /// <summary>
        /// The maximum permissible length of the succession of SKIPs.
        /// </summary>
#endif
        int MaxSkip { get; }

#if JHELP
        /// <summary>
        /// 最大再投票回数
        /// </summary>
#else
        /// <summary>
        /// The maximum number of revotes.
        /// </summary>
#endif
        int MaxRevote { get; }

#if JHELP
        /// <summary>
        /// 最大再襲撃投票回数
        /// </summary>
#else
        /// <summary>
        /// The maximum number of revotes for attack.
        /// </summary>
#endif
        int MaxAttackRevote { get; }

#if JHELP
        /// <summary>
        /// 誰も襲撃しないことを許すか否か
        /// </summary>
#else
        /// <summary>
        /// Whether or not the game permit to attack no one.
        /// </summary>
#endif
        bool EnableNoAttack { get; }

#if JHELP
        /// <summary>
        /// 誰が誰に投票したかわかるか否か
        /// </summary>
#else
        /// <summary>
        /// Whether or not agent can see who vote to who.
        /// </summary>
#endif
        bool VoteVisible { get; }

#if JHELP
        /// <summary>
        /// 初日に投票があるか否か
        /// </summary>
#else
        /// <summary>
        /// Whether or not there is vote on the first day.
        /// </summary>
#endif
        bool VotableOnFirstDay { get; }

#if JHELP
        /// <summary>
        /// 同票数の場合追放なしとするか否か
        /// </summary>
#else
        /// <summary>
        /// Whether or not executing nobody is allowed.
        /// </summary>
#endif
        bool EnableNoExecution { get; }

#if JHELP
        /// <summary>
        /// 初日にトークがあるか否か
        /// </summary>
#else
        /// <summary>
        /// Whether or not there are talks on the first day.
        /// </summary>
#endif
        bool TalkOnFirstDay { get; }

#if JHELP
        /// <summary>
        /// 発話文字列のチェックをするか否か
        /// </summary>
#else
        /// <summary>
        /// Whether or not the uttered text is validated.
        /// </summary>
#endif
        bool ValidateUtterance { get; }

#if JHELP
        /// <summary>
        /// 再襲撃投票前に囁きがあるか否か
        /// </summary>
#else
        /// <summary>
        /// Whether or not werewolf can whisper before the revote for attack.
        /// </summary>
#endif
        bool WhisperBeforeRevote { get; }

#if JHELP
        /// <summary>
        /// 乱数の種
        /// </summary>
#else
        /// <summary>
        /// The random seed.
        /// </summary>
#endif
        long RandomSeed { get; }

#if JHELP
        /// <summary>
        /// リクエスト応答時間の上限
        /// </summary>
#else
        /// <summary>
        /// The upper limit for the response time to the request.
        /// </summary>
#endif
        int TimeLimit { get; }

#if JHELP
        /// <summary>
        /// プレイヤーの数
        /// </summary>
#else
        /// <summary>
        /// The number of players.
        /// </summary>
#endif
        int PlayerNum { get; }
    }
}
