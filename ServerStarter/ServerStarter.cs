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
using System.Threading.Tasks;

namespace AIWolf.Server
{
    class ServerStarter
    {
        public static void Main(string[] args)
        {
            int port = 10000;
            int agentNum = 12;

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("-"))
                {
                    if (args[i].Equals("-p"))
                    {
                        i++;
                        port = int.Parse(args[i]);
                    }
                    else if (args[i].Equals("-n"))
                    {
                        i++;
                        agentNum = int.Parse(args[i]);
                    }
                }
            }

            Console.WriteLine("Start AIWolf Server port:{0} playerNum:{1}", port, agentNum);
            GameSetting gameSetting = GameSetting.GetDefaultGameSetting(agentNum);

            TcpipServer server = new TcpipServer(port, agentNum, gameSetting);
            server.WaitForConnection();
            AIWolfGame game = new AIWolfGame(gameSetting, server);
            game.Rand = new Random();
            game.Start();
        }
    }
}
