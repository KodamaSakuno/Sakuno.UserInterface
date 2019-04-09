using System.Collections.Generic;

namespace Sakuno.UserInterface.Documents.BBCode
{
    public sealed class ComplexParameter : Parameter
    {
        public IReadOnlyDictionary<string, string> Values { get; }

        public ComplexParameter(IReadOnlyDictionary<string, string> values)
        {
            Values = values;
        }
    }
}
