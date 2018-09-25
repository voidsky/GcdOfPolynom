using System;
using System.Runtime.Serialization;

namespace MonomialParse
{
    [Serializable]
    internal class InvalidMonomialOperationException : Exception
    {
        public InvalidMonomialOperationException()
        {
        }

        public InvalidMonomialOperationException(string message) : base(message)
        {
        }

        public InvalidMonomialOperationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidMonomialOperationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}