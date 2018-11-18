using BetterThanIMDB.Models.Collections;
using BetterThanIMDB.ViewModels.Base;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BetterThanIMDB.ViewModels
{
    class MainPageViewModel : BaseViewModel
    {
        public ICommand OpenSettingsCommand { get; private set; }

        public MainPageViewModel()
        {
            OpenSettingsCommand = new Command(async() => await App.NavigationService.NavigateAsync("SettingsPage"));
        }
    }
}
