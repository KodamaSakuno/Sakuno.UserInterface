using System;
using System.Windows;

namespace Sakuno.UserInterface
{
    public class Accent
    {
        public string Name { get; }

        ResourceDictionary _resourceDictionary;
        public ResourceDictionary ResourceDictionary
        {
            get
            {
                if (_resourceDictionary == null)
                {
                    const string InternalUri = "pack://application:,,,/Sakuno.UserInterface;component/Themes/Accents/{0}.xaml";

                    string uriString;

                    switch (Name)
                    {
                        case nameof(Accents.Blue):
                            uriString = string.Format(InternalUri, nameof(Accents.Blue));
                            break;

                        case nameof(Accents.Brown):
                            uriString = string.Format(InternalUri, nameof(Accents.Brown));
                            break;

                        default: throw new InvalidOperationException();
                    }

                    _resourceDictionary = new ResourceDictionary() { Source = new Uri(uriString) };
                }

                return _resourceDictionary;
            }
        }

        internal Accent(string name)
        {
            if (name.IsNullOrEmpty())
                throw new ArgumentException(nameof(name));

            Name = name;
        }

        internal static string[] EnumerateRequiredKeys() => new[]
        {
            "AccentColorKey",
            "AccentBrushKey",
            "AccentForegroundColorKey",
            "AccentForegroundBrushKey",
        };
    }
}
