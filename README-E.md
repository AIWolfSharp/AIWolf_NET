[View in Japanese](https://github.com/AIWolfSharp/AIWolf_NET/blob/master/README.md)
# AIWolf.NET
## .NET version of AIWolf Library

AIWolf.NET is the AIWolf (artificial intelligence based werewolf) library for .NET.
The current version 1.0.6 is compatible with AIWolf platform version 0.4.5.

1. TUTORIALS (sorry, in Japanese)

    1. Visual Studio 2017
        * [C#�Ől�T�m�\�G�[�W�F���g�̍����`Visual Studio�ҁ`�iAIWolf.NET 1.0.6�Łj](https://www.slideshare.net/takots/c-59927842)
    1. VIsual Studio Code
        * [.NET Core��VS Code�ō��l�T�m�\�iAIWolf.NET 1.0.6�Łj](http://www.slideshare.net/takots/net-corevs-code-71808207)

1. DOWNLOADS

    * ClientStarter :
[ClientStarter-1.0.6.zip](https://github.com/AIWolfSharp/AIWolf_NET/releases/download/v1.0.6/ClientStarter-1.0.6.zip)

1. [REFERENCE MANUAL](https://github.com/AIWolfSharp/AIWolfCore/releases/download/v1.0.2/AIWolf_NET_ReferenceManual.zip).

1. HISTORY and CHANGES

    * 1.0.0 : The first release.
    * 1.0.1 : Fix the following bugs of RequestContentBuilder.
      * "REQUEST(REQUEST(...))" can be generated.
      * The content, which is given as argument of the constructor, is modified .
    * 1.0.2 : Fix AbstractRoleAssignPlayer's creating a new instance of agent every game.
This brings the great change of AbstractRoleAssignPlayer's usage.
    * 1.0.4 : Rebuilt on .NET Standard library 1.4.
    * 1.0.6 : Modifications for the coming AIWolfServer library.
      * Introduce enumeration type Team.

---
This software is released under the MIT License, see [LICENSE](https://github.com/AIWolfSharp/AIWolf_NET/blob/master/LICENSE).
