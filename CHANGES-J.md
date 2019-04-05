* 1.0.0: 最初のリリース
* 1.1.0: ゲームサーバが加わり.NET版だけで完結するようになったのに伴い，
名称を「ライブラリ」から「プラットフォーム」に変えました．
  * ゲームサーバが加わりました．
ただし，発話文字列チェックと時間制限は実装していません．
  * 計算コスト削減のため，エージェント-サーバ間でのクラスの共用の方法を見直しました．
  * 意図しない書き換えによる誤動作を防止するため，
エージェントに渡されるGameInfo, GameSettingを書き換え不可にしました．
  * 各種クライアントスタータをプロセスとして起動するGameStarterが加わりました．
サーバ起動後，Java, .NET, Pythonなどのエージェントを接続して対戦することができます．
* 2.0.0:
  * APIの変更
    * 新規クラス  
      AIWolf.Lib.AbstractRoleAssignPlayer
    * 新規インターフェース
      * AIWolf.Lib.IGameInfo
      * AIWolf.Lib.IGameSetting
      * AIWolf.Lib.IUtterance
    * TalkクラスとWhisperクラスがIUtteranceインターフェースを実装
    * AIWolf.Lib.Contentクラス
      * プロパティの型変更
        * `public IUtterance Utterance { get; }`
        * `public IList<Content> ContentList { get; }`
      * コピーコンストラクタの不可視化
      * 発話テキストから主語を除く新規クラスメソッド  
        `public static string StripSubject(string input)`
    * AIWolf.Lib.IPlayerインターフェースのメソッド引数の型変更
      * `void Update(IGameInfo gameinfo)`
      * `void Initialize(IGameInfo gameInfo, IGameSetting gameSetting)`
    * AIWolf.Lib.ShuffleExtensions.Shuffle拡張メソッドの戻り値の型変更  
      `public static IList<T> Shuffle<T>(this IEnumerable<T> s)`
  * 人狼知能プロトコルバージョン3への対応
    * Agent.ANY, Agent.NONE, Role.ANY, Species.ANY新設
    * 新規トピックTopic.ATTACKED, Topic.VOTED
    * 新規演算子Operator.INQUIRE, Operator.DAY, Operator.NOT and Operator.XOR
    * ContentクラスにDayプロパティ新設  
      `public int Day { get; }`
    * 新規ContentBuilder
      * AttackedContentBuilder
      * VotedContentBuilder
      * InquiryContentBuilder
      * BecauseContentBuilder
      * AndContentBuilder
      * OrContentBuilder
      * XorContentBuilder
      * NotContentBuilder
      * DayContentBuilder
    * 既存のContentBuilderに主語を指定可能な新規コンストラクタ（既存のコンストラクタは非推奨）
  * このバージョンからサンプルプレイヤーは含まれません．
* 2.0.1: Contentクラスのバグフィックス
