using Acr.UserDialogs;
using BetterThanIMDB.Models;
using BetterThanIMDB.Models.Collections;
using BetterThanIMDB.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace BetterThanIMDB.ViewModels
{
    class SearchActorsPageViewModel : BaseViewModel
    {
        private CustomObservableCollection<Film> _allFilms = FilmCollection.Instance.Films;
        private CustomObservableCollection<Film> _films = new CustomObservableCollection<Film>();

        private string _name = "Actor";
        private string _minDate = "01.01.1900";
        private string _maxDate = "01.01.2020";

        public ICommand AddFilmCommand { get; private set; }
        public ICommand RemoveFilmCommand { get; private set; }
        public ICommand ApplyCommand { get; private set; }
        public ICommand PickMinDateCommand { get; private set; }
        public ICommand PickMaxDateCommand { get; private set; }

        public SearchActorsPageViewModel(ActorsPageViewModel vm)
        {
            ResourceManager manager = new ResourceManager("BetterThanIMDB.Resources.locale", typeof(FilmsPageViewModel).GetTypeInfo().Assembly);
            PickMinDateCommand = new Command(async () =>
            {
                DateTime.TryParse(MinDate, out DateTime currentDate);
                var result = await UserDialogs.Instance.DatePromptAsync(new DatePromptConfig()
                {
                    SelectedDate = currentDate
                });
                if (result.Ok)
                {
                    MinDate = result.SelectedDate.ToString("dd.MM.yyyy");
                }
            });
            PickMaxDateCommand = new Command(async () =>
            {
                DateTime.TryParse(MaxDate, out DateTime currentDate);
                var result = await UserDialogs.Instance.DatePromptAsync(new DatePromptConfig()
                {
                    SelectedDate = currentDate
                });
                if (result.Ok)
                {
                    MaxDate = result.SelectedDate.ToString("dd.MM.yyyy");
                }
            });
            AddFilmCommand = new Command(() =>
            {
                ActionSheetConfig config = new ActionSheetConfig();
                var filmsExceptAlreadyAdded = _allFilms.Except(_films);
                foreach (var film in filmsExceptAlreadyAdded)
                {
                    config.Add(film.Title, () => _films.Add(film));
                }
                config.SetCancel();
                config.Title = manager.GetString("addFilm", CustomSettings.Settings.Instance.Culture);
                UserDialogs.Instance.ActionSheet(config);
            });
            RemoveFilmCommand = new Command(() =>
            {
                ActionSheetConfig config = new ActionSheetConfig();
                foreach (var film in _films)
                {
                    config.Add(film.Title, () => _films.Remove(film));
                }
                config.SetCancel();
                config.Title = manager.GetString("removeFilm", CustomSettings.Settings.Instance.Culture);
                UserDialogs.Instance.ActionSheet(config);
            });
            ApplyCommand = new Command(async () =>
            {
                DateTime minDate = DateTime.Parse(MinDate);
                DateTime maxDate = DateTime.Parse(MaxDate);
                if (minDate > maxDate)
                {
                    UserDialogs.Instance.Toast("Check your date preferences");
                }
                else
                {
                    vm.Actors = new CustomObservableCollection<Actor>(ActorCollection.Instance.Actors.Where(a =>a.Name.ToLower().Contains(Name.ToLower())).
                    Where(a => DateTime.Parse(a.DateOfBirth) <= maxDate && DateTime.Parse(a.DateOfBirth) >= minDate).
                    Where(a => a.Films.Intersect(_films).Count() == _films.Count));
                    await App.NavigationService.GoBackAsync();
                }
            });
        }

        public string MinDate
        {
            get => _minDate;
            set
            {
                _minDate = value;
                OnPropertyChanged("MinDate");
            }
        }

        public string MaxDate
        {
            get => _maxDate;
            set
            {
                _maxDate = value;
                OnPropertyChanged("MaxDate");
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
    }
}
