using System;

namespace Sakuno.UserInterface.Documents.BBCode
{
    readonly struct Token
    {
        public TokenType Type { get; }
        public ReadOnlyMemory<char> Segment { get; }

        public Token(TokenType type, ReadOnlyMemory<char> segment)
        {
            Type = type;
            Segment = segment;
        }
    }
}
