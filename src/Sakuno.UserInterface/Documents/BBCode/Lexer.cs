namespace Sakuno.UserInterface.Documents.BBCode
{
    struct Lexer
    {
        string _code;
        int _position;

        bool _inTag;

        public Lexer(string code)
        {
            _code = code;
            _position = -1;

            _inTag = false;
        }

        void Advance() => _position++;

        char Read()
        {
            Advance();
            return _code[_position];
        }
        char Peek()
        {
            if (_position + 1 >= _code.Length)
                return '\0';

            return _code[_position + 1];
        }

        public Token Scan()
        {
            while (_position + 1 < _code.Length)
            {
                switch (Read())
                {
                    case '[':
                        var isSlash = Peek() == '/';
                        if (isSlash)
                            Advance();

                        _inTag = true;
                        return Return(!isSlash ? TokenType.LeftBracket : TokenType.LeftBracketWithSlash, _position, !isSlash ? 1 : 2);

                    case ']':
                        _inTag = false;
                        return Return(TokenType.RightBracket, _position);

                    default:
                        var startPosition = _position;

                        if (!_inTag)
                        {
                            while (true)
                            {
                                var c = Peek();
                                if (c == '[' || c == '\0')
                                    break;

                                Advance();
                            }

                            return Return(TokenType.Text, startPosition, _position - startPosition + 1);
                        }
                        else
                        {
                            while (true)
                            {
                                var c = Peek();
                                if (!char.IsLetterOrDigit(c))
                                    break;

                                Advance();
                            }

                            return Return(TokenType.Identifier, startPosition, _position - startPosition + 1);
                        }
                }
            }

            return Return(TokenType.EOS, _position);
        }

        Token Return(TokenType type, int startPosition, int length = 1) => new Token(type, startPosition, length);
    }
}
