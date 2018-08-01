using System;
using System.Windows;

namespace Sakuno.UserInterface
{
    public class Theme
    {
        public string Name { get; }

        ResourceDictionary _resourceDictionary;
        public ResourceDictionary ResourceDictionary
        {
            get
            {
                if (_resourceDictionary == null)
                {
                    const string InternalUri = "pack://application:,,,/Sakuno.UserInterface;component/Themes/{0}.xaml";

                    string uriString;

                    switch (Name)
                    {
                        case nameof(Themes.Light):
                            uriString = string.Format(InternalUri, nameof(Themes.Light));
                            break;

                        case nameof(Themes.Dark):
                            uriString = string.Format(InternalUri, nameof(Themes.Dark));
                            break;

                        default: throw new InvalidOperationException();
                    }

                    _resourceDictionary = new ResourceDictionary() { Source = new Uri(uriString) };
                }

                return _resourceDictionary;
            }
        }

        internal Theme(string name)
        {
            if (name.IsNullOrEmpty())
                throw new ArgumentException(nameof(name));

            Name = name;
        }

        internal static string[] EnumerateRequiredKeys() => new[]
        {
            "ThemeColorKey",
            "ThemeBrushKey",

            "BackgroundColorKey",
            "BackgroundBrushKey",
            "BorderColorKey",
            "BorderBrushKey",
            "ForegroundColorKey",
            "ForegroundBrushKey",

            "ActiveBackgroundColorKey",
            "ActiveBackgroundBrushKey",
            "ActiveBorderColorKey",
            "ActiveBorderBrushKey",
            "ActiveForegroundColorKey",
            "ActiveForegroundBrushKey",

            "InactiveBackgroundColorKey",
            "InactiveBackgroundBrushKey",
            "InactiveBorderColorKey",
            "InactiveBorderBrushKey",
            "InactiveForegroundColorKey",
            "InactiveForegroundBrushKey",
        };
    }
}
