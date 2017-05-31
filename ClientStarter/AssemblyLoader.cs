//
// AssemblyLoader.cs
//
// Copyright (c) 2017 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

#if NETCOREAPP1_1
using Microsoft.Extensions.DependencyModel;
using System.Runtime.Loader;
#endif
using System.IO;
using System.Linq;
using System.Reflection;

namespace AIWolf
{
#if NETCOREAPP1_1
    public class AssemblyLoader : AssemblyLoadContext
    {
        string folderPath;

        public AssemblyLoader(string folderPath) => this.folderPath = folderPath;

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
