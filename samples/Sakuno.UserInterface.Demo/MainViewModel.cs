using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sakuno.UserInterface.Demo
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

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            _theme = App.Theme;
        }

        void NotifyPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
