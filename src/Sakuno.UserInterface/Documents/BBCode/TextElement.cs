using System;

namespace Sakuno.UserInterface.Documents.BBCode
{
    public sealed class TextElement : DocumentElement
    {
        public ReadOnlyMemory<char> Text { get; }

        internal TextElement(ReadOnlyMemory<char> text)
        {
            Text = text;
        }
    }
}
