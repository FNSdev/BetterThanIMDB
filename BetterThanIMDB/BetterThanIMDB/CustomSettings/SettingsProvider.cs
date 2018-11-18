using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BetterThanIMDB.CustomSettings
{
    class SettingsProvider
    {
        public SettingsProvider() { }
        public string Culture { get; set; }
        public bool DownloadImages { get; set; }
    }
}
