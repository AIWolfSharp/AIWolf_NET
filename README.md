[View in English](https://github.com/AIWolfSharp/AIWolf_NET/blob/master/README-E.md)
# AIWolf.NET
## .NET版人狼知能ライブラリ
AIWolf.NETは.NET版人狼知能ライブラリです．
最新版はバージョン1.0.8で，人狼知能プラットフォームバージョン0.4.x互換です．

1. チュートリアル
    1. Visual Studio 2017 の場合
        * [C#版人狼知能エージェントの作り方～Visual Studio編～（AIWolf.NET 1.0.x版）](https://www.slideshare.net/takots/c-59927842)
    1. Visual Studio Code の場合
        * [.NET CoreとVisual Studio Codeで作る人狼知能（AIWolf.NET 1.0.x版）](https://www.slideshare.net/takots/net-corevs-code-71808207)

1. ダウンロード

    * クライアントスタータ：
[ClientStarter-1.0.8.zip](https://github.com/AIWolfSharp/AIWolf_NET/releases/download/v1.0.8/ClientStarter-1.0.8.zip)

1. リファレンスマニュアルは[こちら](https://github.com/AIWolfSharp/AIWolf_NET/releases/download/v1.0.7/AIWolf_NET_1.0.7_ReferenceManual_J.zip)
からダウンロードしてください．

1. 履歴と変更点

    * 1.0.0 : 最初のリリース
    * 1.0.1 : RequestContentBuilderのバグフィックス版です．修正されたバグは以下の通りです．
      * 入れ子になったリクエスト発話が生成可能
      * 引数として渡したContentが変更される
    * 1.0.2 : AbstractRoleAssignPlayerで各役職エージェントのインスタンスが毎回生成されていたのを修正しました．
それに伴ってAbstractRoleAssignPlayerの使用法が大きく変わりました．
    * 1.0.4 : .NET Standard 1.4ベースになりました．
    * 1.0.6 : 将来リリース予定のAIWolfServer（サーバ用ライブラリ）のための変更です．
      * 内部は大きく変わっているのですが，APIレベルでは変わっていません．
      * 列挙型Teamが新たに導入されました．
    * 1.0.7 : ClientStarterが複数DLLに対応できるように修正しました．
ライブラリには変更ありませんが，SHFBがようやく.NET Core/Standardに対応したので，リファレンスマニュアルがちゃんとしました．
    * 1.0.8 : 1.0.6でサーバ向けに実装したシリアライズ機能はコスト増につながることから，
      エージェント用ライブラリから切り離しました．サーバ機能は1.1.x以降に含まれる予定です．
      それに伴いエージェントと無関係なRequest列挙型は再びinternalに戻しました．

---
このソフトウェアは，MITライセンスのもとで公開されています．[LICENSE](https://github.com/AIWolfSharp/AIWolf_NET/blob/master/LICENSE)を参照のこと.
