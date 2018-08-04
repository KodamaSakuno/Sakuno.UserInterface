using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Sakuno.UserInterface
{
    public class ThemedApp : Application, INotifyPropertyChanged
    {
        ResourceDictionary _root = new ResourceDictionary();

        Theme _theme;
        public Theme Theme
        {
            get => _theme;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                if (_theme == value)
                    return;

                OverwriteItems(value.ResourceDictionary, Theme.EnumerateRequiredKeys());

                _theme = value;
                NotifyPropertyChanged();
            }
        }

        Accent _accent;
        public Accent Accent
        {
            get => _accent;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                if (_accent == value)
                    return;

                OverwriteItems(value.ResourceDictionary, Accent.EnumerateRequiredKeys());

                _accent = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ThemedApp()
        {
            Theme = Themes.Light;
            Accent = Accents.Blue;

            Resources.MergedDictionaries.Add(_root);
        }

        void OverwriteItems(ResourceDictionary dictionary, string[] keys)
        {
            foreach (var key in keys)
                if (!dictionary.Contains(key))
                    throw new KeyNotFoundException(key);

            foreach (var key in keys)
                _root[key] = dictionary[key];
        }

        void NotifyPropertyChanged([CallerMemberName] string property = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    }
}
