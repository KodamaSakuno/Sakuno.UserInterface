using System;

namespace Sakuno.UserInterface.Documents.BBCode
{
    public sealed class Tag : DocumentElement
    {
        public string Name { get; }

        public DocumentElement Child { get; }

        internal Tag(string name, DocumentElement child)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Child = child;
        }
    }
}
