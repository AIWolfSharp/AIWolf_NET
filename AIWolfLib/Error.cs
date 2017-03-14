//
// Error.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace AIWolf.Lib
{
    /// <summary>
    /// Error handling class.
    /// </summary>
    static class Error
    {
        /// <summary>
        /// Writes a warning message.
        /// </summary>
        /// <param name="message">Warning message.</param>
        /// <param name="memberName">The name of the caller.</param>
        /// <param name="filePath">The path of file containing the code of the caller.</param>
        /// <param name="lineNumber">The line number of the caller in the file.</param>
        public static void Warning(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            Console.Error.WriteLine(memberName + ": " + message + " at line " + lineNumber + " in " + Path.GetFileName(filePath));
        }

        /// <summary>
        /// Writes an error message, then throws AIWolfRuntimeException on debug.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="memberName">The name of the caller.</param>
        /// <param name="filePath">The path of file containing the code of the caller.</param>
        /// <param name="lineNumber">The line number of the caller in the file.</param>
        public static void RuntimeError(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            ThrowRuntimeException(memberName + ": " + message + " at line " + lineNumber + " in " + Path.GetFileName(filePath));
        }

        [Conditional("DEBUG")]
        static void ThrowRuntimeException(string message)
        {
            throw new AIWolfLibException(message);
        }

        /// <summary>
        /// Writes an error message, then throws TimeoutException on debug.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="memberName">The name of the caller.</param>
        /// <param name="filePath">The path of file containing the code of the caller.</param>
        /// <param name="lineNumber">The line number of the caller in the file.</param>
        public static void TimeoutError(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            ThrowTimeoutException(memberName + ": " + message + " at line " + lineNumber + " in " + Path.GetFileName(filePath));
        }

        [Conditional("DEBUG")]
        static void ThrowTimeoutException(string message)
        {
            throw new TimeoutException(message);
        }
    }

    /// <summary>
    /// Exception that occurred in AIWolf library.
    /// </summary>
    class AIWolfLibException : Exception
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public AIWolfLibException()
        {
        }

        /// <summary>
        /// Initializes a new instance of this class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public AIWolfLibException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of this class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception,
        /// or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public AIWolfLibException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
