using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BetterThanIMDB.CustomSettings
{
    public class Settings
    {
        private static Settings _instance;

        private CultureInfo _culture = new CultureInfo("ru-RU");
        private bool _downloadImages = true;

        public static Settings Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new Settings();
                }
                return _instance;
            }
        }

        private Settings()
        {

        }

        public CultureInfo Culture
        {
            get => _culture;
            set
            {
                _culture = value;
            }
        }

        public bool DownloadImages
        {
            get => _downloadImages;
            set
            {
                _downloadImages = value;
            }
        }
        
    }
}
