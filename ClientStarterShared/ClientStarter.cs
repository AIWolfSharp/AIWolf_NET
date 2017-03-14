//
// ClientStarter.cs
//
// Copyright (c) 2017 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using AIWolf.Lib;
using AIWolf.Player.Sample;
#if !NET4
using Microsoft.Extensions.DependencyModel;
using System.Runtime.Loader;
#endif
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AIWolf
{
    /// <summary>
    /// Client starter class.
    /// </summary>
    public class ClientStarter
    {
        /// <summary>
        /// Main method.
        /// </summary>
        /// <param name="args">Arguments.</param>
        /// <remarks>
        /// Usage: [-h host] [-p port] [-t timeout] -c clientClass dllName [-r role] [-n name] [-d]
        /// </remarks>
        public static void Main(string[] args)
        {
            string host = "localhost";
            int port = 10000;
            string clsName = null;
            string dllName = null;
            Role roleRequest = Role.UNC; // No request by default.
            string playerName = null; // Obtained from the player by default.
            int timeout = -1; // No limit by default.
            bool useDefaultPlayer = false;

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals("-d"))
                {
                    useDefaultPlayer = true;
                }
                else if (args[i].Equals("-p"))
                {
                    i++;
                    if (i < args.Length || !args[i].StartsWith("-"))
                    {
                        if (!int.TryParse(args[i], out port))
                        {
                            Console.Error.WriteLine("ClientStarter: Invalid port {0}.", args[i]);
                            return;
                        }
                    }
                    else
                    {
                        Usage();
                    }
                }
                else if (args[i].Equals("-h"))
                {
                    i++;
                    if (i < args.Length || !args[i].StartsWith("-"))
                    {
                        host = args[i];
                    }
                    else
                    {
                        Usage();
                    }
                }
                else if (args[i].Equals("-c"))
                {
                    i++;
                    if (i < args.Length || !args[i].StartsWith("-"))
                    {
                        clsName = args[i];
                    }
                    else
                    {
                        Usage();
                    }
                    i++;
                    if (i < args.Length || !args[i].StartsWith("-"))
                    {
                        dllName = args[i];
                    }
                    else
                    {
                        Usage();
                    }
                }
                else if (args[i].Equals("-r"))
                {
                    i++;
                    if (i < args.Length || !args[i].StartsWith("-"))
                    {
                        if (!Enum.TryParse(args[i], out roleRequest))
                        {
                            Console.Error.WriteLine("ClientStarter: Invalid role {0}.", args[i]);
                            return;
                        }
                    }
                    else
                    {
                        Usage();
                    }
                }
                else if (args[i].Equals("-n"))
                {
                    i++;
                    if (i < args.Length || !args[i].StartsWith("-"))
                    {
                        playerName = args[i];
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
                            Console.Error.WriteLine("ClientStarter: Invalid timeout {0}.", args[i]);
                            return;
                        }
                    }
                    else
                    {
                        Usage();
                    }
                }
            }
            if (port < 0 || (!useDefaultPlayer && clsName == null))
            {
                Usage();
            }

            IPlayer player;
            if (useDefaultPlayer)
            {
                player = new RoleAssignPlayer();
            }
            else
            {
                Assembly assembly;
                try
                {
#if NET4
                    assembly = Assembly.LoadFrom(dllName);
#else
                    assembly = new AssemblyLoader(Path.GetDirectoryName(dllName)).LoadFromAssemblyPath(Path.GetFullPath(dllName));
#endif
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine("ClientStarter: Error in loading {0}.", dllName);
                    Console.Error.WriteLine(e);
                    return;
                }

                try
                {
                    player = (IPlayer)Activator.CreateInstance(assembly.GetType(clsName));
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine("ClientStarter: Error in creating instance of {0}.", clsName);
                    Console.Error.WriteLine(e);
                    return;
                }
            }

            TcpipClient client = new TcpipClient(host, port, playerName, roleRequest, timeout);
            try
            {
                client.Connect(player);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("ClientStarter: Error in running player.");
                Console.Error.WriteLine(ex);
                if (ex is AggregateException)
                {
                    foreach (var e in (ex as AggregateException).InnerExceptions)
                    {
                        Console.Error.WriteLine(e);
                    }
                }
                return;
            }
        }

        static void Usage()
        {
            Console.Error.WriteLine("Usage: ClientStarter [-h host] [-p port] -c clientClass dllName [roleRequest] [-n name] [-t timeout] [-d]");
            Environment.Exit(0);
        }
    }

#if !NET4
    class AssemblyLoader : AssemblyLoadContext
    {
        string folderPath;

        public AssemblyLoader(string folderPath)
        {
            this.folderPath = folderPath;
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            var cl = DependencyContext.Default.CompileLibraries.Where(d => d.Name.Contains(assemblyName.Name));
            if (cl.Count() > 0)
            {
                return Assembly.Load(new AssemblyName(cl.First().Name));
            }
            else
            {
                var fileInfo = new FileInfo($"{folderPath}{Path.DirectorySeparatorChar}{assemblyName.Name}.dll");
                if (File.Exists(fileInfo.FullName))
                {
                    var asl = new AssemblyLoader(fileInfo.DirectoryName);
                    return asl.LoadFromAssemblyPath(fileInfo.FullName);
                }
            }
            return Assembly.Load(assemblyName);
        }
    }
#endif
}
