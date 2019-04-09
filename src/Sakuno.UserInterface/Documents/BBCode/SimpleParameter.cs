using System;

namespace Sakuno.UserInterface.Documents.BBCode
{
    public sealed class SimpleParameter : Parameter
    {
        public string Value { get; }

        public SimpleParameter(string value)
        {
            if (value.IsNullOrWhiteSpace())
                throw new ArgumentException(nameof(value));

            Value = value;
        }
    }
}
