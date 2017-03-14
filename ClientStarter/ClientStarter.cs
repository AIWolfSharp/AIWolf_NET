using AIWolf.Common.Data;
using AIWolf.Common.Net;
using System;
using System.IO;
using System.Reflection;

namespace AIWolf.ClientStarter
{
    /// <summary>
    /// AIWolf client starter.
    /// </summary>
    /// <remarks>
    /// Usage: [-h host] [-p port] [-t timeout] -c clientClass dllName [roleRequest] [-n name]
    /// </remarks>
    public class ClientStarter
    {
        /// <summary>
        /// Initializes a new instance of ClientStarter class.
        /// </summary>
        /// <remarks></remarks>
        public ClientStarter() { }

        /// <summary>
        /// Main method.
        /// </summary>
        /// <param name="args">Arguments.</param>
        /// <remarks></remarks>
        public static void Main(string[] args)
        {
            string host = "localhost";
            int port = 10000;
            string clsName = null;
            string dllName = null;
            Role? roleRequest = null;
            string playerName = null;
            int timeout = -1; // Do not limit by default.

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("-"))
                {
                    if (args[i].Equals("-p"))
                    {
                        i++;
                        port = int.Parse(args[i]);
                    }
                    else if (args[i].Equals("-h"))
                    {
                        i++;
                        host = args[i];
                    }
                    else if (args[i].Equals("-c"))
                    {
                        i++;
                        clsName = args[i];
                        i++;
                        dllName = args[i];
                        i++;
                        try
                        {
                            if (i > args.Length - 1 || args[i].StartsWith("-")) // Roleでない
                            {
                                i--;
                                roleRequest = null;
                                continue;
                            }
                            roleRequest = (Role)Enum.Parse(typeof(Role), args[i]);
                        }
                        catch (ArgumentException)
                        {
                            Console.Error.WriteLine("No such role as " + args[i]);
                            return;
                        }
                    }
                    else if (args[i].Equals("-n"))
                    {
                        i++;
                        playerName = args[i];
                    }
                    else if (args[i].Equals("-t"))
                    {
                        i++;
                        timeout = int.Parse(args[i]);
                    }
                }
            }
            if (port < 0 || clsName == null)
            {
                Console.Error.WriteLine("Usage:" + typeof(ClientStarter).Name + " [-h host] [-p port] -c clientClass dllName [roleRequest] [-n name] [-t timeout]");
                return;
            }

            Assembly assembly;
            try
            {
                assembly = Assembly.LoadFrom(dllName);
            }
            catch (FileNotFoundException)
            {
                Console.Error.WriteLine("Can not find " + dllName);
                return;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error in loading " + dllName);
                Console.Error.WriteLine(e);
                return;
            }

            IPlayer player;
            try
            {
                player = (IPlayer)Activator.CreateInstance(assembly.GetType(clsName));
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error in creating instance of " + clsName);
                Console.Error.WriteLine(e);
                return;
            }

            TcpipClient client = new TcpipClient(host, port, roleRequest);
            client.Timeout = timeout;
            if (playerName != null)
            {
                client.PlayerName = playerName;
            }
            try
            {
                client.Connect(player);
            }
            catch (AggregateException e)
            {
                Console.Error.WriteLine(e.InnerException);
                return;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                return;
            }
        }
    }
}
