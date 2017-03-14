//
// TcpipClient.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace AIWolf.Lib
{
#if JHELP
    /// <summary>
    /// TCP/IP接続人狼知能クライアント
    /// </summary>
#else
    /// <summary>
    /// AIWolf client using TCP/IP connection.
    /// </summary>
#endif
    public class TcpipClient
    {
        string host;
        int port;
        int timeout;
        Role requestRole;
        string playerName;

        bool running = false;
        TcpClient tcpClient;
        IPlayer player;
        GameInfo gameInfo;
        GameInfo lastGameInfo;
        int day = -1;
        Dictionary<int, List<Talk>> dayTalkMap = new Dictionary<int, List<Talk>>();
        Dictionary<int, List<Whisper>> dayWhisperMap = new Dictionary<int, List<Whisper>>();

#if JHELP
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="host">このクライアントの接続先のホスト名</param>
        /// <param name="port">このクライアントの接続先のポート番号</param>
        /// <param name="playerName">このクライアントが面倒を見るプレイヤーの名前</param>
        /// <param name="requestRole">このクライアントが要求する役職</param>
        /// <param name="timeout">リクエスト応答の制限時間</param>
#else
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="host">Hostname this client connects.</param>
        /// <param name="port">Port number this client connects.</param>
        /// <param name="playerName">The name of player this client manages.</param>
        /// <param name="requestRole">Role this client requests.</param>
        /// <param name="timeout">The number of milliseconds to wait for the request call.</param>
#endif
        public TcpipClient(string host, int port, string playerName, Role requestRole, int timeout)
        {
            this.host = host;
            this.port = port;
            this.playerName = playerName;
            this.requestRole = requestRole;
            this.timeout = timeout;
            running = false;
        }

#if JHELP
        /// <summary>
        /// プレイヤーをサーバに接続する
        /// </summary>
        /// <param name="player">接続するプレイヤー</param>
#else
        /// <summary>
        /// Connects the player to the server.
        /// </summary>
        /// <param name="player">The player to be connected.</param>
#endif
        public void Connect(IPlayer player)
        {
            this.player = player;

            tcpClient = new TcpClient();
            tcpClient.ConnectAsync(host, port).Wait();

            try
            {
                StreamReader sr = new StreamReader(tcpClient.GetStream());
                StreamWriter sw = new StreamWriter(tcpClient.GetStream());
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Packet packet = ToPacket(line);

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
                            sw.WriteLine(DataConverter.Serialize(obj));
                        }
                        sw.Flush();
                    }
                }
            }
            catch (IOException)
            {
                if (running)
                {
                    throw;
                }
            }
            finally
            {
                running = false;
            }
        }

        /// <summary>
        /// Receives a packet from the server and sends it to the player, and returns what returned from the player.
        /// </summary>
        /// <param name="packet">The packet from the server.</param>
        /// <returns>The object returned from the player.</returns>
        object Recieve(Packet packet)
        {
            GameSetting gameSetting = packet.GameSetting;

            if (packet.GameInfo != null)
            {
                gameInfo = packet.GameInfo;
                if (gameInfo.Day == day + 1) // New day.
                {
                    if (gameInfo.Day > 0)
                    {
                        // Save yesterday's talks/whispers.
                        dayTalkMap[day] = lastGameInfo.TalkList;
                        dayWhisperMap[day] = lastGameInfo.WhisperList;
                    }
                    day = gameInfo.Day;
                }
                lastGameInfo = gameInfo;
            }
            else
            {
                gameInfo = lastGameInfo;
            }

            if (packet.TalkHistory != null)
            {
                foreach (var t in packet.TalkHistory)
                {
                    if (IsNew(t))
                    {
                        gameInfo.TalkList.Add(t);
                    }
                }
            }

            if (packet.WhisperHistory != null)
            {
                foreach (var w in packet.WhisperHistory)
                {
                    if (IsNew(w))
                    {
                        gameInfo.WhisperList.Add(w);
                    }
                }
            }

            Task<object> task = Task.Run(() =>
            {
                object returnObject = null;
                switch (packet.Request)
                {
                    case Request.NO_REQUEST:
                        break;
                    case Request.INITIALIZE:
                        running = true;
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
                        if (playerName == null || playerName.Length == 0)
                        {
                            string name = player.Name;
                            if (name == null || name.Length == 0)
                            {
                                returnObject = player.GetType().Name;
                            }
                            returnObject = name;
                        }
                        else
                        {
                            returnObject = playerName;
                        }
                        break;
                    case Request.ROLE:
                        if (requestRole != Role.UNC)
                        {
                            returnObject = requestRole.ToString();
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
                        string talkText = player.Talk();
                        if (talkText == null)
                        {
                            returnObject = Utterance.SKIP;
                        }
                        else
                        {
                            returnObject = talkText;
                        }
                        break;
                    case Request.WHISPER:
                        player.Update(gameInfo);
                        string whisperText = player.Whisper();
                        if (whisperText == null)
                        {
                            returnObject = Utterance.SKIP;
                        }
                        else
                        {
                            returnObject = whisperText;
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
                        running = false;
                        day = -1;
                        dayTalkMap.Clear();
                        dayWhisperMap.Clear();
                        break;
                    default:
                        break;
                }
                return returnObject;
            });
            if (task.Wait(timeout))
            {
                return task.Result;
            }
            else
            {
                Error.TimeoutError(string.Format("{0}@{1} exceeds the time limit({2}ms).", packet.Request, player.Name, timeout));
                task.Wait(-1);
                return task.Result;
            }
        }

        /// <summary>
        /// Whether or not the given utterance is newer than ones already received.
        /// </summary>
        /// <param name="utterance">The utterance to be checked.</param>
        /// <returns>True if it is new.</returns>
        bool IsNew(Utterance utterance)
        {
            if (utterance is Whisper)
            {
                Whisper lastWhisper = null;
                if (gameInfo.WhisperList != null && gameInfo.WhisperList.Count != 0)
                {
                    lastWhisper = gameInfo.WhisperList.Last();
                }
                if (lastWhisper != null)
                {
                    if (utterance.Day < lastWhisper.Day)
                    {
                        return false;
                    }
                    if (utterance.Day == lastWhisper.Day && utterance.Idx <= lastWhisper.Idx)
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                Talk lastTalk = null;
                if (gameInfo.TalkList != null && gameInfo.TalkList.Count != 0)
                {
                    lastTalk = gameInfo.TalkList.Last();
                }
                if (lastTalk != null)
                {
                    if (utterance.Day < lastTalk.Day)
                    {
                        return false;
                    }
                    if (utterance.Day == lastTalk.Day && utterance.Idx <= lastTalk.Idx)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Returns the instance of Packet class converted from the JSON string given.
        /// </summary>
        /// <param name="line">The JSON string to be converted.</param>
        /// <returns>The instance of Packet class converted from the JSON string.</returns>
        Packet ToPacket(string line)
        {
            Dictionary<string, object> map = DataConverter.Deserialize<Dictionary<string, object>>(line);

            if (map["request"] == null)
            {
                Error.RuntimeError("There is no request in " + line + ".");
                Error.Warning("Force it to be Request.DUMMY.");
                return new Packet(Request.NO_REQUEST);
            }

            Request request;
            if (!Enum.TryParse((string)map["request"], out request))
            {
                Error.RuntimeError("Invalid request in " + line + ".");
                Error.Warning("Force it to be Request.DUMMY.");
                return new Packet(Request.NO_REQUEST);
            }

            if (map["gameInfo"] != null)
            {
                GameInfo gameInfo = DataConverter.Deserialize<GameInfo>(DataConverter.Serialize(map["gameInfo"]));
                if (map["gameSetting"] != null)
                {
                    GameSetting gameSetting = DataConverter.Deserialize<GameSetting>(DataConverter.Serialize(map["gameSetting"]));
                    return new Packet(request, gameInfo, gameSetting);
                }
                else
                {
                    return new Packet(request, gameInfo);
                }
            }
            else if (map["talkHistory"] != null)
            {
                List<Talk> talkHistoryList = DataConverter.Deserialize<List<Dictionary<string, string>>>(DataConverter.Serialize(map["talkHistory"]))
                    .Select(m => DataConverter.Deserialize<Talk>(DataConverter.Serialize(m))).ToList();
                List<Whisper> whisperHistoryList = DataConverter.Deserialize<List<Dictionary<string, string>>>(DataConverter.Serialize(map["whisperHistory"]))
                    .Select(m => DataConverter.Deserialize<Whisper>(DataConverter.Serialize(m))).ToList();
                return new Packet(request, talkHistoryList, whisperHistoryList);
            }
            else
            {
                return new Packet(request);
            }
        }
    }
}
