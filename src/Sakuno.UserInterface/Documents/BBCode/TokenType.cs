namespace Sakuno.UserInterface.Documents.BBCode
{
    enum TokenType
    {
        EOS,

        Text,
        Identifier,
        ParameterValue,

        LeftBracket,
        LeftBracketWithSlash,
        RightBracket,
    }
}
