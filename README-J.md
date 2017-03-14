# AIWolf.NET Core
## .NET Core版人狼知能ライブラリ
AIWolf.NET Coreは人狼知能プラットフォーム0.4.x互換の.NET Core版人狼知能ライブラリです．
最新版はバージョン1.0.2です．

1. クイックスタート
    1. .NET Core SDK 1.0.3をインストールしてください．
    2. AIWolf.NET CoreにはAIWolfLibとAIWolfPlayerの2つのライブラリがあるのですが，
NuGetパッケージマネージャが管理しますので，インストール作業は不要です．
    3. AIWolf.NET Coreには自前のゲームサーバがありませんので，サーバを動かすために最新の
[人狼知能プラットフォーム](http://aiwolf.org/server/)をダウンロードしてください．
    4. クライアントエージェントを起動するClientStarterの実行ファイルは配布していませんので，
まずはClientStarterをビルドするところから始めましょう．
       1. [ClientStarter-1.0.2.zip](https://github.com/AIWolfSharp/AIWolfCore/releases/download/v1.0.2/ClientStarter-1.0.2.zip)
をダウンロードして展開するとClientStarterフォルダができますので，お好きな場所に置いてください．
       2. ClientStarterフォルダで以下のコマンドを実行してください．  
`dotnet restore`  
`dotnet build`
    5. ビルドに成功したら，以下のコマンドでサンプルエージェントが10000番ポートで接続待ちのゲームサーバに接続できるか確認しましょう．  
`dotnet run -d`

    6. 自分でエージェントを作るには
      * [リファレンスマニュアル](https://github.com/AIWolfSharp/AIWolfCore/releases/download/v1.0.2/AIWolf_NET_ReferenceManual.zip) が役に立つでしょう．
      * [.NET CoreとVS Codeで作る人狼知能](http://www.slideshare.net/takots/net-corevs-code-71808207)をご覧ください．
1. 履歴と変更点

    * 1.0.0-prerelease2 : 最初の公開版
    * 1.0.0-prerelease3 : IEnumerableの拡張メソッドShuffle()は結構使われそうなので，
名前空間AIWolf.Player.SampleからAIWolf.Libに移動しました．
    * 1.0.0-prerelease4 : Portable PDBを生成するようにproject.jsonを修正しました．
VS Codeでデバッグが可能になります．
    * 1.0.0 : 正式リリース．prerelease4と同等です．
    * 1.0.1 : RequestContentBuilderのバグフィックス版です．修正されたバグは以下の通りです．
      * 入れ子になったリクエスト発話が生成可能
      * 引数として渡したContentが変更される
    * 1.0.2 : AbstractRoleAssignPlayerで各役職エージェントのインスタンスが毎回生成されていたのを修正しました．
それに伴ってAbstractRoleAssignPlayerの使用法が大きく変わりました．
      

---
このソフトウェアは，MITライセンスのもとで公開されています．[LICENSE](https://github.com/AIWolfSharp/AIWolf_NET/blob/master/LICENSE)を参照のこと.
