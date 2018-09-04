[View in English](https://github.com/AIWolfSharp/AIWolf_NET/blob/v1.1.2/README-E.md)
# AIWolf.NET
## .NET版人狼知能プラットフォーム

1. チュートリアル

    * [C#版人狼知能エージェントの作り方～Visual Studio編～（AIWolf.NET 1.0.x版）](https://www.slideshare.net/takots/c-59927842)
    * [.NET CoreとVisual Studio Codeで作る人狼知能（AIWolf.NET 1.0.x版）](https://www.slideshare.net/takots/net-corevs-code-71808207)

1. ダウンロード

    * クライアントスタータ: 
      [ClientStarter-1.1.2.zip](https://github.com/AIWolfSharp/AIWolf_NET/releases/download/v1.1.2/ClientStarter-1.1.2.zip)
    * サーバスタータ：
      [ServerStarter-1.1.2.zip](https://github.com/AIWolfSharp/AIWolf_NET/releases/download/v1.1.2/ServerStarter-1.1.2.zip)
    * ゲームスタータ：
      [GameStarter-1.1.2.zip](https://github.com/AIWolfSharp/AIWolf_NET/releases/download/v1.1.2/GameStarter-1.1.2.zip)
    * リファレンスマニュアル: 
      [AIWolf_NET_1.1.2_ReferenceManual_J.zip](https://github.com/AIWolfSharp/AIWolf_NET/releases/download/v1.1.2/AIWolf_NET_1.1.2_ReferenceManual_J.zip)

1. 履歴と変更点

    * 1.0.0: 最初のリリース．
    * 1.0.1: RequestContentBuilderの以下のバグを修正しました．
      * 入れ子になったリクエスト発話が生成可能
      * 引数として渡したContentが変更される
    * 1.0.2: AbstractRoleAssignPlayerで
    各役職エージェントのインスタンスが毎回生成されていたのを修正したのに伴い，
    AbstractRoleAssignPlayerの使用法も変わりました．
    * 1.0.4: .NET Standard 1.4ベースになりました．
    * 1.0.6: サーバ側との共用を視野に入れた変更を加えました．
      それに伴い列挙型Teamが新たに導入されました．
    * 1.0.7: ClientStarter が複数の
      DLL からなるエージェントを起動できるようになりました．
    * 1.0.8: 1.0.6で実装したシリアライズ機能は，
      処理量の増加のおそれがあるためバージョン 1.0.x
      系列では実装しないことにしました．
    * 1.0.9: 1.0.8 で削除した GameSetting.GetDefaultGameSetting()
      を復活させました．
    * 1.1.0: ゲームサーバが加わり.NET版だけで完結するようになったのに伴い，
      名称を「ライブラリ」から「プラットフォーム」に変えました．
      * 公式ゲームサーバと異なり，発話文字列チェックと時間制限は実装していません．
      * ServerStarterはゲームサーバ単体を起動します．
      * GameStarterは各種クライアントスタータをプロセスとして起動しますので，
        サーバ起動後，Java, .NET, Pythonなどのエージェントを接続して対戦することができます．
    * 1.1.1: 各種ContentBuilderでtargeがnullの場合の処理を変更しました．
    * 1.1.2: AbstractRoleAssignPlayer.Nameプロパティをvirtualにしました．
      

---
このソフトウェアは，MITライセンスのもとで公開されています．[LICENSE](https://github.com/AIWolfSharp/AIWolf_NET/blob/v1.1.2/LICENSE)を参照のこと.
