using Sakuno.UserInterface.Controls;
using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace Sakuno.UserInterface.Shell
{
    class ModernWindowChromeEdge
    {
        ModernWindowChrome _owner;

        public GlowingEdge Edge { get; }
        public GlowingWindow Window { get; }

        public ModernWindowChromeEdge(ModernWindowChrome owner, IntPtr previousWindow, Dock position)
        {
            _owner = owner;

            Window = new GlowingWindow(owner, previousWindow, position);

            Edge = new GlowingEdge(position);
            Edge.SetBinding(Control.BackgroundProperty, new Binding(nameof(ModernWindow.BorderBrush)) { Source = owner.Owner });

            Window.RootVisual = Edge;
        }
    }
}
