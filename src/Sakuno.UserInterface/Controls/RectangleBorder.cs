using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Sakuno.UserInterface.Controls
{
    public class RectangleBorder : Decorator
    {
        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register(nameof(Background), typeof(Brush), typeof(RectangleBorder),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));

        public Brush Background
        {
            get => (Brush)GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }

        public static readonly DependencyProperty BorderBrushProperty =
            DependencyProperty.Register(nameof(BorderBrush), typeof(Brush), typeof(RectangleBorder),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender, OnBorderPenSubPropertyChanged));

        public Brush BorderBrush
        {
            get => (Brush)GetValue(BorderBrushProperty);
            set => SetValue(BorderBrushProperty, value);
        }

        public static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.Register(nameof(BorderThickness), typeof(Thickness), typeof(RectangleBorder),
                new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnBorderPenSubPropertyChanged));

        public Thickness BorderThickness
        {
            get => (Thickness)GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }

        static void OnBorderPenSubPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var border = (RectangleBorder)d;

            border._leftPen = null;
            border._topPen = null;
            border._rightPen = null;
            border._bottomPen = null;
        }

        public static readonly DependencyProperty IsLeftSideVisibleProperty =
            DependencyProperty.Register(nameof(IsLeftSideVisible), typeof(bool), typeof(RectangleBorder),
                new FrameworkPropertyMetadata(BoxedConstants.Boolean.True, FrameworkPropertyMetadataOptions.AffectsRender));

        public bool IsLeftSideVisible
        {
            get => (bool)GetValue(IsLeftSideVisibleProperty);
            set => SetValue(IsLeftSideVisibleProperty, BooleanUtil.GetBoxed(value));
        }

        public static readonly DependencyProperty IsTopSideVisibleProperty =
            DependencyProperty.Register(nameof(IsTopSideVisible), typeof(bool), typeof(RectangleBorder),
                new FrameworkPropertyMetadata(BoxedConstants.Boolean.True, FrameworkPropertyMetadataOptions.AffectsRender));

        public bool IsTopSideVisible
        {
            get => (bool)GetValue(IsTopSideVisibleProperty);
            set => SetValue(IsTopSideVisibleProperty, BooleanUtil.GetBoxed(value));
        }

        public static readonly DependencyProperty IsRightSideVisibleProperty =
            DependencyProperty.Register(nameof(IsRightSideVisible), typeof(bool), typeof(RectangleBorder),
                new FrameworkPropertyMetadata(BoxedConstants.Boolean.True, FrameworkPropertyMetadataOptions.AffectsRender));

        public bool IsRightSideVisible
        {
            get => (bool)GetValue(IsRightSideVisibleProperty);
            set => SetValue(IsRightSideVisibleProperty, BooleanUtil.GetBoxed(value));
        }

        public static readonly DependencyProperty IsBottomSideVisibleProperty =
            DependencyProperty.Register(nameof(IsBottomSideVisible), typeof(bool), typeof(RectangleBorder),
                new FrameworkPropertyMetadata(BoxedConstants.Boolean.True, FrameworkPropertyMetadataOptions.AffectsRender));

        public bool IsBottomSideVisible
        {
            get => (bool)GetValue(IsBottomSideVisibleProperty);
            set => SetValue(IsBottomSideVisibleProperty, BooleanUtil.GetBoxed(value));
        }

        public static readonly DependencyProperty PaddingProperty =
            DependencyProperty.Register(nameof(Padding), typeof(Thickness), typeof(RectangleBorder),
                new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        public Thickness Padding
        {
            get => (Thickness)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }

        Pen _leftPen, _topPen, _rightPen, _bottomPen;

        protected override Size MeasureOverride(Size constraint)
        {
            var thickness = BorderThickness;

            if (UseLayoutRounding)
                thickness = DpiUtil.RoundLayoutValue(thickness, VisualTreeHelper.GetDpi(this));

            var padding = Padding;
            var result = new Size(thickness.Left + thickness.Right + padding.Left + padding.Right, thickness.Top + thickness.Bottom + padding.Top + padding.Bottom);

            var child = Child;
            if (child != null)
            {
                child.Measure(new Size(Math.Max(constraint.Width - result.Width, 0), Math.Max(constraint.Height - result.Height, 0)));

                var desiredSize = child.DesiredSize;
                result = new Size(result.Width + desiredSize.Width, result.Height + desiredSize.Height);
            }

            return result;
        }
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var child = Child;
            if (child != null)
            {
                var thickness = BorderThickness;
                if (UseLayoutRounding)
                    thickness = DpiUtil.RoundLayoutValue(thickness, VisualTreeHelper.GetDpi(this));

                var padding = Padding;

                var rect = new Rect(thickness.Left + padding.Left, thickness.Top + padding.Top,
                    Math.Max(arrangeSize.Width - thickness.Left - thickness.Right - padding.Left - padding.Right, 0),
                    Math.Max(arrangeSize.Height - thickness.Top - thickness.Bottom - padding.Top - padding.Bottom, 0));

                child.Arrange(rect);
            }

            return arrangeSize;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var backgroundBrush = Background;
            if (backgroundBrush != null)
                drawingContext.DrawRectangle(backgroundBrush, null, new Rect(RenderSize));

            var thickness = BorderThickness;
            var isLeftSideVisible = IsLeftSideVisible;
            var isTopSideVisible = IsTopSideVisible;
            var isRightSideVisible = IsRightSideVisible;
            var isBottomSideVisible = IsBottomSideVisible;

            var width = RenderSize.Width;
            var height = RenderSize.Height;

            Brush penBrush = null;

            var useLayoutRounding = UseLayoutRounding;
            var dpi = VisualTreeHelper.GetDpi(this);

            if (isLeftSideVisible && thickness.Left > 0)
            {
                if (_leftPen == null)
                {
                    var penThickness = useLayoutRounding ? DpiUtil.RoundLayoutValue(thickness.Left, dpi.DpiScaleX) : thickness.Left;
                    _leftPen = new Pen(penBrush = BorderBrush, penThickness);

                    if (penBrush.IsFrozen)
                        _leftPen.Freeze();
                }

                var x = _leftPen.Thickness * .5;
                drawingContext.DrawLine(_leftPen, new Point(x, 0), new Point(x, height));
            }

            if (isTopSideVisible && thickness.Top > 0)
            {
                if (_topPen == null)
                {
                    if (penBrush == null)
                        penBrush = BorderBrush;

                    var penThickness = useLayoutRounding ? DpiUtil.RoundLayoutValue(thickness.Top, dpi.DpiScaleX) : thickness.Top;
                    _topPen = new Pen(penBrush, penThickness);

                    if (penBrush.IsFrozen)
                        _topPen.Freeze();
                }

                var y = _topPen.Thickness * .5;
                drawingContext.DrawLine(_topPen, new Point(0, y), new Point(width, y));
            }

            if (isRightSideVisible && thickness.Right > 0)
            {
                if (_rightPen == null)
                {
                    if (penBrush == null)
                        penBrush = BorderBrush;

                    var penThickness = useLayoutRounding ? DpiUtil.RoundLayoutValue(thickness.Right, dpi.DpiScaleX) : thickness.Right;
                    _rightPen = new Pen(penBrush, penThickness);

                    if (penBrush.IsFrozen)
                        _rightPen.Freeze();
                }

                var x = _rightPen.Thickness * .5;
                drawingContext.DrawLine(_rightPen, new Point(width - x, 0), new Point(width - x, height));
            }

            if (isBottomSideVisible && thickness.Bottom > 0)
            {
                if (_bottomPen == null)
                {
                    if (penBrush == null)
                        penBrush = BorderBrush;

                    var penThickness = useLayoutRounding ? DpiUtil.RoundLayoutValue(thickness.Bottom, dpi.DpiScaleX) : thickness.Bottom;
                    _bottomPen = new Pen(penBrush, penThickness);

                    if (penBrush.IsFrozen)
                        _bottomPen.Freeze();
                }

                var y = _bottomPen.Thickness * .5;
                drawingContext.DrawLine(_bottomPen, new Point(0, height - y), new Point(width, height - y));
            }
        }
    }
}
