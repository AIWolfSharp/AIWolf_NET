//
// AssemblyLoader.cs
//
// Copyright (c) 2017 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using System.IO;
using System.Reflection;
using System;

namespace AIWolf.Client
{
    class AssemblyLoader
    {
        string folderPath;

        public AssemblyLoader(string folderPath) => this.folderPath = folderPath;

        public Assembly LoadFromFolder(object sender, ResolveEventArgs args)
        {
            string assemblyPath = Path.Combine(folderPath, new AssemblyName(args.Name).Name + ".dll");
            if (!File.Exists(assemblyPath)) return null;
            return Assembly.LoadFrom(assemblyPath);
        }
    }
}
