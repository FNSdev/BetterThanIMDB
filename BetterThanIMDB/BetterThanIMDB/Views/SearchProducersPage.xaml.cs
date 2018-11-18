using BetterThanIMDB.ViewModels;
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
	public partial class SearchProducersPage : ContentPage
	{
		public SearchProducersPage (ProducersPageViewModel vm)
		{
            BindingContext = new SearchProducersPageViewModel(vm);
			InitializeComponent ();
		}
	}
}