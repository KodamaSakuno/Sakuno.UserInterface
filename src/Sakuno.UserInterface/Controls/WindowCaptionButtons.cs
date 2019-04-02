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
    public sealed class WindowCaptionButtons : Control
    {
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

            if (_ownerHandle == IntPtr.Zero)
            {
                var owner = Window.GetWindow(this);

                _ownerHandle = new WindowInteropHelper(owner).Handle;
            }

            if (source == _minimizeButton)
                PostSystemCommand(NativeConstants.SystemCommand.SC_MINIMIZE);
            else if (source == _maximizeButton)
                PostSystemCommand(NativeConstants.SystemCommand.SC_MAXIMIZE);
            else if (source == _normalizeButton)
                PostSystemCommand(NativeConstants.SystemCommand.SC_RESTORE);
            else if (source == _closeButton)
                PostSystemCommand(NativeConstants.SystemCommand.SC_CLOSE);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _minimizeButton = GetTemplateChild("PART_MinimizeButton") as Button;
            _maximizeButton = GetTemplateChild("PART_MaximizeButton") as Button;
            _normalizeButton = GetTemplateChild("PART_NormalizeButton") as Button;
            _closeButton = GetTemplateChild("PART_CloseButton") as Button;
        }

        void PostSystemCommand(NativeConstants.SystemCommand systemCommand) =>
            NativeMethods.User32.PostMessageW(_ownerHandle, NativeConstants.WindowMessage.WM_SYSCOMMAND, (IntPtr)systemCommand, IntPtr.Zero);
    }
}
