using System.Windows;
using System.Windows.Controls;

namespace Sakuno.UserInterface.Shell
{
    class GlowingEdge : Control
    {
        static readonly DependencyPropertyKey PositionPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(Position), typeof(Dock), typeof(GlowingEdge),
                new PropertyMetadata(EnumUtil.GetBoxed(Dock.Left)));

        public static readonly DependencyProperty PositionProperty = PositionPropertyKey.DependencyProperty;

        public Dock Position
        {
            get => (Dock)GetValue(PositionProperty);
            private set => SetValue(PositionPropertyKey, EnumUtil.GetBoxed(value));
        }

        static GlowingEdge()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GlowingEdge), new FrameworkPropertyMetadata(typeof(GlowingEdge)));

            SnapsToDevicePixelsProperty.OverrideMetadata(typeof(GlowingEdge), new FrameworkPropertyMetadata(BooleanUtil.True));
            UseLayoutRoundingProperty.OverrideMetadata(typeof(GlowingEdge), new FrameworkPropertyMetadata(BooleanUtil.True));
        }
        public GlowingEdge(Dock position)
        {
            Position = position;
        }
    }
}
