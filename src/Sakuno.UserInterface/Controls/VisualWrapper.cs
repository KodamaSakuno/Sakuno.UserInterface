using System;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace Sakuno.UserInterface.Controls
{
    [ContentProperty(nameof(Child))]
    public class VisualWrapper : UIElement
    {
        protected override int VisualChildrenCount => 1;

        Visual _child;
        public Visual Child
        {
            get => _child;
            set
            {
                if (_child == value)
                    return;

                if (_child != null)
                    RemoveVisualChild(_child);

                _child = value;

                if (_child != null)
                    AddVisualChild(_child);
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index != 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            return _child;
        }
    }
}
