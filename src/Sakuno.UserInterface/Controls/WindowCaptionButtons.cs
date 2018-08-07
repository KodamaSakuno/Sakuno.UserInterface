using Sakuno.SystemLayer;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace Sakuno.UserInterface.Controls
{
    [TemplatePart(Name = "PART_MinimizeButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_MaximizeButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_NormalizeButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_CloseButton", Type = typeof(Button))]
    public class WindowCaptionButtons : Control
    {
        Window _owner;
        IntPtr _ownerHandle;

        Button _minimizeButton;
        Button _maximizeButton;
        Button _normalizeButton;
        Button _closeButton;

        static WindowCaptionButtons()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowCaptionButtons), new FrameworkPropertyMetadata(typeof(WindowCaptionButtons)));
        }
        public WindowCaptionButtons()
        {
            AddHandler(Button.ClickEvent, new RoutedEventHandler(OnButtonClick));
        }

        void OnButtonClick(object sender, RoutedEventArgs e)
        {
            var source = e.OriginalSource;

            if (source == _minimizeButton)
                PostSystemCommand(NativeConstants.SystemCommand.SC_MINIMIZE);
            else if (source == _maximizeButton)
                PostSystemCommand(NativeConstants.SystemCommand.SC_MAXIMIZE);
            else if (source == _normalizeButton)
                PostSystemCommand(NativeConstants.SystemCommand.SC_RESTORE);
            else if (source == _closeButton)
                PostSystemCommand(NativeConstants.SystemCommand.SC_CLOSE);
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            _owner = Window.GetWindow(this);

            if (_owner == null)
                return;

            _ownerHandle = new WindowInteropHelper(_owner).Handle;
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _minimizeButton = Template.FindName("PART_MinimizeButton", this) as Button;
            _maximizeButton = Template.FindName("PART_MaximizeButton", this) as Button;
            _normalizeButton = Template.FindName("PART_NormalizeButton", this) as Button;
            _closeButton = Template.FindName("PART_CloseButton", this) as Button;
        }

        void PostSystemCommand(NativeConstants.SystemCommand systemCommand) =>
            NativeMethods.User32.PostMessageW(_ownerHandle, NativeConstants.WindowMessage.WM_SYSCOMMAND, (IntPtr)systemCommand, IntPtr.Zero);
    }
}
