[View in Japanese](https://github.com/AIWolfSharp/AIWolf_NET/blob/master/README.md)
# AIWolf.NET
## .NET version of AIWolf Library

AIWolf.NET is the AIWolf (artificial intelligence based werewolf) library for .NET
which is compatible with AIWolf platform version 0.4.x.

1. TUTORIALS (sorry, in Japanese.)
    * [C#版人狼知能エージェントの作り方～Visual Studio編～（AIWolf.NET 1.0.x版）](https://www.slideshare.net/takots/c-59927842)
    * [.NET CoreとVS Codeで作る人狼知能（AIWolf.NET 1.0.x版）](http://www.slideshare.net/takots/net-corevs-code-71808207)

1. DOWNLOADS

    * ClientStarter:
      [ClientStarter-1.0.9.zip](https://github.com/AIWolfSharp/AIWolf_NET/releases/download/v1.0.9/ClientStarter-1.0.9.zip)
    * Reference manual:
      [AIWolf_NET_1.0.9_ReferenceManual_E.zip](https://github.com/AIWolfSharp/AIWolf_NET/releases/download/v1.0.9/AIWolf_NET_1.0.9_ReferenceManual_E.zip).

1. HISTORY and CHANGES

    * 1.0.0: The first release.
    * 1.0.1: Fix the following bugs of RequestContentBuilder.
      * "REQUEST(REQUEST(...))" can be generated.
      * The constructor unexpectedly modify the content given as an argument.
    * 1.0.2: Fix AbstractRoleAssignPlayer's creating a new instance of agent every game.
      This brings the change of AbstractRoleAssignPlayer's usage.
    * 1.0.4: Rebuilt on .NET Standard library 1.4.
    * 1.0.6: Modify many classes considering shared library between agent and server.
      As a result, enumeration type Team is introduced.
    * 1.0.7: Enable ClientStarter load assembly from multiple DLLs.
    * 1.0.8: Serializability introduced in 1.0.6 is removed from 1.0.x
      because of its computational cost.
    * 1.0.9: Restore GameSetting.GetDefaultGameSetting()
      which was removed at last update.

---
This software is released under the MIT License, see [LICENSE](https://github.com/AIWolfSharp/AIWolf_NET/blob/master/LICENSE).
