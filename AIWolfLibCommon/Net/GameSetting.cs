using AIWolf.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace AIWolf.Common.Net
{
    /// <summary>
    /// Settings of game.
    /// </summary>
    /// <remarks></remarks>
    [DataContract]
    public class GameSetting : ICloneable
    {
        /// <summary>
        /// Number of each roles.
        /// </summary>
        /// <remarks>
        /// Order: bodyguard, freemason, medium, possessed, seer, villager, werewolf.
        /// </remarks>
        static readonly int[][] roleNumArray =
        {
            null,//0
            null,//1
            null,//2
            null,//3
            new[] {0, 0, 0, 0, 1, 2, 1 },//4
            new[] {0, 0, 0, 1, 1, 2, 1 },//5
            new[] {0, 0, 0, 1, 1, 3, 1 },//6
            new[] {0, 0, 0, 0, 1, 4, 2 },//7
            new[] {0, 0, 1, 0, 1, 4, 2 },//8
            new[] {0, 0, 1, 0, 1, 5, 2 },//9
            new[] {1, 0, 1, 1, 1, 4, 2 },//10
            new[] {1, 0, 1, 1, 1, 5, 2 },//11
            new[] {1, 0, 1, 1, 1, 5, 3 },//12
            new[] {1, 0, 1, 1, 1, 6, 3 },//13
            new[] {1, 0, 1, 1, 1, 7, 3 },//14
            new[] {1, 0, 1, 1, 1, 8, 3 },//15
            new[] {1, 0, 1, 1, 1, 9, 3 },//16
            new[] {1, 0, 1, 1, 1, 10, 3 },//17
            new[] {1, 0, 1, 1, 1, 11, 3 },//18
        };

        /// <summary>
        /// The setting for the seminar.
        /// </summary>
        /// <remarks>
        /// 1 bodyguard, 1 seer, 8 villagers, and 3 werewolves.
        /// </remarks>
        static readonly int[] seminarArray =
        {
            1, 0, 0, 0, 1, 8, 3
        };

        /// <summary>
        /// Default setting of game.
        /// </summary>
        /// <param name="agentNum">The number of agents.</param>
        /// <returns>Default setting of game with given number of agents.</returns>
        /// <remarks></remarks>
        public static GameSetting GetDefaultGame(int agentNum)
        {
            if (agentNum < 5)
            {
                throw new ArgumentOutOfRangeException("agentNum", "agentNum must be bigger than 5 but " + agentNum);
            }
            if (agentNum > roleNumArray.Length)
            {
                throw new ArgumentOutOfRangeException("agentNum", "agentNum must be smaller than " + (roleNumArray.Length + 1) + " but " + agentNum);
            }

            GameSetting setting = new GameSetting();
            setting.MaxTalk = 10;
            setting.EnableNoAttack = false;
            setting.VoteVisible = true;
            setting.VotableInFirstDay = false;

            Role[] roles = (Role[])Enum.GetValues(typeof(Role));
            for (int i = 0; i < roles.Length; i++)
            {
                setting.RoleNumMap[roles[i]] = roleNumArray[agentNum][i];
            }
            return setting;
        }

        /// <summary>
        /// The setting of the game for the seminar.
        /// </summary>
        /// <returns>GameSetting for the seminar.</returns>
        /// <remarks></remarks>
        public static GameSetting GetSeminarGame()
        {
            GameSetting setting = new GameSetting();
            setting.MaxTalk = 10;
            setting.EnableNoAttack = false;
            setting.VoteVisible = true;

            Role[] roles = (Role[])Enum.GetValues(typeof(Enum));
            for (int i = 0; i < roles.Length; i++)
            {
                setting.RoleNumMap[roles[i]] = seminarArray[i];
            }
            return setting;
        }

        /// <summary>
        /// The number of each role.
        /// </summary>
        /// <value>Dictionary storing the number of each role.</value>
        /// <remarks></remarks>
        [DataMember(Name = "roleNumMap")]
        public Dictionary<Role, int> RoleNumMap { get; set; }

        /// <summary>
        /// The maximum number of talks.
        /// </summary>
        /// <value>The maximum number of talks.</value>
        /// <remarks></remarks>
        [DataMember(Name = "maxTalk")]
        public int MaxTalk { get; set; }

        /// <summary>
        /// Whether or not the game permit to attack no one.
        /// </summary>
        /// <value>True if the game permit to attack no one, otherwise, false.</value>
        /// <remarks></remarks>
        [DataMember(Name = "enableNoAttack")]
        public bool EnableNoAttack { get; set; }

        /// <summary>
        /// Whether or not agent can see who vote to who.
        /// </summary>
        /// <value>True if agent can see who vote to who, otherwise, false.</value>
        /// <remarks></remarks>
        [DataMember(Name = "voteVisible")]
        public bool VoteVisible { get; set; }

        /// <summary>
        /// Whether or not there is vote in the first day.
        /// </summary>
        /// <value>True if there is vote in the first day, otherwise, false.</value>
        /// <remarks></remarks>
        [DataMember(Name = "votableInFirstDay")]
        public bool VotableInFirstDay { get; private set; }

        /// <summary>
        /// The random seed.
        /// </summary>
        /// <value>The random seed.</value>
        /// <remarks></remarks>
        [DataMember(Name = "randomSeed")]
        public long RandomSeed { get; set; } = Environment.TickCount;

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <remarks></remarks>
        public GameSetting()
        {
            RoleNumMap = new Dictionary<Role, int>();
        }

        /// <summary>
        /// The number of players.
        /// </summary>
        /// <value>The number of players.</value>
        /// <remarks></remarks>
        [DataMember(Name = "playerNum")]
        public int PlayerNum
        {
            get
            {
                return RoleNumMap.Values.Sum();
            }
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of the current instance.</returns>
        /// <remarks></remarks>
        public object Clone()
        {
            GameSetting gameSetting = new GameSetting();
            gameSetting.EnableNoAttack = EnableNoAttack;
            gameSetting.VotableInFirstDay = VotableInFirstDay;
            gameSetting.VoteVisible = VoteVisible;
            gameSetting.MaxTalk = MaxTalk;
            gameSetting.RandomSeed = RandomSeed;
            gameSetting.RoleNumMap = new Dictionary<Role, int>(RoleNumMap);
            return gameSetting;
        }
    }
}
