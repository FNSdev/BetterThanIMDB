using Acr.UserDialogs;
using BetterThanIMDB.Models;
using BetterThanIMDB.Models.Collections;
using BetterThanIMDB.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace BetterThanIMDB.ViewModels
{
    class SearchFilmsPageViewModel : BaseViewModel
    {
        private CustomObservableCollection<Actor> _allActors = ActorCollection.Instance.Actors;
        private CustomObservableCollection<Producer> _allProducers = ProducerCollection.Instance.Producers;
        private CustomObservableCollection<Actor> _actors = new CustomObservableCollection<Actor>();
        private CustomObservableCollection<Producer> _producers = new CustomObservableCollection<Producer>();
        private List<Genres> _genres = new List<Genres>();

        private string _title = "Title";
        private string _minDate = "01.01.1900";
        private string _maxDate = "01.01.2020";

        private int _minDuration = 0;
        private int _maxDuration = 300;

        public ICommand AddActorCommand { get; private set; }
        public ICommand AddProducerCommand { get; private set; }
        public ICommand AddGenreCommand { get; private set; }
        public ICommand RemoveActorCommand { get; private set; }
        public ICommand RemoveProducerCommand { get; private set; }
        public ICommand RemoveGenreCommand { get; private set; }
        public ICommand ApplyCommand { get; private set; }
        public ICommand PickMinDateCommand { get; private set; }
        public ICommand PickMaxDateCommand { get; private set; }
        
        public SearchFilmsPageViewModel(FilmsPageViewModel vm)
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
            AddActorCommand = new Command(() =>
            {
                ActionSheetConfig config = new ActionSheetConfig();
                var actorsExceptAlreadyAdded = _allActors.Except(_actors);
                foreach (var actor in actorsExceptAlreadyAdded)
                {
                    config.Add(actor.Name, () => _actors.Add(actor));
                }
                config.SetCancel();
                config.Title = manager.GetString("addActor", CustomSettings.Settings.Instance.Culture);
                UserDialogs.Instance.ActionSheet(config);
            });
            AddProducerCommand = new Command(() =>
            {
                ActionSheetConfig config = new ActionSheetConfig();
                var producersExceptAlreadyAdded = _allProducers.Except(_producers);
                foreach (var producer in producersExceptAlreadyAdded)
                {
                    config.Add(producer.Name, () => _producers.Add(producer));
                }
                config.SetCancel();
                config.Title = manager.GetString("addProducer", CustomSettings.Settings.Instance.Culture);
                UserDialogs.Instance.ActionSheet(config);
            });
            AddGenreCommand = new Command(() =>
            {
                ActionSheetConfig config = new ActionSheetConfig();
                List<Genres> allGenres = new List<Genres>();
                foreach (var g in Enum.GetValues(typeof(Genres)))
                {
                    allGenres.Add((Genres)g);
                }
                foreach(var genre in allGenres.Except(_genres))
                {
                    config.Add(genre.ToString(), () => _genres.Add(genre));
                }
                config.SetCancel();
                config.Title = manager.GetString("addGenre", CustomSettings.Settings.Instance.Culture);
                UserDialogs.Instance.ActionSheet(config);
            });
            RemoveActorCommand = new Command(() =>
            {
                ActionSheetConfig config = new ActionSheetConfig();
                foreach (var actor in _actors)
                {
                    config.Add(actor.Name, () => _actors.Remove(actor));
                }
                config.SetCancel();
                config.Title = manager.GetString("removeActor", CustomSettings.Settings.Instance.Culture);
                UserDialogs.Instance.ActionSheet(config);
            });
            RemoveProducerCommand = new Command(() =>
            {
                ActionSheetConfig config = new ActionSheetConfig();
                foreach(var producer in _producers)
                {
                    config.Add(producer.Name, () => _producers.Remove(producer));
                }
                config.SetCancel();
                config.Title = manager.GetString("removeProducer", CustomSettings.Settings.Instance.Culture);
                UserDialogs.Instance.ActionSheet(config);
            });
            RemoveGenreCommand = new Command(() =>
            {
                ActionSheetConfig config = new ActionSheetConfig();
                foreach(var genre in _genres)
                {
                    config.Add(genre.ToString(), () => _genres.Remove(genre));
                }
                config.SetCancel();
                config.Title = manager.GetString("removeGenre", CustomSettings.Settings.Instance.Culture);
                UserDialogs.Instance.ActionSheet(config);
            });

            ApplyCommand = new Command(async () =>
            {
                DateTime minDate = DateTime.Parse(MinDate);
                DateTime maxDate = DateTime.Parse(MaxDate);
                if(MinDuration > MaxDuration || MaxDuration == 0)
                {
                    UserDialogs.Instance.Toast("Check your duration preferences");                    
                }
                else if (minDate > maxDate)
                {
                    UserDialogs.Instance.Toast("Check your date preferences");
                }
                else
                {
                    vm.Films = new CustomObservableCollection<Film>(FilmCollection.Instance.Films.
                    Where(f => f.Title.ToLower().Contains(Title.ToLower())).
                    Where(f => f.Duration <= MaxDuration && f.Duration >= MinDuration).
                    Where(f => DateTime.Parse(f.ReleaseDate) <= maxDate && DateTime.Parse(f.ReleaseDate) >= minDate).
                    Where(f => f.Actors.Intersect(_actors).Count() == _actors.Count).
                    Where(f => f.Producers.Intersect(_producers).Count() == _producers.Count).
                    Where(f => f.GenresList.Intersect(_genres).Count() == _genres.Count));
                    await App.NavigationService.GoBackAsync();
                }
            });
        }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
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

        public int MinDuration
        {
            get => _minDuration;
            set
            {
                _minDuration = value;
                OnPropertyChanged("MinDuration");
            }
        }

        public int MaxDuration
        {
            get => _maxDuration;
            set
            {
                _maxDuration = value;
                OnPropertyChanged("MaxDuration");
            }
        }
    }
}
