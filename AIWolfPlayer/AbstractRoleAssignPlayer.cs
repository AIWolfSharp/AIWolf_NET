//
// AbstractRoleAssignPlayer.cs
//
// Copyright (c) 2017 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using AIWolf.Lib;

namespace AIWolf.Player.Lib
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
#if JHELP
        /// <summary>
        /// 村人プレイヤー
        /// </summary>
#else
        /// <summary>
        /// Villager player.
        /// </summary>
#endif
        protected IPlayer VillagerPlayer { get; set; } = new Sample.Villager();

#if JHELP
        /// <summary>
        /// 狩人プレイヤー
        /// </summary>
#else
        /// <summary>
        /// Bodyguard player.
        /// </summary>
#endif
        protected IPlayer BodyguardPlayer { get; set; } = new Sample.Bodyguard();

#if JHELP
        /// <summary>
        /// 占い師プレイヤー
        /// </summary>
#else
        /// <summary>
        /// Seer player.
        /// </summary>
#endif
        protected IPlayer SeerPlayer { get; set; } = new Sample.Seer();

#if JHELP
        /// <summary>
        /// 霊媒師プレイヤー
        /// </summary>
#else
        /// <summary>
        /// Medium player.
        /// </summary>
#endif
        protected IPlayer MediumPlayer { get; set; } = new Sample.Medium();

#if JHELP
        /// <summary>
        /// 裏切り者プレイヤー
        /// </summary>
#else
        /// <summary>
        /// Possessed player.
        /// </summary>
#endif
        protected IPlayer PossessedPlayer { get; set; } = new Sample.Possessed();

#if JHELP
        /// <summary>
        /// 人狼プレイヤー
        /// </summary>
#else
        /// <summary>
        /// Werewolf player.
        /// </summary>
#endif
        protected IPlayer WerewolfPlayer { get; set; } = new Sample.Werewolf();

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
        public string Name => GetType().ToString();

#if JHELP
        /// <summary>
        /// AbstractRoleAssignPlayerの新しいインスタンスを初期化する
        /// </summary>
#else
        /// <summary>
        /// Initializes a new instance of AbstractRoleAssignPlayer.
        /// </summary>
#endif
        public AbstractRoleAssignPlayer()
        {
            SetPlayers();
        }

#if JHELP
        /// <summary>
        /// 各役職用のプレイヤーエージェントを設定する
        /// </summary>
        /// <example>
        /// 例えば村人エージェントとしてMyVillagerクラスを使うには
        /// <code>
        /// VillagerPlayer = new MyVillager();
        /// </code>
        /// </example>
        /// <remarks>
        /// ここで設定しなかった役職についてはサンプルエージェントが使われる
        /// </remarks>
#else
        /// <summary>
        /// Set the player agent to each role.
        /// </summary>
        /// <example>
        /// To use MyVillager class for villager agent,
        /// <code>
        /// VillagerPlayer = new MyVillager();
        /// </code>
        /// </example>
        /// <remarks>
        /// If no agent is set to the role here, the sample agent will be used.
        /// </remarks>
#endif
        protected abstract void SetPlayers();

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
        public void Update(GameInfo gameInfo) => player.Update(gameInfo);

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
        public void Initialize(GameInfo gameInfo, GameSetting gameSetting)
        {
            switch (gameInfo.Role)
            {
                case Role.VILLAGER:
                    player = VillagerPlayer;
                    break;
                case Role.SEER:
                    player = SeerPlayer;
                    break;
                case Role.MEDIUM:
                    player = MediumPlayer;
                    break;
                case Role.BODYGUARD:
                    player = BodyguardPlayer;
                    break;
                case Role.POSSESSED:
                    player = PossessedPlayer;
                    break;
                case Role.WEREWOLF:
                    player = WerewolfPlayer;
                    break;
                default:
                    player = VillagerPlayer;
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
}
