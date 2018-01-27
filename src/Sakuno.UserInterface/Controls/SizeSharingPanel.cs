using System;
using System.Windows;
using System.Windows.Controls;

namespace Sakuno.UserInterface.Controls
{
    public class SizeSharingPanel : Panel
    {
        protected override Size MeasureOverride(Size constraint)
        {
            var result = default(Size);
            var children = InternalChildren;
            var count = children.Count;

            for (var i = 0; i < count; i++)
            {
                var element = children[i];

                element.Measure(constraint);

                result.Width = Math.Max(result.Width, element.DesiredSize.Width);
                result.Height = Math.Max(result.Height, element.DesiredSize.Height);
            }

            return result;
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var children = InternalChildren;
            var count = children.Count;
            var rect = new Rect(arrangeSize);

            for (var i = 0; i < count; i++)
            {
                var element = children[i];

                element.Arrange(rect);
            }

            return arrangeSize;
        }
    }
}
