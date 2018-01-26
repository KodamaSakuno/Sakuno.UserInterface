using System;
using System.Collections.ObjectModel;
using System.Windows.Markup;

namespace Sakuno.UserInterface.Markup
{
    public class IntegerSequenceExtension : MarkupExtension
    {
        public int First { get; set; }
        public int Count { get; set; }

        public int Step { get; set; } = 1;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var sequence = new int[Count];
            var current = First;

            for (var i = 0; i < Count; i++)
            {
                sequence[i] = current;
                current += Step;
            }

            return new ReadOnlyObservableCollection<int>(new ObservableCollection<int>(sequence));
        }
    }
}
