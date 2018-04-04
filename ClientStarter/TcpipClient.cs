//
// TcpipClient.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using AIWolf.Lib;
using AIWolf.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace AIWolf.Client
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
        TcpClient tcpClient;
        IPlayer player;
        GameInfo gameInfo;
        GameInfo lastGameInfo;

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
        public TcpipClient(string host, int port, string playerName, Role requestRole, int timeout = -1)
        {
            this.host = host;
            this.port = port;
            this.playerName = playerName;
            this.requestRole = requestRole;
            this.timeout = timeout;
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

            using (var sr = new StreamReader(tcpClient.GetStream()))
            using (var sw = new StreamWriter(tcpClient.GetStream()))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    var packet = ToPacket(line);
                    var obj = Recieve(packet);
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
                        else // Maybe obj is Agent.
                        {
                            sw.WriteLine(DataConverter.Serialize(obj));
                        }
                        sw.Flush();
                    }
                }
            }
        }

        /// <summary>
        /// Receives a packet from the server and sends it to the player, and returns what returned from the player.
        /// </summary>
        /// <param name="packet">The packet from the server.</param>
        /// <returns>The object returned from the player.</returns>
        object Recieve(Packet packet)
        {
            var gameSetting = packet.GameSetting;

            if (packet.GameInfo is GameInfo gi)
            {
                gameInfo = gi;
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
                        gameInfo.AddTalk(t);
                    }
                }
            }

            if (packet.WhisperHistory != null)
            {
                foreach (var w in packet.WhisperHistory)
                {
                    if (IsNew(w))
                    {
                        gameInfo.AddWhisper(w);
                    }
                }
            }

            var task = Task.Run(() =>
            {
                object returnObject = null;
                switch (packet.Request)
                {
                    case Request.NO_REQUEST:
                        break;
                    case Request.INITIALIZE:
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
                            var name = player.Name;
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
                        var talkText = player.Talk();
                        if (talkText == null)
                        {
                            returnObject = Lib.Talk.SKIP;
                        }
                        else
                        {
                            returnObject = talkText;
                        }
                        break;
                    case Request.WHISPER:
                        player.Update(gameInfo);
                        var whisperText = player.Whisper();
                        if (whisperText == null)
                        {
                            returnObject = Lib.Talk.SKIP;
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
                Error.TimeoutError($"{packet.Request}@{player.Name} exceeds the time limit({timeout}ms).");
                task.Wait(-1);
                return task.Result;
            }
        }

        /// <summary>
        /// Whether or not the given utterance is newer than ones already received.
        /// </summary>
        /// <param name="utterance">The utterance to be checked.</param>
        /// <returns>True if it is new.</returns>
        bool IsNew(IUtterance utterance)
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
            var map = DataConverter.Deserialize<Dictionary<string, object>>(line);
            var request = (Request)Enum.Parse(typeof(Request), (string)map["request"]);
            if (map["gameInfo"] != null)
            {
                var gameInfo = DataConverter.Deserialize<GameInfo>(DataConverter.Serialize(map["gameInfo"]));
                if (map["gameSetting"] != null)
                {
                    var gameSetting = DataConverter.Deserialize<GameSetting>(DataConverter.Serialize(map["gameSetting"]));
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
