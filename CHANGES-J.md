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
* 2.0.0: APIの変更
  * 新規クラス
    * AIWolf.Lib.AbstractRoleAssignPlayer
  * 新規インターフェース
    * AIWolf.Lib.IGameInfo
    * AIWolf.Lib.IGameSetting
    * AIWolf.Lib.IUtterance
  * AIWolf.Lib.Contentクラス
    * プロパティの型変更
      * `public IUtterance Utterance { get; }`
      * `public IList<Content> ContentList { get; }`
    * コンストラクタの不可視化
      * `internal Content(Content content)`
  * AIWolf.Lib.ContentBuilderクラスの抽象化
  * AIWolf.Lib.IPlayerインターフェース
    * メソッド引数の型変更
      * `void Update(IGameInfo gameinfo)`
      * `void Initialize(IGameInfo gameInfo, IGameSetting gameSetting)`
  * AIWolf.Lib.ShuffleExtensions.Shuffle拡張メソッド
    * 戻り値の型変更
      * `public static IList<T> Shuffle<T>(this IEnumerable<T> s)`