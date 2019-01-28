using System;
using System.Windows.Markup;
using System.Xaml;

namespace Sakuno.UserInterface.Markup
{
    public sealed class DocumentRootExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var rootObjectProvider = serviceProvider.GetService<IRootObjectProvider>() ??
                throw new InvalidOperationException("There is no RootObjectProvider service in the service provider.");

            return rootObjectProvider.RootObject;
        }
    }
}
