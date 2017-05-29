using System;
using System.IO;
using System.Text;

namespace AIWolf
{
    class GameStarter
    {
        static void Main(string[] args)
        {
            string initFileName = null;
            string logDir = null;
            int gameNum = 0;
            int timeout = 0;
            int port = 0;

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-f")
                {
                    i++;
                    initFileName = args[i];
                }
                else if (args[i] == "-g")
                {
                    i++;
                    if (i < args.Length || !args[i].StartsWith("-"))
                    {
                        if (!int.TryParse(args[i], out gameNum))
                        {
                            Console.Error.WriteLine("GameStarter: Invalid number of games {0}.", args[i]);
                            return;
                        }
                    }
                    else
                    {
                        Usage();
                    }
                }
                else if (args[i] == "-l")
                {
                    i++;
                    logDir = args[i];

                }
                else if (args[i] == "-p")
                {
                    i++;
                    if (i < args.Length || !args[i].StartsWith("-"))
                    {
                        if (!int.TryParse(args[i], out port))
                        {
                            Console.Error.WriteLine("GameStarter: Invalid port {0}.", args[i]);
                            return;
                        }
                    }
                    else
                    {
                        Usage();
                    }
                }
                else if (args[i].Equals("-t"))
                {
                    i++;
                    if (i < args.Length || !args[i].StartsWith("-"))
                    {
                        if (!int.TryParse(args[i], out timeout))
                        {
                            Console.Error.WriteLine("GameStarter: Invalid timeout {0}.", args[i]);
                            return;
                        }
                    }
                    else
                    {
                        Usage();
                    }
                }
                else
                {
                    Usage();
                }
            }
            if (initFileName == null)
            {
                Usage();
            }

        }

        static void Usage()
        {
            Console.Error.WriteLine("Usage: GameStarter -f initFileName [-g gameNum] [-l logDir] [-p port] [-t timeout]");
            Environment.Exit(0);
        }

        int AgentNum { get; set; } = 15;
        int GameNum { get; set; } = 1;
        int Port { get; set; } = 10000;
        int Timeout { get; set; } = -1;
        string LogDir { get; set; } = "./log/";

        GameStarter(string initFileName)
        {
            var lines = File.ReadLines(initFileName, Encoding.UTF8);
        }

    }
}