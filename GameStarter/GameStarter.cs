//
// GameStarter.cs
//
// Copyright (c) 2017 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using AIWolf.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AIWolf
{
    class GameStarter
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Usage();
            }
            var starter = new GameStarter(args[0]);
            starter.Start();
        }

        static void Usage()
        {
            Console.Error.WriteLine("Usage: GameStarter initFileName");
            Environment.Exit(0);
        }

        int agentNum = 5;
        int gameNum = 1;
        int port = 10000;
        string logDir = "./log/";
        bool isConsoleLog = true;
        int delay = 100;
        List<string[]> commands = new List<string[]>();

        void StartServer()
        {
            Console.WriteLine("Start AIWolf.NET Server port:{0} playerNum:{1} gameNum:{2}", port, agentNum, gameNum);
            var gameSetting = GameSettingToSend.GetDefaultGameSetting(agentNum);
            var server = new TcpipServer(port, agentNum, gameSetting);
            server.WaitForConnection();
            var logSubDir = Path.Combine(Path.GetFullPath(logDir), DateTime.Now.ToString("yyyyMMddHHmmss"));
            for (var i = 0; i < gameNum; i++)
            {
                var logName = Path.Combine(logSubDir, i.ToString("D3") + ".log");
                var game = new AIWolfGame(gameSetting, server);
                game.ShowConsoleLog = isConsoleLog;
                game.GameLogger = new FileGameLogger(logName);
                game.Rand = new Random();
                game.Start();
            }
            server.Close();
        }

        void Start()
        {
            var tasks = new List<Task>();
            tasks.Add(Task.Run(() => StartServer()));
            foreach (var command in commands)
            {
                var process = new Process();
                if (command.Length > 0)
                {
                    process.StartInfo.FileName = command[0];
                }
                if (command.Length > 1)
                {
                    process.StartInfo.Arguments = command[1];
                }
                if (command.Length > 2)
                {
                    process.StartInfo.WorkingDirectory = command[2];
                }
                process.Start();
                tasks.Add(Task.Run(() => process.WaitForExit()));
                Thread.Sleep(delay);
            }
            Task.WaitAll(tasks.ToArray());
        }

        GameStarter(string initFileName)
        {
            foreach (var line in File.ReadLines(initFileName, Encoding.UTF8))
            {
                if (line.StartsWith("#"))
                {
                    continue;
                }
                if (line.Contains("=")) // 代入行
                {
                    var eq = line.Split('=');
                    if (eq.Length < 2)
                    {
                        continue;
                    }
                    var lhs = eq[0].Trim();
                    var rhs = eq[1].Trim();
                    if (String.Compare(lhs, "log", ignoreCase: true) == 0)
                    {
                        logDir = rhs;
                    }
                    else if (String.Compare(lhs, "port", ignoreCase: true) == 0)
                    {
                        port = int.Parse(rhs);
                    }
                    else if (String.Compare(lhs, "agent", ignoreCase: true) == 0)
                    {
                        agentNum = int.Parse(rhs);
                    }
                    else if (String.Compare(lhs, "game", ignoreCase: true) == 0)
                    {
                        gameNum = int.Parse(rhs);
                    }
                    else if (String.Compare(lhs, "delay", ignoreCase: true) == 0)
                    {
                        delay = int.Parse(rhs);
                    }
                    else if (String.Compare(lhs, "consolelog", ignoreCase: true) == 0)
                    {
                        var stringComparison = StringComparison.CurrentCultureIgnoreCase;
                        if (rhs.StartsWith("y", stringComparison) || rhs.StartsWith("t", stringComparison))
                        {
                            isConsoleLog = true;
                        }
                        else
                        {
                            isConsoleLog = false;
                        }
                    }
                }
                else // コマンド行
                {
                    var command = line.Split(',');
                    if (!String.IsNullOrEmpty(command[0].Trim()))
                    {
                        commands.Add(command);
                    }
                }
            }
        }

    }
}