using Acr.UserDialogs;
using BetterThanIMDB.Models;
using BetterThanIMDB.Services;
using BetterThanIMDB.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace BetterThanIMDB
{
	public partial class App : Application
	{
        private static App instance;

		public App ()
		{
			InitializeComponent();

            NavigationService.Configure("MainPage", typeof(MainPage));
            NavigationService.Configure("FilmsPage", typeof(FilmsPage));
            NavigationService.Configure("ActorsPage", typeof(ActorsPage));
            NavigationService.Configure("ProducersPage", typeof(ProducersPage));
            NavigationService.Configure("EditFilmPage", typeof(EditFilmPage));
            NavigationService.Configure("EditActorPage", typeof(EditActorPage));
            NavigationService.Configure("EditProducerPage", typeof(EditProducerPage));
            NavigationService.Configure("TMDBFilmsPage", typeof(TMDBFilmsPage));
            NavigationService.Configure("TMDBPersonPage", typeof(TMDBPersonPage));
            NavigationService.Configure("SearchFilmsPage", typeof(SearchFilmsPage));
            NavigationService.Configure("SearchActorsPage", typeof(SearchActorsPage));
            NavigationService.Configure("SearchProducersPage", typeof(SearchProducersPage));
            NavigationService.Configure("SettingsPage", typeof(SettingsPage));

            var mainPage = ((NavigationService)NavigationService).SetRootPage("MainPage");

            MainPage = mainPage;

            instance = this;
            
		}

        public static INavigationService NavigationService { get; } = new NavigationService(); 
        public static void Refresh()
        {
            var mainPage = ((NavigationService)NavigationService).SetRootPage("MainPage");
            instance.MainPage = mainPage;
        }

		protected override void OnStart ()
		{
            // Handle when your app starts
        }

		protected override void OnSleep ()
		{
        }

		protected override void OnResume ()
		{
            // Handle when your app resumes
        }
	}
}
