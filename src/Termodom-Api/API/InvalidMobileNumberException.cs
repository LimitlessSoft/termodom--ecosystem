using System;
using System.Runtime.Serialization;

namespace API
{
    [Serializable]
    internal class InvalidMobileNumberException : Exception
    {
        public InvalidMobileNumberException()
        {
        }

        public InvalidMobileNumberException(string message) : base(message)
        {
        }

        public InvalidMobileNumberException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidMobileNumberException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public string RawNumber { get; set; }
    }
}