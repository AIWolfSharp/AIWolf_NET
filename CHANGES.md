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
* 2.0.0:
  * Modifications to APIs.
    * Create AIWolf.Lib.AbstractRoleAssignPlayer class.
    * Create the following interfaces.
      * AIWolf.Lib.IGameInfo
      * AIWolf.Lib.IGameSetting
      * AIWolf.Lib.IUtterance
    * Make Talk and Whisper implement IUtterance.
    * On AIWolf.Lib.Content class,
      * Change the types of the properties as follows.
        * `public IUtterance Utterance { get; }`
        * `public IList<Content> ContentList { get; }`
      * Make the copy constructor invisible.
      * Create class method below to remove subject part from the uttered text.  
        `public static string StripSubject(string input)`
    * On AIWolf.Lib.IPlayer interface, change the types of the arguments as follows.
        * `void Update(IGameInfo gameinfo)`
        * `void Initialize(IGameInfo gameInfo, IGameSetting gameSetting)`
    * On AIWolf.Lib.ShuffleExtensions.Shuffle extension method,
      change the type of the returned value as follows.  
      `public static IList<T> Shuffle<T>(this IEnumerable<T> s)`
  * For the compatibility with AIWolf protocol version 3,
    * Introduce Agent.ANY, Agent.NONE, Role.ANY and Species.ANY.
    * Introduce Topic.ATTACKED and Topic.VOTED.
    * Introduce Operator.INQUIRE, Operator.DAY, Operator.NOT and Operator.XOR.
    * Introduce Content.Day property.  
      `public int Day { get; }`
    * Create the following ContentBuilders.
      * AttackedContentBuilder
      * VotedContentBuilder
      * InquiryContentBuilder
      * BecauseContentBuilder
      * AndContentBuilder
      * OrContentBuilder
      * XorContentBuilder
      * NotContentBuilder
      * DayContentBuilder
    * In the existing ContentBuilders,
      introduce new constructors to specify the subjects of the utterances
      and make the existing constructors obsolete.
  * Sample player is not included in this version.
* 2.0.1: Bugfixes in Content class.
