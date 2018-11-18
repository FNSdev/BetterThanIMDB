using BetterThanIMDB.Models;
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
	public partial class EditProducerPage : ContentPage
	{
		public EditProducerPage (Producer producer)
		{
            BindingContext = new EditProducerPageViewModel(producer);
			InitializeComponent ();
		}
	}
}