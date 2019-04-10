using System;
using System.Collections.Generic;

namespace Sakuno.UserInterface.Documents.BBCode
{
    public sealed class Document
    {
        internal static Document Empty { get; } = new Document(Array.Empty<DocumentElement>());

        public IReadOnlyList<DocumentElement> Elements { get; }

        internal Document(IReadOnlyList<DocumentElement> elements)
        {
            Elements = elements ?? throw new ArgumentNullException(nameof(elements));
        }
    }
}
