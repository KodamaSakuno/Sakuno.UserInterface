namespace Sakuno.UserInterface.Documents.BBCode
{
    public sealed class TextElement : DocumentElement
    {
        public string Text { get; }

        internal TextElement(string text)
        {
            Text = text;
        }
    }
}
