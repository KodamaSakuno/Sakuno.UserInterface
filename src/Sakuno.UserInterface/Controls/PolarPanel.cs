using System;
using System.Windows;
using System.Windows.Controls;

namespace Sakuno.UserInterface.Controls
{
    public class PolarPanel : Panel
    {
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.RegisterAttached("Radius", typeof(double), typeof(PolarPanel),
                new FrameworkPropertyMetadata(BoxedConstants.Double.Zero, OnPositioningChanged));

        public static double GetRadius(UIElement element) =>
            (double)element.GetValue(RadiusProperty);
        public static void SetRadius(UIElement element, double value) =>
            element.SetValue(RadiusProperty, value);

        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.RegisterAttached("Angle", typeof(double), typeof(PolarPanel),
                new FrameworkPropertyMetadata(BoxedConstants.Double.Zero, OnPositioningChanged));

        public static double GetAngle(UIElement element) =>
            (double)element.GetValue(AngleProperty);
        public static void SetAngle(UIElement element, double value) =>
            element.SetValue(AngleProperty, value);

        static void OnPositioningChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is UIElement element))
                return;

            var parent = element.GetParent<PolarPanel>();

            parent?.InvalidateArrange();
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var children = InternalChildren;
            var count = children.Count;

            for (var i = 0; i < count; i++)
            {
                var element = children[i];

                element.Measure(constraint);
            }

            return default;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var children = InternalChildren;
            var count = children.Count;

            var centerPoint = new Point(finalSize.Width / 2, finalSize.Height / 2);

            for (var i = 0; i < count; i++)
            {
                var element = children[i];
                var size = element.DesiredSize;
                var angle = MathUtil.DegreesToRadians(GetAngle(element));
                var radius = GetRadius(element);

                var x = centerPoint.X + radius * Math.Cos(angle) - size.Width / 2;
                var y = centerPoint.Y + radius * Math.Sin(angle) - size.Height / 2;

                element.Arrange(new Rect(x, y, size.Width, size.Height));
            }

            return finalSize;
        }
    }
}
