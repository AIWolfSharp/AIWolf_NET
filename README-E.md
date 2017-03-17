[View in Japanese](https://github.com/AIWolfSharp/AIWolf_NET/blob/master/README.md)
# AIWolf.NET
## .NET version of AIWolf Library

AIWolf.NET is the AIWolf (artificial intelligence based werewolf) library for .NET.
The current version 1.0.4 is compatible with AIWolf platform version 0.4.4.
This version is build on .NET Standard library 1.4
in order that it can [commonly used](https://docs.microsoft.com/en-us/dotnet/articles/standard/library)
by .NET Core agent and .NET Framework (4.6.1 and above) agent.


1. TUTORIALS

    1. .NET Framework
        * Under construction.
    1. .NET Core
        * [Here](http://www.slideshare.net/takots/net-corevs-code-71808207) (sorry, in Japanese).

1. DOWNLOADS

    * Begin with building ClientStarter (recommended) :
[ClientStarter-1.0.4.zip](https://github.com/AIWolfSharp/AIWolf_NET/releases/download/v1.0.4/ClientStarter-1.0.4.zip)
    * Don't build ClientStarter (.NET Framework only) :
[AIWolf_NET-1.0.4.zip](https://github.com/AIWolfSharp/AIWolf_NET/releases/download/v1.0.4/AIWolf_NET-1.0.4.zip)


1. [REFERENCE MANUAL](https://github.com/AIWolfSharp/AIWolfCore/releases/download/v1.0.2/AIWolf_NET_ReferenceManual.zip).

1. HISTORY and CHANGES

    * 1.0.0 : The first release.
    * 1.0.1 : Fix the following bugs of RequestContentBuilder.
      * "REQUEST(REQUEST(...))" can be generated.
      * The content, which is given as argument of the constructor, is modified .
    * 1.0.2 : Fix AbstractRoleAssignPlayer's creating a new instance of agent every game.
This brings the great change of AbstractRoleAssignPlayer's usage.
    * 1.0.4 : Rebuilt on .NET Standard library 1.4.

---
This software is released under the MIT License, see [LICENSE](https://github.com/AIWolfSharp/AIWolf_NET/blob/master/LICENSE).
