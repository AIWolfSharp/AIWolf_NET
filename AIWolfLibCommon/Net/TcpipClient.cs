using AIWolf.Common.Data;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace AIWolf.Common.Net
{
    /// <summary>
    /// AIWolf client using TCP/IP connection.
    /// </summary>
    /// <remarks></remarks>
    public class TcpipClient : IGameClient
    {
        string host;
        int port;

        TcpClient tcpClient;

        IPlayer player;

        /// <summary>
        /// The requested role.
        /// </summary>
        /// <value>The requested role.</value>
        /// <remarks></remarks>
        public Role? RequestRole { get; set; }

        /// <summary>
        /// Whether or not this client is running.
        /// </summary>
        /// <value>True if this client is running, otherwise, false.</value>
        /// <remarks></remarks>
        public bool Running { get; private set; }

        GameInfo lastGameInfo;

        /// <summary>
        /// The name of player this client manages.
        /// </summary>
        /// <value>The name of player this client manages.</value>
        /// <remarks></remarks>
        public string PlayerName { get; set; }

        /// <summary>
        /// The number of milliseconds to wait for the request call.
        /// </summary>
        /// <value>The number of milliseconds.</value>
        /// <remarks></remarks>
        public int Timeout { get; set; } = -1; // Do not limit by default.

        /// <summary>
        /// Initializes a new instance of this class which connects given port of given host.
        /// </summary>
        /// <param name="host">Hostname this client connects.</param>
        /// <param name="port">Port number this client connects.</param>
        /// <remarks></remarks>
        public TcpipClient(string host, int port)
        {
            this.host = host;
            this.port = port;
            Running = false;
        }

        /// <summary>
        /// Initializes a new instance of this class which connects given port of given host, and requests given role.
        /// </summary>
        /// <param name="host">Hostname this client connects.</param>
        /// <param name="port">Port number this client connects.</param>
        /// <param name="requestRole">Role this client requests.</param>
        /// <remarks></remarks>
        public TcpipClient(string host, int port, Role? requestRole) : this(host, port)
        {
            RequestRole = requestRole;
        }

        /// <summary>
        /// Connects the player to the server.
        /// </summary>
        /// <param name="player">The player to be connected.</param>
        /// <remarks></remarks>
        public void Connect(IPlayer player)
        {
            this.player = player;

            tcpClient = new TcpClient();
            tcpClient.Connect(Dns.GetHostAddresses(host), port);

            try
            {
                StreamReader sr = new StreamReader(tcpClient.GetStream());
                StreamWriter sw = new StreamWriter(tcpClient.GetStream());
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Packet packet = DataConverter.GetInstance().ToPacket(line);

                    object obj = Recieve(packet);
                    if (packet.Request.HasReturn())
                    {
                        if (obj == null)
                        {
                            sw.WriteLine();
                        }
                        else if (obj is string)
                        {
                            sw.WriteLine(obj);
                        }
                        else
                        {
                            sw.WriteLine(DataConverter.GetInstance().Convert(obj));
                        }
                        sw.Flush();
                    }
                }
            }
            catch (IOException e)
            {
                if (Running)
                {
                    throw e;
                }
            }
            finally
            {
                Running = false;
            }
        }

        /// <summary>
        /// Receives a packet from the server and sends it to the player, and returns what returned from the player.
        /// </summary>
        /// <param name="packet">The packet from the server.</param>
        /// <returns>The object returned from the player.</returns>
        /// <remarks></remarks>
        public object Recieve(Packet packet)
        {
            GameInfo gameInfo = lastGameInfo;
            GameSetting gameSetting = packet.GameSetting;

            if (packet.GameInfo != null)
            {
                gameInfo = packet.GameInfo.ToGameInfo();
                lastGameInfo = gameInfo;
            }

            if (packet.TalkHistory != null)
            {
                Talk lastTalk = null;
                if (gameInfo.TalkList != null && gameInfo.TalkList.Count != 0)
                {
                    lastTalk = gameInfo.TalkList[gameInfo.TalkList.Count - 1];
                }
                foreach (var talk in packet.TalkHistory)
                {
                    if (IsAfter(talk, lastTalk))
                    {
                        gameInfo.TalkList.Add(talk.ToTalk());
                    }
                }
            }

            if (packet.WhisperHistory != null)
            {
                Talk lastWhisper = null;
                if (gameInfo.WhisperList != null && gameInfo.WhisperList.Count != 0)
                {
                    lastWhisper = gameInfo.WhisperList[gameInfo.WhisperList.Count - 1];
                }
                foreach (var whisper in packet.WhisperHistory)
                {
                    if (IsAfter(whisper, lastWhisper))
                    {
                        gameInfo.WhisperList.Add(whisper.ToTalk());
                    }
                }
            }

            Task<object> task = Task.Run(() =>
            {
                object returnObject = null;
                switch (packet.Request)
                {
                    case Request.INITIALIZE:
                        Running = true;
                        player.Initialize(gameInfo, gameSetting);
                        break;
                    case Request.DAILY_INITIALIZE:
                        player.Update(gameInfo);
                        player.DayStart();
                        break;
                    case Request.DAILY_FINISH:
                        player.Update(gameInfo);
                        break;
                    case Request.NAME:
                        if (PlayerName == null)
                        {
                            returnObject = player.Name;
                            if (returnObject == null)
                            {
                                returnObject = player.GetType().Name;
                            }
                        }
                        else
                        {
                            returnObject = PlayerName;
                        }
                        break;
                    case Request.ROLE:
                        if (RequestRole != null)
                        {
                            returnObject = RequestRole.ToString();
                        }
                        else
                        {
                            returnObject = "none";
                        }
                        break;
                    case Request.ATTACK:
                        player.Update(gameInfo);
                        returnObject = player.Attack();
                        break;
                    case Request.TALK:
                        player.Update(gameInfo);
                        returnObject = player.Talk();
                        if (returnObject == null)
                        {
                            returnObject = Talk.SKIP;
                        }
                        break;
                    case Request.WHISPER:
                        player.Update(gameInfo);
                        returnObject = player.Whisper();
                        if (returnObject == null)
                        {
                            returnObject = Talk.SKIP;
                        }
                        break;
                    case Request.DIVINE:
                        player.Update(gameInfo);
                        returnObject = player.Divine();
                        break;
                    case Request.GUARD:
                        player.Update(gameInfo);
                        returnObject = player.Guard();
                        break;
                    case Request.VOTE:
                        player.Update(gameInfo);
                        returnObject = player.Vote();
                        break;
                    case Request.FINISH:
                        player.Update(gameInfo);
                        player.Finish();
                        Running = false;
                        break;
                    default:
                        break;
                }
                return returnObject;
            });
            if (task.Wait(Timeout))
            {
                return task.Result;
            }
            else
            {
                throw new TimeoutException(packet.Request + "@" + player.Name + " aborts because of timeout(" + Timeout + "ms).");
            }
        }

        /// <summary>
        /// Whether or not the given talk is after lastTalk.
        /// </summary>
        /// <param name="talk"></param>
        /// <param name="lastTalk"></param>
        /// <returns></returns>
        /// <remarks>If it is same, return false.</remarks>
        private bool IsAfter(TalkToSend talk, Talk lastTalk)
        {
            if (lastTalk != null)
            {
                if (talk.Day < lastTalk.Day)
                {
                    return false;
                }
                if (talk.Day == lastTalk.Day && talk.Idx <= lastTalk.Idx)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
