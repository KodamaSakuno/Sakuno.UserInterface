using System;

namespace Sakuno.UserInterface.Documents.BBCode
{
    public sealed class Tag : DocumentElement
    {
        public string Name { get; }

        public Parameter Parameter { get; }

        public DocumentElement Child { get; }

        internal Tag(string name)
        {
            Name = name;
        }
        internal Tag(string name, Parameter parameter, DocumentElement child)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Parameter = parameter;
            Child = child;
        }
    }
}
