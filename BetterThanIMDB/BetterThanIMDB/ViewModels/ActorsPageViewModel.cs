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
    public class ActorsPageViewModel : BaseViewModel
    {
        private CustomObservableCollection<Actor> _actors = ActorCollection.Instance.Actors;
        private Actor _selectedActor = null;
        private Actor _prevSelectedActor = null;

        private bool _sortedByName = false;
        private bool _sortedByCount = false;
        private bool _sortedByDate = false;

        public ICommand ExpandItemCommand { get; private set; }
        public ICommand DeleteItemCommand { get; private set; }
        public ICommand ShowFilmsCommand { get; private set; }
        public ICommand ShowInfoAboutActorCommand { get; private set; }
        public ICommand AddActorCommand { get; private set; }
        public ICommand SearchActorCommand { get; private set; }
        public ICommand UnapplyCommand { get; private set; }
        public ICommand SortByNameCommand { get; private set; }
        public ICommand SortByCountCommand { get; private set; }
        public ICommand SortByDateCommand { get; private set; }

        public ActorsPageViewModel()
        {
            ResourceManager manager = new ResourceManager("BetterThanIMDB.Resources.locale", typeof(FilmsPageViewModel).GetTypeInfo().Assembly);
            ExpandItemCommand = new Command(() =>
            {
                if (_prevSelectedActor == SelectedActor)
                {
                    SelectedActor.IsVisible = !SelectedActor.IsVisible;
                }
                else
                {
                    if (_prevSelectedActor != null)
                    {
                        _prevSelectedActor.IsVisible = false;
                    }
                    SelectedActor.IsVisible = true;
                }

                _prevSelectedActor = SelectedActor;
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
                            ActorCollection.Instance.DeleteActor(SelectedActor);
                            SelectedActor = null;
                        }
                    }
                });
            });
            ShowFilmsCommand = new Command(() =>
            {
                ActionSheetConfig config = new ActionSheetConfig();
                foreach (var film in SelectedActor.Films)
                {
                    config.Add(film.Title, () => ShowInfoAboutFilm(film));
                }
                config.SetCancel();
                config.Title = manager.GetString("filmsList", CustomSettings.Settings.Instance.Culture);
                UserDialogs.Instance.ActionSheet(config);
            });
            ShowInfoAboutActorCommand = new Command(async () =>
                await App.NavigationService.NavigateAsync("EditActorPage", SelectedActor));

            AddActorCommand = new Command(() =>
            {
                ActionSheetConfig config = new ActionSheetConfig();
                config.Add(manager.GetString("createActor", CustomSettings.Settings.Instance.Culture), () => CreateNewActor());
                config.Add(manager.GetString("getActor", CustomSettings.Settings.Instance.Culture), async () => await App.NavigationService.NavigateAsync("TMDBPersonPage", "Actor"));
                config.SetCancel();
                UserDialogs.Instance.ActionSheet(config);
            });
            SearchActorCommand = new Command(async () => await App.NavigationService.NavigateAsync("SearchActorsPage", this));

            UnapplyCommand = new Command(() => Actors = ActorCollection.Instance.Actors);
            SortByNameCommand = new Command(() =>
            {
                if (_sortedByName)
                {
                    Actors = new CustomObservableCollection<Actor>(Actors.OrderByDescending(a => a.Name));
                }
                else
                {
                    Actors = new CustomObservableCollection<Actor>(Actors.OrderBy(a => a.Name));
                }
                _sortedByName = !_sortedByName;
            });
            SortByDateCommand = new Command(() =>
            {
                if (_sortedByDate)
                {
                    Actors = new CustomObservableCollection<Actor>(Actors.OrderByDescending(a => DateTime.Parse(a.DateOfBirth)));
                }
                else
                {
                    Actors = new CustomObservableCollection<Actor>(Actors.OrderBy(a => DateTime.Parse(a.DateOfBirth)));
                }
                _sortedByDate = !_sortedByDate;
            });
            SortByCountCommand = new Command(() =>
            {
                if (_sortedByCount)
                {
                    Actors = new CustomObservableCollection<Actor>(Actors.OrderByDescending(a => a.Films.Count));
                }
                else
                {
                    Actors = new CustomObservableCollection<Actor>(Actors.OrderBy(a => a.Films.Count));
                }
                _sortedByCount = !_sortedByCount;
            });
        }

        public CustomObservableCollection<Actor> Actors
        {
            get { return _actors; }
            set
            {
                _actors = value;
                OnPropertyChanged("Actors");
            }
        }

        public Actor SelectedActor
        {
            get { return _selectedActor; }
            set
            {
                _selectedActor = value;
                OnPropertyChanged("SelectedActor");
            }
        }

        private async void ShowInfoAboutFilm(Film film)
        {
            await App.NavigationService.NavigateAsync("EditFilmPage", film);
        }

        private void CreateNewActor()
        {
            Actor actor = new Actor("New Actor", "01.01.1901", "no_photo.png");
            ActorCollection.Instance.Actors.Add(actor);
        }
    }
}
