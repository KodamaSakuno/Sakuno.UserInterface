using System;

namespace Sakuno.UserInterface.Documents.BBCode
{
    public sealed class BBCodeParseException : Exception
    {
        public BBCodeParseException() { }
        public BBCodeParseException(string message) : base(message) { }
        public BBCodeParseException(string message, Exception innerException) : base(message, innerException) { }
    }
}
