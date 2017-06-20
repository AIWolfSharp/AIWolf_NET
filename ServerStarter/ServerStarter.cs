//
// ServerStarter.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using AIWolf.Lib;
using System;
using System.IO;

namespace AIWolf.Server
{
    class ServerStarter
    {
        public static void Main(string[] args)
        {
            int port = 10000;
            int agentNum = 5;
            int gameNum = 1;
            string logDir = ".";
            bool showConsoleLog = true;

            for (int i = 0; i < args.Length; i++)
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

            Console.WriteLine("Start AIWolf.NET Server port:{0} playerNum:{1} gameNum:{2}", port, agentNum, gameNum);
            GameSetting gameSetting = GameSetting.GetDefaultGameSetting(agentNum);

            TcpipServer server = new TcpipServer(port, agentNum, gameSetting);
            server.WaitForConnection();
            string logSubDir = Path.Combine(Path.GetFullPath(logDir), DateTime.Now.ToString("yyyyMMddHHmmss"));
            for (int i = 0; i < gameNum; i++)
            {
                string logName = Path.Combine(logSubDir, i.ToString("D3") + ".log");
                AIWolfGame game = new AIWolfGame(gameSetting, server);
                game.ShowConsoleLog = showConsoleLog;
                game.GameLogger = new FileGameLogger(logName);
                game.Rand = new Random();
                game.Start();
            }
            server.Close();
        }
    }
}
