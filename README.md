[View in English](https://github.com/AIWolfSharp/AIWolf_NET/blob/master/README-E.md)
# AIWolf.NET
## .NET版人狼知能プラットフォーム
AIWolf.NETは.NET版人狼知能プラットフォームです．
ゲームの構成と発話プロトコルは公式の人狼知能プラットフォーム互換となっています．

1. チュートリアル（バージョン1.0.x用です）
    1. Visual Studio 2017 の場合
        * [C#版人狼知能エージェントの作り方～Visual Studio編～（AIWolf.NET 1.0.x版）](https://www.slideshare.net/takots/c-59927842)
    1. Visual Studio Code の場合
        * [.NET CoreとVisual Studio Codeで作る人狼知能（AIWolf.NET 1.0.x版）](https://www.slideshare.net/takots/net-corevs-code-71808207)

1. ダウンロード

    * クライアントスタータ：
[ClientStarter-2.0.0.zip](https://github.com/AIWolfSharp/AIWolf_NET/releases/download/v2.0.0/ClientStarter-2.0.0.zip)
    * サーバスタータ：
[ServerStarter-2.0.0.zip](https://github.com/AIWolfSharp/AIWolf_NET/releases/download/v2.0.0/ServerStarter-2.0.0.zip)
    * ゲームスタータ：
[GameStarter-2.0.0.zip](https://github.com/AIWolfSharp/AIWolf_NET/releases/download/v2.0.0/GameStarter-2.0.0.zip)
    * リファレンスマニュアル：[AIWolf_NET_2.0.0_ReferenceManual_J.zip](https://github.com/AIWolfSharp/AIWolf_NET/releases/download/v2.0.0/AIWolf_NET_2.0.0_ReferenceManual_J.zip)

1. 履歴と変更点

    * 1.0.0: 最初のリリース
    * 1.0.1: RequestContentBuilderのバグフィックス版です．
      修正されたバグは以下の通りです．
      * 入れ子になったリクエスト発話が生成可能
      * 引数として渡したContentが変更される
    * 1.0.2: AbstractRoleAssignPlayerで各役職エージェントのインスタンスが
      毎回生成されていたのを修正しました．
      それに伴ってAbstractRoleAssignPlayerの使用法が大きく変わりました．
    * 1.0.4: .NET Standard 1.4ベースになりました．
    * 1.0.6: 将来リリース予定のAIWolfServer（サーバ用ライブラリ）のための変更です．
      * 内部は大きく変わっているのですが，APIレベルでは変わっていません．
      * 列挙型Teamが新たに導入されました．
    * 1.0.7: ClientStarterが複数DLLに対応できるように修正しました．
      ライブラリには変更ありませんが，SHFB(Sandcastle Help File Builder)が
      ようやく.NET Core/Standardに対応したので，
      リファレンスマニュアルが普通に生成できるようになりました．
    * 2.0.0: ゲームサーバが加わり.NET版だけで完結するようになったのに伴い，
      名称を「ライブラリ」から「プラットフォーム」に変えました．
      * ゲームサーバが加わりました．
        ただし，発話文字列チェックと時間制限は実装していません．
      * 計算コスト削減のため，エージェント-サーバ間でのクラスの共用の方法を見直しました．
      * 意図しない書き換えによる誤動作を防止するため，
        エージェントに渡されるGameInfo, GameSettingを書き換え不可にしました．
      * 各種クライアントスタータをプロセスとして起動するGameStarterが加わりました．
        サーバ起動後，Java, .NET, Pythonなどのエージェントを接続して対戦することができます．
      * Contentクラスの変更点
        * コピーコンストラクタの不可視化
        * ConentListをreadonlyのIListに
      * GameInfoクラスの変更点
        * IGameInfoインターフェースの実装
        * プロパティのsetterを廃止し，ListをreadonlyのIListに，DictionaryをreadonlyのIDictionaryに
        * コンストラクタの不可視化
      * GameSettingクラスの変更点
        * IGameSettingインターフェースの実装
        * プロパティのsetterを廃止し，RoleNumMapをreadonlyのIDictionaryに
      * IPlayerインターフェースの変更点
        * GameInfo, GameSettingをIGameInfo, IGameSettingに      

---
このソフトウェアは，MITライセンスのもとで公開されています．[LICENSE](https://github.com/AIWolfSharp/AIWolf_NET/blob/master/LICENSE)を参照のこと.
