[View in Japanese](https://github.com/AIWolfSharp/AIWolf_NET/blob/master/README.md)
# AIWolf.NET
## .NET version of AIWolf Library

AIWolf.NET is the AIWolf (artificial intelligence based werewolf) library for .NET.
The current version 1.0.8 is compatible with AIWolf platform version 0.4.x.

1. TUTORIALS (sorry, in Japanese.)

    1. For Visual Studio 2017.
        * [C#版人狼知能エージェントの作り方～Visual Studio編～（AIWolf.NET 1.0.x版）](https://www.slideshare.net/takots/c-59927842)
    1. For VIsual Studio Code.
        * [.NET CoreとVS Codeで作る人狼知能（AIWolf.NET 1.0.x版）](http://www.slideshare.net/takots/net-corevs-code-71808207)

1. DOWNLOADS

    * ClientStarter :
[ClientStarter-1.0.8.zip](https://github.com/AIWolfSharp/AIWolf_NET/releases/download/v1.0.8/ClientStarter-1.0.8.zip)

1. [REFERENCE MANUAL](https://github.com/AIWolfSharp/AIWolf_NET/releases/download/v1.0.7/AIWolf_NET_1.0.7_ReferenceManual_E.zip).

1. HISTORY and CHANGES

    * 1.0.0: The first release.
    * 1.0.1: Fix the following bugs of RequestContentBuilder.
      * "REQUEST(REQUEST(...))" can be generated.
      * The content, which is given as argument of the constructor, is modified .
    * 1.0.2: Fix AbstractRoleAssignPlayer's creating a new instance of agent every game.
This brings the great change of AbstractRoleAssignPlayer's usage.
    * 1.0.4: Rebuilt on .NET Standard library 1.4.
    * 1.0.6: Modifications for the coming AIWolfServer library.
      * Introduce enumeration type Team.
    * 1.0.7: Enable ClientStarter load assembly from multiple DLLs.
    * 1.0.8: Serializability introduced in 1.0.6 is removed because of
      its computational cost. This feature will be included in coming 1.1.x.
      In addition, make enum Request internal again
      because it is unrelated to agent.

---
This software is released under the MIT License, see [LICENSE](https://github.com/AIWolfSharp/AIWolf_NET/blob/master/LICENSE).
