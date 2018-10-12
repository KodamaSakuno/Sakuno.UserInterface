using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Sakuno.UserInterface.Demo.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        ThemedApp App => Application.Current as ThemedApp;

        Theme _theme;
        public Theme Theme
        {
            get => _theme;
            set
            {
                if (_theme != value)
                {
                    App.Theme = value;
                    _theme = value;
                    NotifyPropertyChanged();
                }
            }
        }

        Accent _accent;
        public Accent Accent
        {
            get => _accent;
            set
            {
                if (_accent != value)
                {
                    App.Accent = value;
                    _accent = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return;

            _theme = App.Theme;
            _accent = App.Accent;
        }

        void NotifyPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
