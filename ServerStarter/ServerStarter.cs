//
// ServerStarter.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using System;
using System.IO;

namespace AIWolf.Server
{
    class ServerStarter
    {
        public static void Main(string[] args)
        {
            var port = 10000;
            var agentNum = 5;
            var gameNum = 1;
            var logDir = ".";
            var showConsoleLog = true;

            for (var i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("-"))
                {
                    if (args[i] == "-p")
                    {
                        i++;
                        port = int.Parse(args[i]);
                    }
                    else if (args[i] == "-n")
                    {
                        i++;
                        agentNum = int.Parse(args[i]);
                    }
                    else if (args[i] == "-g")
                    {
                        i++;
                        gameNum = int.Parse(args[i]);
                    }
                    else if (args[i] == "-l")
                    {
                        i++;
                        logDir = args[i];
                    }
                    else if (args[i] == "-cl")
                    {
                        showConsoleLog = false;
                    }
                }
            }

            Console.WriteLine($"Start AIWolf.NET Server port:{port} playerNum:{agentNum} gameNum:{gameNum}");
            var gameSetting = GameSetting.GetDefaultGameSetting(agentNum);

            var server = new TcpipServer(port, agentNum, gameSetting);
            server.WaitForConnection();
            var logSubDir = Path.Combine(Path.GetFullPath(logDir), DateTime.Now.ToString("yyyyMMddHHmmss"));
            for (var i = 0; i < gameNum; i++)
            {
                var logName = Path.Combine(logSubDir, i.ToString("D3") + ".log");
                var game = new AIWolfGame(gameSetting, server);
                game.ShowConsoleLog = showConsoleLog;
                game.GameLogger = new FileGameLogger(logName);
                game.Rand = new Random();
                game.Start();
            }
            server.Close();
        }
    }
}
