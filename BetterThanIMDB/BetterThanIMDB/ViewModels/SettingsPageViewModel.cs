using BetterThanIMDB.CustomSettings;
using BetterThanIMDB.Models;
using BetterThanIMDB.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace BetterThanIMDB.ViewModels
{
    class SettingsPageViewModel : BaseViewModel
    {
        private bool _english = false;
        private bool _russian = false;
        private bool _downloadImages;

        public ICommand ApplyChangesCommand { get; private set; }

        public SettingsPageViewModel()
        {
            ApplyChangesCommand = new Command(ApplyChanges);

            switch (Settings.Instance.Culture.Name)
            {
                case "en-US":
                    English = true;
                    break;
                case "ru-RU":
                    Russian = true;
                    break;
            }
            _downloadImages = Settings.Instance.DownloadImages;
            
        }

        public bool English
        {
            get => _english;
            set
            {
                _english = value;
                _russian = !value;
                OnPropertyChanged("Russian");
                OnPropertyChanged("English");
            }
        }

        public bool Russian
        {
            get => _russian;
            set
            {
                _russian = value;
                _english = !value;
                OnPropertyChanged("Russian");
                OnPropertyChanged("English");
            }
        }

        public bool DownloadImages
        {
            get => _downloadImages;
            set
            {
                _downloadImages = value;
                OnPropertyChanged("DownloadImages");
            }
        }

        private async void ApplyChanges()
        {
            Settings.Instance.Culture = Russian ? new CultureInfo("ru-RU") : new CultureInfo("en-US");
            Settings.Instance.DownloadImages = DownloadImages;
            await DataHelper.SaveAppSettings();
            App.Refresh();
        }
        
    }
}
