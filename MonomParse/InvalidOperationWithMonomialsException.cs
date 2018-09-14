using System;
using System.Runtime.Serialization;

namespace MonomialParse
{
    [Serializable]
    internal class InvalidOperationWithMonomialsException : Exception
    {
        public InvalidOperationWithMonomialsException()
        {
        }

        public InvalidOperationWithMonomialsException(string message) : base(message)
        {
        }

        public InvalidOperationWithMonomialsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidOperationWithMonomialsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}