//
// AbstractRoleAssignPlayer.cs
//
// Copyright (c) 2017 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

namespace AIWolf.Lib
{
#if JHELP
    /// <summary>
    /// 役職ごとに実際に使用するプレイヤーを切り替えるプレイヤーの抽象クラス
    /// </summary>
#else
    /// <summary>
    /// Abstract player class which switches player actually used according to its role.
    /// </summary>
#endif
    public abstract class AbstractRoleAssignPlayer : IPlayer
    {
        /// <summary>
        /// Dummy player.
        /// </summary>
        static readonly IPlayer dummyPlayer = new DummyPlayer();

#if JHELP
        /// <summary>
        /// 村人プレイヤー
        /// </summary>
        /// <remarks>nullの場合ダミープレイヤーが使われる</remarks>
#else
        /// <summary>
        /// Villager player.
        /// </summary>
        /// <remarks>If this is null, a dummy player is used.</remarks>
#endif
        protected abstract IPlayer VillagerPlayer { get; }

#if JHELP
        /// <summary>
        /// 狩人プレイヤー
        /// </summary>
        /// <remarks>nullの場合ダミープレイヤーが使われる</remarks>
#else
        /// <summary>
        /// Bodyguard player.
        /// </summary>
        /// <remarks>If this is null, a dummy player is used.</remarks>
#endif
        protected abstract IPlayer BodyguardPlayer { get; }

#if JHELP
        /// <summary>
        /// 占い師プレイヤー
        /// </summary>
        /// <remarks>nullの場合ダミープレイヤーが使われる</remarks>
#else
        /// <summary>
        /// Seer player.
        /// </summary>
        /// <remarks>If this is null, a dummy player is used.</remarks>
#endif
        protected abstract IPlayer SeerPlayer { get; }

#if JHELP
        /// <summary>
        /// 霊媒師プレイヤー
        /// </summary>
        /// <remarks>nullの場合ダミープレイヤーが使われる</remarks>
#else
        /// <summary>
        /// Medium player.
        /// </summary>
        /// <remarks>If this is null, a dummy player is used.</remarks>
#endif
        protected abstract IPlayer MediumPlayer { get; }

#if JHELP
        /// <summary>
        /// 裏切り者プレイヤー
        /// </summary>
        /// <remarks>nullの場合ダミープレイヤーが使われる</remarks>
#else
        /// <summary>
        /// Possessed player.
        /// </summary>
        /// <remarks>If this is null, a dummy player is used.</remarks>
#endif
        protected abstract IPlayer PossessedPlayer { get; }

#if JHELP
        /// <summary>
        /// 人狼プレイヤー
        /// </summary>
        /// <remarks>nullの場合ダミープレイヤーが使われる</remarks>
#else
        /// <summary>
        /// Werewolf player.
        /// </summary>
        /// <remarks>If this is null, a dummy player is used.</remarks>
#endif
        protected abstract IPlayer WerewolfPlayer { get; }

        IPlayer player;

#if JHELP
        /// <summary>
        /// プレイヤー名
        /// </summary>
#else
        /// <summary>
        /// This player's name.
        /// </summary>
#endif
        public abstract string Name { get; }

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
        public void Update(IGameInfo gameInfo) => player.Update(gameInfo);

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
        public void Initialize(IGameInfo gameInfo, IGameSetting gameSetting)
        {
            switch (gameInfo.Role)
            {
                case Role.VILLAGER:
                    player = VillagerPlayer ?? dummyPlayer;
                    break;
                case Role.SEER:
                    player = SeerPlayer ?? dummyPlayer;
                    break;
                case Role.MEDIUM:
                    player = MediumPlayer ?? dummyPlayer;
                    break;
                case Role.BODYGUARD:
                    player = BodyguardPlayer ?? dummyPlayer;
                    break;
                case Role.POSSESSED:
                    player = PossessedPlayer ?? dummyPlayer;
                    break;
                case Role.WEREWOLF:
                    player = WerewolfPlayer ?? dummyPlayer;
                    break;
                default:
                    player = dummyPlayer;
                    break;
            }

            player.Initialize(gameInfo, gameSetting);
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
        public void DayStart() => player.DayStart();

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
        public string Talk() => player.Talk();

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
        public string Whisper() => player.Whisper();

#if JHELP
        /// <summary>
        /// このプレイヤーが追放したいエージェントを返す
        /// </summary>
        /// <returns>このプレイヤーが追放したいエージェント</returns>
        /// <remarks>nullを返した場合エージェントはランダムに決められる</remarks>
#else
        /// <summary>
        /// Returns the agent this player wants to execute.
        /// </summary>
        /// <returns>The agent this player wants to execute.</returns>
        /// <remarks>Null results in random vote.</remarks>
#endif
        public Agent Vote() => player.Vote();

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
        public Agent Attack() => player.Attack();

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
        public Agent Divine() => player.Divine();

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
        public Agent Guard() => player.Guard();

#if JHELP
        /// <summary>
        /// ゲーム終了時に呼ばれる
        /// </summary>
        /// <remarks>このメソッドが呼ばれる前に，ゲーム情報は補完される（役職公開）</remarks>
#else
        /// <summary>
        /// Called when the game finishes.
        /// </summary>
        /// <remarks>Before this method is called, the game information is updated with all information.</remarks>
#endif
        public void Finish() => player.Finish();
    }

#if JHELP
    /// <summary>
    /// ダミープレイヤー
    /// </summary>
#else
    /// <summary>
    /// Dummy player.
    /// </summary>
#endif
    public class DummyPlayer : IPlayer
    {
#if JHELP
        /// <summary>
        /// プレイヤー名
        /// </summary>
#else
        /// <summary>
        /// This player's name.
        /// </summary>
#endif
        public string Name => "Dummy";

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
        public Agent Attack() => null;

#if JHELP
        /// <summary>
        /// 新しい日が始まるときに呼ばれる
        /// </summary>
#else
        /// <summary>
        /// Called when the day started.
        /// </summary>
#endif
        public void DayStart() { }

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
        public Agent Divine() => null;

#if JHELP
        /// <summary>
        /// ゲーム終了時に呼ばれる
        /// </summary>
        /// <remarks>このメソッドが呼ばれる前に，ゲーム情報は補完される（役職公開）</remarks>
#else
        /// <summary>
        /// Called when the game finishes.
        /// </summary>
        /// <remarks>Before this method is called, the game information is updated with all information.</remarks>
#endif
        public void Finish() { }

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
        public Agent Guard() => null;

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
        public void Initialize(IGameInfo gameInfo, IGameSetting gameSetting) { }

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
        public string Talk() => Lib.Talk.OVER;

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
        public void Update(IGameInfo gameInfo) { }

#if JHELP
        /// <summary>
        /// このプレイヤーが追放したいエージェントを返す
        /// </summary>
        /// <returns>このプレイヤーが追放したいエージェント</returns>
        /// <remarks>nullを返した場合エージェントはランダムに決められる</remarks>
#else
        /// <summary>
        /// Returns the agent this player wants to execute.
        /// </summary>
        /// <returns>The agent this player wants to execute.</returns>
        /// <remarks>Null results in random vote.</remarks>
#endif
        public Agent Vote() => null;

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
        public string Whisper() => Lib.Talk.OVER;
    }

}
