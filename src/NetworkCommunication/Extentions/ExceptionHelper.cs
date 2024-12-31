using System.Runtime.Serialization;

namespace NetworkCommunications.Extentions
{
    [Serializable]
    public class ParsingException : Exception
    {
        // Default constructor
        public ParsingException() : base() { }

        // Constructor that accepts a message
        public ParsingException(string message) : base(message) { }

        // Constructor that accepts a message and an inner exception
        public ParsingException(string message, Exception innerException) : base(message, innerException) { }

        // Constructor for serialization
        protected ParsingException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }



}
