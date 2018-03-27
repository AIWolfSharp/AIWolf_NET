//
// TcpipServer.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using AIWolf.Lib;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AIWolf.Server
{
    public class TcpipServer : IGameServer
    {
        ILogger serverLogger;
        Dictionary<TcpClient, Agent> connectionAgentMap = new Dictionary<TcpClient, Agent>();
        Dictionary<Agent, TcpClient> agentConnectionMap = new Dictionary<Agent, TcpClient>();
        Dictionary<Agent, string> nameMap = new Dictionary<Agent, string>();
        Dictionary<Agent, int> lastTalkIdxMap = new Dictionary<Agent, int>();
        Dictionary<Agent, int> lastWhisperIdxMap = new Dictionary<Agent, int>();
        TcpListener serverSocket;
        int port;
        IPAddress address = IPAddress.Parse("127.0.0.1");
        int connectionLimit;

        /// <summary>
        /// Current game data.
        /// </summary>
        public GameData GameData { private get; set; }

        /// <summary>
        /// Game Setting.
        /// </summary>
        public GameSettingToSend GameSetting { private get; set; }

        /// <summary>
        /// The list of agents connecting to this server.
        /// </summary>
        public IList<Agent> ConnectedAgentList
        {
            get
            {
                lock (connectionAgentMap)
                {
                    return connectionAgentMap.Values.ToList();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="port">The port number.</param>
        /// <param name="limit">The upper limit of the number of connections.</param>
        /// <param name="gameSetting">The setting of the game.</param>
        public TcpipServer(int port, int limit, GameSettingToSend gameSetting)
        {
            GameSetting = gameSetting;
            this.port = port;
            connectionLimit = limit;
            //ILoggerFactory loggerFactory = new LoggerFactory().AddConsole();
            ILoggerFactory loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new ConsoleLoggerProvider((text, logLevel) => logLevel >= LogLevel.Critical, true));
            serverLogger = loggerFactory.CreateLogger(GetType().ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        public void WaitForConnection()
        {
            foreach (TcpClient client in connectionAgentMap.Keys)
            {
                if (client != null && client.Connected)
                {
                    client.Dispose();
                }
            }

            connectionAgentMap.Clear();
            agentConnectionMap.Clear();
            nameMap.Clear();

            Console.WriteLine("Waiting for connection...");
            serverSocket = new TcpListener(address, port);
            serverSocket.Start();

            while (connectionAgentMap.Count < connectionLimit)
            {
                Task<TcpClient> task = serverSocket.AcceptTcpClientAsync();
                task.Wait();
                TcpClient client = task.Result;
                lock (connectionAgentMap)
                {
                    Agent agent = null;
                    for (int i = 1; i <= connectionLimit; i++)
                    {
                        if (!connectionAgentMap.ContainsValue(Agent.GetAgent(i)))
                        {
                            agent = Agent.GetAgent(i);
                            break;
                        }
                    }
                    connectionAgentMap[client] = agent ?? throw new Exception("Fail to create agent");
                    agentConnectionMap[agent] = client;
                    nameMap[agent] = RequestName(agent);
                }
            }
            serverSocket.Stop();
        }

        /// <summary>
        /// Send data to client.
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="request"></param>
        void Send(Agent agent, Request request)
        {
            try
            {
                string message;
                if (request == Server.Request.DAILY_INITIALIZE || request == Server.Request.INITIALIZE)
                {
                    lastTalkIdxMap.Clear();
                    lastWhisperIdxMap.Clear();
                    message = DataConverter.Serialize(new PacketToSend(request, GameData.GetGameInfo(agent), GameSetting));
                }
                else if (request == Server.Request.NAME || request == Server.Request.ROLE)
                {
                    message = DataConverter.Serialize(new PacketToSend(request));
                }
                else if (request != Server.Request.FINISH)
                {
                    if (request == Server.Request.VOTE && GameData.LatestVoteList.Count != 0)
                    {
                        message = DataConverter.Serialize(new PacketToSend(request, GameData.GetGameInfo(agent)));
                    }
                    else if (request == Server.Request.ATTACK && GameData.LatestAttackVoteList.Count != 0)
                    {
                        message = DataConverter.Serialize(new PacketToSend(request, GameData.GetGameInfo(agent)));
                    }
                    else if (GameData.Executed != null && (request == Server.Request.DIVINE || request == Server.Request.GUARD
                        || request == Server.Request.WHISPER || request == Server.Request.ATTACK))
                    {
                        message = DataConverter.Serialize(new PacketToSend(request, GameData.GetGameInfo(agent)));
                    }
                    else
                    {
                        List<Talk> talkList = new List<Talk>(GameData.GetGameInfo(agent).TalkList);
                        List<Whisper> whisperList = new List<Whisper>(GameData.GetGameInfo(agent).WhisperList);
                        talkList = Minimize(agent, talkList, lastTalkIdxMap);
                        whisperList = Minimize(agent, whisperList, lastWhisperIdxMap);
                        message = DataConverter.Serialize(new PacketToSend(request, talkList, whisperList));
                    }
                }
                else
                {
                    message = DataConverter.Serialize(new PacketToSend(request, GameData.GetFinalGameInfo(agent)));
                }
                serverLogger.LogInformation("=>" + agent + ":" + message);

                TcpClient client = agentConnectionMap[agent];
                StreamWriter sw = new StreamWriter(client.GetStream());
                sw.WriteLine(message);
                sw.Flush();
            }
            catch (IOException e)
            {
                throw new Exception("Lost connection with " + agent, e);
            }
        }

        List<T> Minimize<T>(Agent agent, List<T> list, Dictionary<Agent, int> lastIdxMap)
        {
            int lastIdx = list.Count;
            if (lastIdxMap.ContainsKey(agent) && list.Count >= lastIdxMap[agent])
            {
                list = list.GetRange(lastIdxMap[agent], lastIdx - lastIdxMap[agent]);
            }
            lastIdxMap[agent] = lastIdx;
            return list;
        }

        Object Request(Agent agent, Request request)
        {
            try
            {
                TcpClient client = agentConnectionMap[agent];
                StreamReader sr = new StreamReader(client.GetStream());
                Send(agent, request);

                string line = sr.ReadLine();
                serverLogger.LogInformation("<=" + agent + ":" + line);

                if (line != null && line.Length == 0)
                {
                    line = null;
                }
                if (request == Server.Request.NAME || request == Server.Request.ROLE)
                {
                    return line;
                }
                else if (request == Server.Request.TALK || request == Server.Request.WHISPER)
                {
                    if (GameSetting.ValidateUtterance)
                    {
                        return line; // TODO Implement Content.Validate()
                    }
                    else
                    {
                        return line;
                    }
                }
                else if (request == Server.Request.ATTACK || request == Server.Request.DIVINE || request == Server.Request.GUARD || request == Server.Request.VOTE)
                {
                    if (line == null) return null;
                    Match m = regexToAgent.Match(line);
                    if (m.Success)
                    {
                        return Agent.GetAgent(int.Parse(m.Groups[1].Value));
                    }
                    throw new ArgumentException("Can't convert " + line + " to Agent.");
                }
                else
                {
                    return null;
                }
            }
            catch (IOException e)
            {
                throw new Exception("Lost connection with " + agent, e);
            }
        }

        static readonly Regex regexToAgent = new Regex(@"{""agentIdx"":(\d+)}");

        public void Init(Agent agent)
        {
            Send(agent, Server.Request.INITIALIZE);
        }

        public void DayStart(Agent agent)
        {
            Send(agent, Server.Request.DAILY_INITIALIZE);
        }

        public void DayFinish(Agent agent)
        {
            Send(agent, Server.Request.DAILY_FINISH);
        }

        public string RequestName(Agent agent)
        {
            if (!nameMap.ContainsKey(agent))
            {
                nameMap[agent] = (string)Request(agent, Server.Request.NAME);
            }
            return nameMap[agent];
        }

        public Role RequestRequestRole(Agent agent)
        {
            if (Enum.TryParse<Role>((string)Request(agent, Server.Request.ROLE), out Role role))
            {
                return role;
            }
            return Role.UNC;
        }

        public string RequestTalk(Agent agent)
        {
            return (string)Request(agent, Server.Request.TALK);
        }

        public string RequestWhisper(Agent agent)
        {
            return (string)Request(agent, Server.Request.WHISPER);
        }

        public Agent RequestVote(Agent agent)
        {
            return (Agent)Request(agent, Server.Request.VOTE);
        }

        public Agent RequestDivineTarget(Agent agent)
        {
            return (Agent)Request(agent, Server.Request.DIVINE);
        }

        public Agent RequestGuardTarget(Agent agent)
        {
            return (Agent)Request(agent, Server.Request.GUARD);
        }

        public Agent RequestAttackTarget(Agent agent)
        {
            return (Agent)Request(agent, Server.Request.ATTACK);
        }

        public void Finish(Agent agent)
        {
            Send(agent, Server.Request.FINISH);
        }

        public void Close()
        {
            if (serverSocket != null && serverSocket.Server.Connected)
            {
                serverSocket.Stop();
            }
            foreach (TcpClient client in connectionAgentMap.Keys)
            {
                client.Dispose();
            }
            connectionAgentMap.Clear();
            agentConnectionMap.Clear();
        }
    }
}
