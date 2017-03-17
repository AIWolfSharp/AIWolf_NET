# AIWolf.NET
## .NET version of AIWolf Library

AIWolf.NETとは，人狼知能プラットフォームのライブラリ群を.NET Framework 4.5用にC#で書き直したもので，
最新バージョンは0.2.2です．

1. クイックスタート

    1. AIWolf_NET-0.2.2.zipをダウンロードする

        [AIWolf_NET-0.2.2.zip](https://github.com/AIWolfSharp/AIWolf_NET/releases/download/v0.2.2/AIWolf_NET-0.2.2.zip)は，
        ライブラリ(AIWolfLibCommon.dll, AIWolfLibClient.dll)と
        クライアントスターター(ClientStarter.exe)
        そしてAPIリファレンス(html)をまとめたものです．
        本リリースから，Json.NET 9.0 Release 1 (Json90r1.zip)も同梱されています．
        ライブラリのソースファイルが必要でなければ，これをダウンロードするだけで
        人狼知能エージェントを作成することができます．
        なお，Windowsではダウンロードしたファイルがブロックされるため，そのままでは動作しない場合があります．
        その場合はコンテキストメニューの「プロパティ」よりブロックを解除してください．

    1. AIWolf_NET-0.2.2.zipを展開し，Newtonsoft.Json.dllを展開したフォルダーに置きます． 

    1. サンプルエージェントを試してみる

        [人狼知能プラットフォーム](http://aiwolf.org/server/)をダウンロードし，
        人狼知能サーバを起動しておきます．
        localhost上のポート10000番で接続を待っているサーバに
        サンプルエージェント(SampleRoleAssignPlayer)を接続するには，
        展開したフォルダーでコマンドプロンプトから以下のコマンドを実行します．
        この例では，コマンドラインオプション `-t 100` によって，
        エージェントのリクエスト処理時間の上限を100msに制限しています．

        `ClientStarter.exe -h localhost -p 10000 -t 100 -c AIWolf.Client.Base.Smpl.SampleRoleAssignPlayer AIWolfLibClient.dll`

    1. 独自のエージェントを作成

        エージェント作成のチュートリアルをご覧ください．
        * [C#版人狼知能エージェントの作り方（Visual Studio編）](http://www.slideshare.net/takots/c-59927842)
        * [C#版人狼知能エージェントの作り方(MonoDevelop/Xamarin Studio編)](http://www.slideshare.net/takots/cmonodevelopxamarin-studio)

1. 更新履歴

    * 0.1.0

        初期リリース

    * 0.2.0

        - エージェントが例外を投げて異常終了したとき，
        ClientStarter.exe が標準エラーストリームに例外のスタックトレースを出力するようになりました．

        - エージェントのリクエスト処理時間を制限するための `-t` オプションが ClientStarter.exe に導入されました．
        制限時間（デフォルトで100ms）を超過すると，エージェントは強制的に終了されます．

    * 0.2.1

        - ClientStarter.exe が `-t` オプションなしで起動された場合，リクエスト処理時間を制限しないようになりました．

        - Json.NET 9.0 Release 1 を同梱するようにしました．

    * 0.2.2

        - 例外処理を見直しました．

        - ClientStarter.exeが従来よりもより多くの例外に関する情報を出力するようになりました．

---
このソフトウェアは，MITライセンスのもとで公開されています．[LICENSE](https://github.com/AIWolfSharp/AIWolf_NET/blob/master/LICENSE)を参照のこと．
