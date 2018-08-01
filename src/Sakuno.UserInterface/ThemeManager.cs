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

        Accent _accent;
        public Accent Accent
        {
            get => _accent;
            private set
            {
                _accent = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        ThemeManager() { }

        public void Initialize(Application app, Theme theme, Accent accent)
        {
            if (app.Resources.MergedDictionaries.Contains(_root))
                throw new InvalidOperationException("Duplicate initialization for this Application instance.");

            ChangeTheme(theme);
            ChangeAccent(accent);

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
        public void ChangeAccent(Accent accent)
        {
            if (accent == null)
                throw new ArgumentNullException(nameof(accent));

            if (_accent == accent)
                return;

            Accent = accent;
            OverwriteItems(accent.ResourceDictionary, Accent.EnumerateRequiredKeys());
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
