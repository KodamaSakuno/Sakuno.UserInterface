namespace Sakuno.UserInterface.Documents.BBCode
{
    readonly struct Token
    {
        public TokenType Type { get; }
        public int Position { get; }
        public int Length { get; }

        public Token(TokenType type, int position, int length)
        {
            Type = type;
            Position = position;
            Length = length;
        }
    }
}
