[View in English](https://github.com/AIWolfSharp/AIWolf_NET/blob/master/README-E.md)
# AIWolf.NET
## .NET版人狼知能ライブラリ
AIWolf.NETは.NET版人狼知能ライブラリです．
最新版はバージョン1.0.4で，人狼知能プラットフォームバージョン0.4.4互換です．
このバージョンから.NET Standard 1.4ベースになり，.NET Coreと.NET Framework（4.6.1以上）間で
[共用可能](https://docs.microsoft.com/ja-jp/dotnet/articles/standard/library)
になりました．

1. チュートリアル
    1. .NET Frameworkの場合
        * 現在準備中です．
    1. .NET Coreの場合
        * [.NET CoreとVisual Studio Codeで作る人狼知能](https://www.slideshare.net/takots/net-corevs-code-71808207)
        をご覧ください．

1. リファレンスマニュアルは[こちら](https://github.com/AIWolfSharp/AIWolfCore/releases/download/v1.0.2/AIWolf_NET_ReferenceManual.zip)
からダウンロードしてください．

1. 履歴と変更点

    * 1.0.0 : 最初のリリース
    * 1.0.1 : RequestContentBuilderのバグフィックス版です．修正されたバグは以下の通りです．
      * 入れ子になったリクエスト発話が生成可能
      * 引数として渡したContentが変更される
    * 1.0.2 : AbstractRoleAssignPlayerで各役職エージェントのインスタンスが毎回生成されていたのを修正しました．
それに伴ってAbstractRoleAssignPlayerの使用法が大きく変わりました．
    * 1.0.4 : .NET Standard 1.4ベースになりました．
      

---
このソフトウェアは，MITライセンスのもとで公開されています．[LICENSE](https://github.com/AIWolfSharp/AIWolf_NET/blob/master/LICENSE)を参照のこと.
