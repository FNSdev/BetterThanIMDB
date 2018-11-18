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
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BetterThanIMDB.ViewModels
{
    public class FilmsPageViewModel : BaseViewModel
    {
        private CustomObservableCollection<Film> _films = FilmCollection.Instance.Films;
        private Film _selectedFilm = null;
        private Film _prevSelectedFilm = null;

        private bool _sortedByTitle = false;
        private bool _sortedByRuntime = false;
        private bool _sortedByDate = false;

        public ICommand ExpandItemCommand { get; private set; }
        public ICommand DeleteItemCommand { get; private set; }
        public ICommand TestCommand { get; private set; }
        public ICommand ShowActorsCommand { get; private set; }
        public ICommand ShowProducersCommand { get; private set; }
        public ICommand ShowGenresCommand { get; private set; }
        public ICommand ShowInfoAboutFilmCommand { get; private set; }
        public ICommand ShowInfoAboutActorCommand { get; private set; }
        public ICommand ShowInfoAboutProducerCommand { get; private set; }
        public ICommand AddFilmCommand { get; private set; }
        public ICommand SearchFilmCommand { get; private set; }
        public ICommand UnapplyCommand { get; private set; }
        public ICommand SortByTitleCommand { get; private set; }
        public ICommand SortByRuntimeCommand { get; private set; }
        public ICommand SortByDateCommand { get; private set; }

        //todo implement showinfoaboutactor/producer

        public FilmsPageViewModel()
        {
            ResourceManager manager = new ResourceManager("BetterThanIMDB.Resources.locale", typeof(FilmsPageViewModel).GetTypeInfo().Assembly);

            ExpandItemCommand = new Command(() =>
            {
                if(_prevSelectedFilm == SelectedFilm)
                {
                    SelectedFilm.IsVisible = !SelectedFilm.IsVisible;
                }
                else
                {
                    if (_prevSelectedFilm != null)
                    {
                        _prevSelectedFilm.IsVisible = false;
                    }
                    SelectedFilm.IsVisible = true;
                }

                _prevSelectedFilm = SelectedFilm;
            });

            DeleteItemCommand = new Command(() =>
            {
                UserDialogs.Instance.Confirm(new ConfirmConfig()
                {
                    Message = manager.GetString("confirm", CustomSettings.Settings.Instance.Culture),
                    OnAction = (confirmed) =>
                    {
                        if (confirmed)
                        {
                            FilmCollection.Instance.DeleteFilm(SelectedFilm);
                            SelectedFilm = null;
                        }
                    }                  
                });
            });

            ShowActorsCommand = new Command(() =>
            {
                ActionSheetConfig config = new ActionSheetConfig();
                foreach(var actor in SelectedFilm.Actors)
                {
                    config.Add(actor.Name, async() => await App.NavigationService.NavigateAsync("EditActorPage", actor));
                }
                config.SetCancel();
                config.Title = manager.GetString("actorsList", CustomSettings.Settings.Instance.Culture);
                UserDialogs.Instance.ActionSheet(config);
            });

            ShowProducersCommand = new Command(() =>
            {
                ActionSheetConfig config = new ActionSheetConfig();
                foreach (var producer in SelectedFilm.Producers)
                {
                    config.Add(producer.Name, async() => await App.NavigationService.NavigateAsync("EditProducerPage", producer));
                }
                config.SetCancel();
                config.Title = manager.GetString("producersList", CustomSettings.Settings.Instance.Culture);
                UserDialogs.Instance.ActionSheet(config);
            });

            ShowGenresCommand = new Command(() =>
            {
                ActionSheetConfig config = new ActionSheetConfig();
                foreach (var genre in SelectedFilm.GenresList)
                {
                    config.Add(genre.ToString());
                }
                config.SetCancel();
                config.Title = manager.GetString("genresList", CustomSettings.Settings.Instance.Culture);
                UserDialogs.Instance.ActionSheet(config);

            });

            ShowInfoAboutFilmCommand = new Command(async() =>
                 await App.NavigationService.NavigateAsync("EditFilmPage", SelectedFilm));

            AddFilmCommand = new Command(() =>
            {
                ActionSheetConfig config = new ActionSheetConfig();
                config.Add(manager.GetString("createFilm", CustomSettings.Settings.Instance.Culture), () => CreateNewMovie());
                config.Add(manager.GetString("getFilm", CustomSettings.Settings.Instance.Culture), async () => await App.NavigationService.NavigateAsync("TMDBFilmsPage"));
                config.SetCancel();
                UserDialogs.Instance.ActionSheet(config);
            });
            
            SearchFilmCommand = new Command(async() => await App.NavigationService.NavigateAsync("SearchFilmsPage", this));

            UnapplyCommand = new Command(() => Films = FilmCollection.Instance.Films);

            SortByTitleCommand = new Command(() =>
            {
                if(_sortedByTitle)
                {
                    Films = new CustomObservableCollection<Film>(Films.OrderByDescending(f => f.Title));
                }
                else
                {
                    Films = new CustomObservableCollection<Film>(Films.OrderBy(f => f.Title));
                }
                _sortedByTitle = !_sortedByTitle;
            });
            SortByDateCommand = new Command(() =>
            {
                if (_sortedByDate)
                {
                    Films = new CustomObservableCollection<Film>(Films.OrderByDescending(f => DateTime.Parse(f.ReleaseDate)));
                }
                else
                {
                    Films = new CustomObservableCollection<Film>(Films.OrderBy(f => DateTime.Parse(f.ReleaseDate)));
                }
                _sortedByDate = !_sortedByDate;
            });
            SortByRuntimeCommand = new Command(() =>
            {
                if (_sortedByRuntime)
                {
                    Films = new CustomObservableCollection<Film>(Films.OrderByDescending(f => f.Duration));
                }
                else
                {
                    Films = new CustomObservableCollection<Film>(Films.OrderBy(f => f.Duration));
                }
                _sortedByRuntime = !_sortedByRuntime;
            });
        }

        public CustomObservableCollection<Film> Films
        {
            get { return _films; }
            set
            {
                _films = value;
                OnPropertyChanged("Films");
            }
        }

        public Film SelectedFilm
        {
            get { return _selectedFilm; }
            set
            {
                _selectedFilm = value;
                OnPropertyChanged("SelectedFilm");
            }
        }

        private void CreateNewMovie()
        {
            Film film = new Film("New Film", 0, "01.01.2000", "No overview available", "no_poster.jpeg");
            Films.Add(film);
        }
    }
}
