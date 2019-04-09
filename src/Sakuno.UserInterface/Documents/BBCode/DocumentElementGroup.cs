using System;
using System.Collections.Generic;

namespace Sakuno.UserInterface.Documents.BBCode
{
    public sealed class DocumentElementGroup : DocumentElement
    {
        public IReadOnlyList<DocumentElement> Elements { get; }

        internal DocumentElementGroup(IReadOnlyList<DocumentElement> elements)
        {
            Elements = elements ?? throw new ArgumentNullException(nameof(elements));
        }
    }
}
