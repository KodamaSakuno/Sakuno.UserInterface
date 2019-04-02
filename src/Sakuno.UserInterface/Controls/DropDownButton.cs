using Sakuno.UserInterface.Controls.Primitives;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Sakuno.UserInterface.Controls
{
    [TemplatePart(Name = "PART_ToggleButton", Type = typeof(ToggleButton))]
    [TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
    public class DropDownButton : ButtonWithPopup
    {
        public static readonly DependencyProperty ShowDropDownMarkerProperty =
            DependencyProperty.Register(nameof(ShowDropDownMarker), typeof(bool), typeof(DropDownButton),
                new FrameworkPropertyMetadata(BoxedConstants.Boolean.True));

        public bool ShowDropDownMarker
        {
            get => (bool)GetValue(ShowDropDownMarkerProperty);
            set => SetValue(ShowDropDownMarkerProperty, BooleanUtil.GetBoxed(value));
        }

        static DropDownButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DropDownButton), new FrameworkPropertyMetadata(typeof(DropDownButton)));
        }
    }
}
