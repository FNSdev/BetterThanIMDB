﻿using BetterThanIMDB.Models;
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
	public partial class EditActorPage : ContentPage
	{
		public EditActorPage (Actor actor)
		{
            BindingContext = new EditActorPageViewModel(actor);
			InitializeComponent ();
		}
	}
}