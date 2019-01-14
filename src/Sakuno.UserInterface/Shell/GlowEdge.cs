using System.Windows;
using System.Windows.Controls;

namespace Sakuno.UserInterface.Shell
{
    class GlowEdge : Control
    {
        static readonly DependencyPropertyKey PositionPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(Position), typeof(Dock), typeof(GlowEdge),
                new PropertyMetadata(EnumUtil.GetBoxed(Dock.Left)));

        public static readonly DependencyProperty PositionProperty = PositionPropertyKey.DependencyProperty;

        public Dock Position
        {
            get => (Dock)GetValue(PositionProperty);
            private set => SetValue(PositionPropertyKey, EnumUtil.GetBoxed(value));
        }

        static GlowEdge()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GlowEdge), new FrameworkPropertyMetadata(typeof(GlowEdge)));

            SnapsToDevicePixelsProperty.OverrideMetadata(typeof(GlowEdge), new FrameworkPropertyMetadata(BoxedConstants.Boolean.True));
            UseLayoutRoundingProperty.OverrideMetadata(typeof(GlowEdge), new FrameworkPropertyMetadata(BoxedConstants.Boolean.True));
        }
        public GlowEdge(Dock position)
        {
            Position = position;
        }
    }
}
