* 1.0.0: The first release of library version.
* 1.1.0: The first release of platform version including game server.
  * Now we have a game server library and the server starter.
There remain the following features unimplemented.
    * Validation of the uttered text.
    * Limitation of the agent's response time for request.
  * Re-design the class sharing method between agent and server
to reduce computation cost.
  * GameInfo and GameSetting are no longer writable by agent
to avoid being destroyed accidentally.
  * We have GameStarter for launching the server and the agents at the same time
regardless of their kind such as Java, .NET, Python, etc.
* 2.0.0: Modify some APIs.
  * Create AIWolf.Lib.AbstractRoleAssignPlayer class.
  * Create the following interfaces.
    * AIWolf.Lib.IGameInfo
    * AIWolf.Lib.IGameSetting
    * AIWolf.Lib.IUtterance
  * On AIWolf.Lib.Content class,
    * Modify the types of the following properties.
      * `public IUtterance Utterance { get; }`
      * `public IList<Content> ContentList { get; }`
    * Make the following constructor invisible.
      * `internal Content(Content content)`
  * Make AIWolf.Lib.ContentBuilder class abstract.
  * On AIWolf.Lib.IPlayer interface,
    * Modify the types of the arguments in the following methods.
      * `void Update(IGameInfo gameinfo)`
      * `void Initialize(IGameInfo gameInfo, IGameSetting gameSetting)`
  * On AIWolf.Lib.ShuffleExtensions.Shuffle extension method,
    * Modify the type of the returned value.
      * `public static IList<T> Shuffle<T>(this IEnumerable<T> s)`