using System.Windows;

namespace Sakuno.UserInterface.Demo
{
    partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ThemeManager.Instance.Initialize(this, Themes.Light);

            base.OnStartup(e);
        }
    }
}
