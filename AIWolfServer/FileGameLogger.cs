//
// FileGameLogger.cs
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
    /// <summary>
    /// GameLogger using file.
    /// </summary>
    class FileGameLogger
    {
        string fileName;
        TextWriter writer;

        public FileGameLogger(string path)
        {
            if (path != null)
            {
                if (Directory.Exists(path))
                {
                    fileName = Path.ChangeExtension(Path.Combine(path, DateTime.Now.ToString("yyyyMMddHHmmss")), ".log");
                }
                else
                {
                    fileName = path;
                }
                Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(fileName)));
                try
                {
                    writer = new StreamWriter(new FileStream(fileName, FileMode.CreateNew));
                }
                catch
                {
                    if (writer != null) writer.Dispose();
                    throw;
                }
            }
            else
            {
                writer = Console.Out;
            }
        }

        /// <summary>
        /// Releases all resources used by the writer.
        /// </summary>
        public void Close()
        {
            writer.Dispose();
        }

        /// <summary>
        /// Clears all buffers for the writer.
        /// </summary>
        public void Flush()
        {
            writer.Flush();
        }

        /// <summary>
        /// Writes a line of log.
        /// </summary>
        /// <param name="line">A line of log.</param>
        public void Log(string line)
        {
            try
            {
                writer.WriteLine(line);
            }
            catch (IOException)
            {
                writer.Dispose();
                throw;
            }
        }
    }
}
