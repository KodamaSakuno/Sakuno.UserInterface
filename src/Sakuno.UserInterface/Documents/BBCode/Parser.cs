using System;
using System.Collections.Generic;

namespace Sakuno.UserInterface.Documents.BBCode
{
    struct Parser
    {
        string _code;
        Lexer _lexer;

        Token _lookahead;

        public Parser(string code)
        {
            _code = code;
            _lexer = new Lexer(code);

            _lookahead = default;
        }

        Token Lex()
        {
            var result = _lookahead;

            _lookahead = _lexer.Scan();

            return result;
        }
        void Expect(TokenType type)
        {
            var token = Lex();
            if (token.Type != type)
                throw new BBCodeParseException();
        }

        public Document Parse()
        {
            _lookahead = _lexer.Scan();

            var elements = ParseElements();

            return new Document(elements);
        }
        IReadOnlyList<DocumentElement> ParseElements()
        {
            var result = new List<DocumentElement>();

            while (_lookahead.Type != TokenType.EOS)
                result.Add(ParseElement());

            return result;
        }

        DocumentElement ParseElement()
        {
            switch (_lookahead.Type)
            {
                case TokenType.Text:
                    return ParseText();

                case TokenType.LeftBracket:
                    return ParseTag();

                default: throw new BBCodeParseException();
            }
        }

        TextElement ParseText()
        {
            var token = Lex();

            return new TextElement(token.Length > 0 ? _code.Substring(token.Position, token.Length) : string.Empty);
        }
        Tag ParseTag()
        {
            Expect(TokenType.LeftBracket);

            var token = Lex();
            var tagName = _code.Substring(token.Position, token.Length);

            Expect(TokenType.RightBracket);

            var child = ParseChild();

            Expect(TokenType.LeftBracketWithSlash);

            token = Lex();
            var closingTagName = _code.Substring(token.Position, token.Length);
            if (!tagName.OICEquals(closingTagName))
                throw new BBCodeParseException();

            Expect(TokenType.RightBracket);

            return new Tag(tagName, child);
        }
        DocumentElement ParseChild()
        {
            DocumentElement result = null;
            List<DocumentElement> children = null;

            while (_lookahead.Type != TokenType.LeftBracketWithSlash)
            {
                var element = ParseElement();

                if (result == null)
                {
                    result = element;
                    continue;
                }

                if (children == null)
                    children = new List<DocumentElement>() { result };

                children.Add(element);
            }

            if (children != null)
                result = new DocumentElementGroup(children.ToArray());

            return result;
        }
    }
}
