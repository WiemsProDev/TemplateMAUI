using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Template.Interfaces;

namespace Template.Utils
{
    // You exclude the 'Extension' suffix when using in XAML
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        readonly CultureInfo ci = null;
        const string ResourceId = "Template.Resources.AppResources";
        private static readonly Lazy<ResourceManager> lazy = new(
                    () => new ResourceManager(ResourceId, IntrospectionExtensions.GetTypeInfo(typeof(TranslateExtension)).Assembly));
        static readonly Lazy<ResourceManager> ResMgr = lazy;

        public string Text { get; set; }

        public TranslateExtension()
        {
            if (Microsoft.Maui.Devices.DeviceInfo.Platform == DevicePlatform.iOS  || Microsoft.Maui.Devices.DeviceInfo.Platform == DevicePlatform.Android)
            {
                ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            }
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return string.Empty;

            var translation = ResMgr.Value.GetString(Text, ci);
            if (translation == null)
            {
#if DEBUG
                throw new ArgumentException(
                    string.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", Text, ResourceId, ci.Name),
                    "Text");
#else
                translation = Text; // HACK: returns the key, which GETS DISPLAYED TO THE USER
#endif
            }
            return translation;
        }
    }
}