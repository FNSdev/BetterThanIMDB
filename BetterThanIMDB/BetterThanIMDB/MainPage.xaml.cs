using BetterThanIMDB.Models;
using BetterThanIMDB.ViewModels;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Xamarin.Forms;

namespace BetterThanIMDB
{
	public partial class MainPage : TabbedPage
	{
		public MainPage()
		{
            BindingContext = new MainPageViewModel();
            InitializeComponent();
        }
    }
}
