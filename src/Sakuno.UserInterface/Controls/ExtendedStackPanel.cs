using System;
using System.Windows;
using System.Windows.Controls;

namespace Sakuno.UserInterface.Controls
{
    public class ExtendedStackPanel : Panel
    {

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(ExtendedStackPanel),
                new FrameworkPropertyMetadata(EnumUtil.GetBoxed(Orientation.Vertical),
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, EnumUtil.GetBoxed(value));
        }

        public static readonly DependencyProperty SpacingProperty =
            DependencyProperty.Register(nameof(Spacing), typeof(double), typeof(ExtendedStackPanel),
                new FrameworkPropertyMetadata(BoxedConstants.Double.Zero,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        public double Spacing
        {
            get => (double)GetValue(SpacingProperty);
            set => SetValue(SpacingProperty, value);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var children = InternalChildren;
            var count = children.Count;
            var isVertical = Orientation == Orientation.Vertical;
            var width = .0;
            var height = .0;
            var spacing = Spacing;
            if (isVertical)
                height =  -spacing;
            else
                width = -spacing;

            for (var i = 0; i < count; i++)
            {
                var element = children[i];

                element.Measure(availableSize);

                var desiredSize = element.DesiredSize;

                if (isVertical)
                {
                    height += desiredSize.Height + spacing;
                    width = Math.Max(width, desiredSize.Width);
                }
                else
                {
                    width += desiredSize.Width + spacing;
                    height = Math.Max(height, desiredSize.Height);
                }
            }

            return new Size(width, height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var children = InternalChildren;
            var count = children.Count;
            var isVertical = Orientation == Orientation.Vertical;
            var rect = new Rect(finalSize);
            var spacing = Spacing;
            var previousSize = -spacing;

            for (var i = 0; i < count; i++)
            {
                var element = children[i];

                if (isVertical)
                {
                    rect.Y += previousSize + spacing;
                    previousSize = rect.Height = element.DesiredSize.Height;
                    rect.Width = Math.Max(finalSize.Width, element.DesiredSize.Width);
                }
                else
                {
                    rect.X += previousSize + spacing;
                    previousSize = rect.Width = element.DesiredSize.Width;
                    rect.Height = Math.Max(finalSize.Height, element.DesiredSize.Height);
                }

                element.Arrange(rect);
            }

            return finalSize;
        }
    }
}
