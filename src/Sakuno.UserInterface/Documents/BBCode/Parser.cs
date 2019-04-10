using System;
using System.Buffers;
using System.Collections.Generic;

namespace Sakuno.UserInterface.Documents.BBCode
{
    struct Parser
    {
        static ReadOnlyMemory<char> _breakline = "br".AsMemory();

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
            if (_code.IsNullOrEmpty())
                return Document.Empty;

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

            return new TextElement(token.Segment);
        }
        Tag ParseTag()
        {
            Expect(TokenType.LeftBracket);

            var token = Lex();
            var tagName = token.Segment;

            if (MemoryExtensions.Equals(tagName.Span, _breakline.Span, StringComparison.OrdinalIgnoreCase))
            {
                Expect(TokenType.RightBracket);

                return KnownTags.Breakline;
            }

            Parameter parameter = null;

            if (_lookahead.Type == TokenType.Assign)
            {
                Expect(TokenType.Assign);

                token = Lex();
                parameter = new SimpleParameter(token.Segment);
            }

            Expect(TokenType.RightBracket);

            var child = ParseChild();

            Expect(TokenType.LeftBracketWithSlash);

            token = Lex();
            var closingTagName = token.Segment;
            if (!MemoryExtensions.Equals(tagName.Span, closingTagName.Span, StringComparison.OrdinalIgnoreCase))
                throw new BBCodeParseException();

            Expect(TokenType.RightBracket);

#if !NET462
            return new Tag(new string(tagName.Span), parameter, child);
#else
            return new Tag(tagName.ToString(), parameter, child);
#endif
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
