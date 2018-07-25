using System;
using System.Collections.Generic;
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

        internal static IEnumerable<string> EnumerateRequiredKeys()
        {
            yield return "ThemeColorKey";
            yield return "ThemeBrushKey";

            yield return "BackgroundColorKey";
            yield return "BackgroundBrushKey";
            yield return "BorderColorKey";
            yield return "BorderBrushKey";
            yield return "ForegroundColorKey";
            yield return "ForegroundBrushKey";

            yield return "ActiveBackgroundColorKey";
            yield return "ActiveBackgroundBrushKey";
            yield return "ActiveBorderColorKey";
            yield return "ActiveBorderBrushKey";
            yield return "ActiveForegroundColorKey";
            yield return "ActiveForegroundBrushKey";

            yield return "InactiveBackgroundColorKey";
            yield return "InactiveBackgroundBrushKey";
            yield return "InactiveBorderColorKey";
            yield return "InactiveBorderBrushKey";
            yield return "InactiveForegroundColorKey";
            yield return "InactiveForegroundBrushKey";
        }
    }
}
