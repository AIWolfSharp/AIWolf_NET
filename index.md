[View in English](https://github.com/AIWolfSharp/AIWolf_NET/blob/master/README-E.md)
# AIWolf.NET
## .NET版人狼知能ライブラリ
AIWolf.NETは人狼知能プラットフォームバージョン 0.4.x 互換の .NET 版人狼知能ライブラリです．

1. チュートリアル

    * [C#版人狼知能エージェントの作り方～Visual Studio編～（AIWolf.NET 1.0.x版）](https://www.slideshare.net/takots/c-59927842)
    * [.NET CoreとVisual Studio Codeで作る人狼知能（AIWolf.NET 1.0.x版）](https://www.slideshare.net/takots/net-corevs-code-71808207)

1. ダウンロード

    * クライアントスタータ: 
      [ClientStarter-1.0.9.zip](https://github.com/AIWolfSharp/AIWolf_NET/releases/download/v1.0.9/ClientStarter-1.0.9.zip)
    * リファレンスマニュアル: 
      [AIWolf_NET_1.0.9_ReferenceManual_J.zip](https://github.com/AIWolfSharp/AIWolf_NET/releases/download/v1.0.9/AIWolf_NET_1.0.9_ReferenceManual_J.zip)

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

---
このソフトウェアは，MITライセンスのもとで公開されています．[LICENSE](https://github.com/AIWolfSharp/AIWolf_NET/blob/master/LICENSE)を参照のこと.
