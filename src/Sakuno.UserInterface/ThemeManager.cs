using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Sakuno.UserInterface
{
    public sealed class ThemeManager : INotifyPropertyChanged
    {
        public static ThemeManager Instance { get; } = new ThemeManager();

        ResourceDictionary _root = new ResourceDictionary();

        Theme _theme;
        public Theme Theme
        {
            get => _theme;
            private set
            {
                _theme = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        ThemeManager() { }

        public void Initialize(Application app, Theme theme)
        {
            if (app.Resources.MergedDictionaries.Contains(_root))
                throw new InvalidOperationException("Duplicate initialization for this Application instance.");

            ChangeTheme(theme);

            app.Resources.MergedDictionaries.Add(_root);
        }

        public void ChangeTheme(Theme theme)
        {
            if (theme == null)
                throw new ArgumentNullException(nameof(theme));

            if (_theme == theme)
                return;

            Theme = theme;
            OverwriteItems(theme.ResourceDictionary, Theme.EnumerateRequiredKeys());
        }

        void OverwriteItems(ResourceDictionary dictionary, IEnumerable<string> keys)
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
