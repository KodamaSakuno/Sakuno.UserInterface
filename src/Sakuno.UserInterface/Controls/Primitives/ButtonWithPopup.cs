using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Sakuno.UserInterface.Controls.Primitives
{
    [TemplatePart(Name = "PART_ToggleButton", Type = typeof(ToggleButton))]
    [TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
    public abstract class ButtonWithPopup : HeaderedContentControl
    {
        public static readonly DependencyProperty PopupVerticalOffsetProperty =
            DependencyProperty.Register(nameof(PopupVerticalOffset), typeof(double), typeof(ButtonWithPopup),
                new FrameworkPropertyMetadata(BoxedConstants.Double.Zero));

        public double PopupVerticalOffset
        {
            get => (double)GetValue(PopupVerticalOffsetProperty);
            set => SetValue(PopupVerticalOffsetProperty, value);
        }

        public static readonly DependencyProperty PopupHorizontalOffsetProperty =
            DependencyProperty.Register(nameof(PopupHorizontalOffset), typeof(double), typeof(ButtonWithPopup),
                new FrameworkPropertyMetadata(BoxedConstants.Double.Zero));
        public double PopupHorizontalOffset
        {
            get => (double)GetValue(PopupHorizontalOffsetProperty);
            set => SetValue(PopupHorizontalOffsetProperty, value);
        }

        public static readonly DependencyProperty IsPopupOpenedProperty =
            DependencyProperty.Register(nameof(IsPopupOpened), typeof(bool), typeof(ButtonWithPopup),
                new FrameworkPropertyMetadata(BoxedConstants.Boolean.False, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool IsPopupOpened
        {
            get => (bool)GetValue(IsPopupOpenedProperty);
            set => SetValue(IsPopupOpenedProperty, BooleanUtil.GetBoxed(value));
        }
    }
}
