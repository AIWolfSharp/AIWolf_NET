//
// Error.cs
//
// Copyright (c) 2018 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace AIWolf.Client
{
    /// <summary>
    /// Error handling class for client.
    /// </summary>
    static class Error
    {
        /// <summary>
        /// Writes an error message, then throws TimeoutException on debug.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="memberName">The name of the caller.</param>
        /// <param name="filePath">The path of file containing the code of the caller.</param>
        /// <param name="lineNumber">The line number of the caller in the file.</param>
        public static void TimeoutError(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            ThrowTimeoutException($"{memberName}: {message} at line {lineNumber} in {Path.GetFileName(filePath)}");
            WriteRuntimeErrorMesg($"{memberName}: {message} at line {lineNumber} in {Path.GetFileName(filePath)}");
        }

        [Conditional("DEBUG")]
        static void ThrowTimeoutException(string message)
        {
            throw new TimeoutException(message);
        }

        [Conditional("RELEASE")]
        static void WriteRuntimeErrorMesg(string message)
        {
            Console.Error.WriteLine($"RuntimeError: {message}");
        }
    }
}
