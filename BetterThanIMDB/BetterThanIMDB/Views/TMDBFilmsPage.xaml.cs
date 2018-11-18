using BetterThanIMDB.ViewModels;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BetterThanIMDB.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TMDBFilmsPage : ContentPage
	{
		public TMDBFilmsPage ()
		{
            BindingContext = new TMDBFilmsPageViewModel();
			InitializeComponent ();

            if (!CrossConnectivity.Current.IsConnected)
            {
                Animation.Animation = "network_error.json";
            }
        }
	}
}