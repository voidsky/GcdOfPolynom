using System;
using System.Runtime.Serialization;

namespace MonomialParse
{
    [Serializable]
    internal class MonomialsCannotBeAddedException : Exception
    {
        public MonomialsCannotBeAddedException()
        {
        }

        public MonomialsCannotBeAddedException(string message) : base(message)
        {
        }

        public MonomialsCannotBeAddedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MonomialsCannotBeAddedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}