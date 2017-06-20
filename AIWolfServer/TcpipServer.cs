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
        public GameSetting GameSetting { private get; set; }

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
        public TcpipServer(int port, int limit, GameSetting gameSetting)
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
            foreach (var client in connectionAgentMap.Keys)
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
                var task = serverSocket.AcceptTcpClientAsync();
                task.Wait();
                var client = task.Result;
                lock (connectionAgentMap)
                {
                    Agent agent = null;
                    for (var i = 1; i <= connectionLimit; i++)
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
                if (request == Lib.Request.DAILY_INITIALIZE || request == Lib.Request.INITIALIZE)
                {
                    lastTalkIdxMap.Clear();
                    lastWhisperIdxMap.Clear();
                    message = DataConverter.Serialize(new Packet(request, GameData.GetGameInfo(agent), GameSetting));
                }
                else if (request == Lib.Request.NAME || request == Lib.Request.ROLE)
                {
                    message = DataConverter.Serialize(new Packet(request));
                }
                else if (request != Lib.Request.FINISH)
                {
                    if (request == Lib.Request.VOTE && GameData.LatestVoteList.Count != 0)
                    {
                        message = DataConverter.Serialize(new Packet(request, GameData.GetGameInfo(agent)));
                    }
                    else if (request == Lib.Request.ATTACK && GameData.LatestAttackVoteList.Count != 0)
                    {
                        message = DataConverter.Serialize(new Packet(request, GameData.GetGameInfo(agent)));
                    }
                    else if (GameData.Executed != null && (request == Lib.Request.DIVINE || request == Lib.Request.GUARD
                        || request == Lib.Request.WHISPER || request == Lib.Request.ATTACK))
                    {
                        message = DataConverter.Serialize(new Packet(request, GameData.GetGameInfo(agent)));
                    }
                    else
                    {
                        var talkList = new List<Talk>(GameData.GetGameInfo(agent).TalkList);
                        var whisperList = new List<Whisper>(GameData.GetGameInfo(agent).WhisperList);
                        talkList = Minimize(agent, talkList, lastTalkIdxMap);
                        whisperList = Minimize(agent, whisperList, lastWhisperIdxMap);
                        message = DataConverter.Serialize(new Packet(request, talkList, whisperList));
                    }
                }
                else
                {
                    message = DataConverter.Serialize(new Packet(request, GameData.GetFinalGameInfo(agent)));
                }
                serverLogger.LogInformation("=>" + agent + ":" + message);

                var client = agentConnectionMap[agent];
                var sw = new StreamWriter(client.GetStream());
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
            var lastIdx = list.Count;
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
                var client = agentConnectionMap[agent];
                var sr = new StreamReader(client.GetStream());
                Send(agent, request);

                var line = sr.ReadLine();
                serverLogger.LogInformation("<=" + agent + ":" + line);

                if (line != null && line.Length == 0)
                {
                    line = null;
                }
                if (request == Lib.Request.NAME || request == Lib.Request.ROLE)
                {
                    return line;
                }
                else if (request == Lib.Request.TALK || request == Lib.Request.WHISPER)
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
                else if (request == Lib.Request.ATTACK || request == Lib.Request.DIVINE || request == Lib.Request.GUARD || request == Lib.Request.VOTE)
                {
                    if (line == null) return null;
                    var m = regexToAgent.Match(line);
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
            Send(agent, Lib.Request.INITIALIZE);
        }

        public void DayStart(Agent agent)
        {
            Send(agent, Lib.Request.DAILY_INITIALIZE);
        }

        public void DayFinish(Agent agent)
        {
            Send(agent, Lib.Request.DAILY_FINISH);
        }

        public string RequestName(Agent agent)
        {
            if (!nameMap.ContainsKey(agent))
            {
                nameMap[agent] = (string)Request(agent, Lib.Request.NAME);
            }
            return nameMap[agent];
        }

        public Role RequestRequestRole(Agent agent)
        {
            if (Enum.TryParse<Role>((string)Request(agent, Lib.Request.ROLE), out var role))
            {
                return role;
            }
            return Role.UNC;
        }

        public string RequestTalk(Agent agent)
        {
            return (string)Request(agent, Lib.Request.TALK);
        }

        public string RequestWhisper(Agent agent)
        {
            return (string)Request(agent, Lib.Request.WHISPER);
        }

        public Agent RequestVote(Agent agent)
        {
            return (Agent)Request(agent, Lib.Request.VOTE);
        }

        public Agent RequestDivineTarget(Agent agent)
        {
            return (Agent)Request(agent, Lib.Request.DIVINE);
        }

        public Agent RequestGuardTarget(Agent agent)
        {
            return (Agent)Request(agent, Lib.Request.GUARD);
        }

        public Agent RequestAttackTarget(Agent agent)
        {
            return (Agent)Request(agent, Lib.Request.ATTACK);
        }

        public void Finish(Agent agent)
        {
            Send(agent, Lib.Request.FINISH);
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
