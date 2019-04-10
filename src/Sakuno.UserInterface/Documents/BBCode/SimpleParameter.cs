using System;

namespace Sakuno.UserInterface.Documents.BBCode
{
    public sealed class SimpleParameter : Parameter
    {
        public ReadOnlyMemory<char> Value { get; }

        public SimpleParameter(ReadOnlyMemory<char> value)
        {
            if (value.IsEmpty)
                throw new ArgumentException(nameof(value));

            Value = value;
        }
    }
}
