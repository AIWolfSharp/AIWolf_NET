using System;
using System.Runtime.Serialization;

namespace AIWolf.Common
{
    /// <summary>
    /// Exception that occurs during execution of AIWolf application.
    /// </summary>
    /// <remarks></remarks>
    [Serializable]
    public class AIWolfRuntimeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the AIWolfRuntimeException class.
        /// </summary>
        /// <remarks></remarks>
        public AIWolfRuntimeException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the AIWolfRuntimeException class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <remarks></remarks>
        public AIWolfRuntimeException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the AIWolfRuntimeException class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception,
        /// or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        /// <remarks></remarks>
        public AIWolfRuntimeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the AIWolfRuntimeException class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        /// <remarks></remarks>
        protected AIWolfRuntimeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
