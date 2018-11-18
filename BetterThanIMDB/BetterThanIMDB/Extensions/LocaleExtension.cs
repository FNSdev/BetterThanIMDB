using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms.Xaml;

using BetterThanIMDB.CustomSettings;
using System.Resources;
using System.Reflection;

namespace BetterThanIMDB.Extensions
{
    class LocaleExtension : IMarkupExtension
    {
        private const string _id = "BetterThanIMDB.Resources.locale";

        private CultureInfo _culture = Settings.Instance.Culture;
        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if(Text == null || Text == string.Empty)
            {
                return string.Empty;
            }

            ResourceManager manager = new ResourceManager(_id, typeof(LocaleExtension).GetTypeInfo().Assembly);

            string translation = manager.GetString(Text, _culture);

            if(translation == null)
            {
                translation = Text;
            }
            return translation;
        }
    }
}
