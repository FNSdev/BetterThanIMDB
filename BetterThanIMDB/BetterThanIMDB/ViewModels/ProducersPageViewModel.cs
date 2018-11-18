using Acr.UserDialogs;
using BetterThanIMDB.Models;
using BetterThanIMDB.Models.Collections;
using BetterThanIMDB.ViewModels.Base;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Resources;
using System.Reflection;

namespace BetterThanIMDB.ViewModels
{
    public class ProducersPageViewModel : BaseViewModel
    {
        private CustomObservableCollection<Producer> _producers = ProducerCollection.Instance.Producers;
        private Producer _selectedProducer;
        private Producer _prevSelectedProducer;
        private bool _sortedByName = false;
        private bool _sortedByCount = false;
        private bool _sortedByDate = false;

        public ICommand ExpandItemCommand { get; private set; }
        public ICommand DeleteItemCommand { get; private set; }
        public ICommand ShowFilmsCommand { get; private set; }
        public ICommand ShowInfoAboutProducerCommand { get; private set; }
        public ICommand AddProducerCommand { get; private set; }
        public ICommand SearchProducerCommand { get; private set; }
        public ICommand UnapplyCommand { get; private set; }
        public ICommand SortByNameCommand { get; private set; }
        public ICommand SortByCountCommand { get; private set; }
        public ICommand SortByDateCommand { get; private set; }

        public ProducersPageViewModel()
        {
            ResourceManager manager = new ResourceManager("BetterThanIMDB.Resources.locale", typeof(FilmsPageViewModel).GetTypeInfo().Assembly);
            ExpandItemCommand = new Command(() =>
            {
                if (_prevSelectedProducer == SelectedProducer)
                {
                    SelectedProducer.IsVisible = !SelectedProducer.IsVisible;
                }
                else
                {
                    if (_prevSelectedProducer != null)
                    {
                        _prevSelectedProducer.IsVisible = false;
                    }
                    SelectedProducer.IsVisible = true;
                }

                _prevSelectedProducer = SelectedProducer;
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
                            ProducerCollection.Instance.DeleteProducer(SelectedProducer);
                            SelectedProducer = null;
                        }
                    }
                });
            });
            ShowFilmsCommand = new Command(() =>
            {
                ActionSheetConfig config = new ActionSheetConfig();
                foreach (var film in SelectedProducer.Films)
                {
                    config.Add(film.Title, async() => await App.NavigationService.NavigateAsync("EditFilmPage", film));
                }
                config.SetCancel();
                config.Title = manager.GetString("filmsList", CustomSettings.Settings.Instance.Culture);
                UserDialogs.Instance.ActionSheet(config);
            });
            ShowInfoAboutProducerCommand = new Command(async () =>
                await App.NavigationService.NavigateAsync("EditProducerPage", SelectedProducer));

            AddProducerCommand = new Command(() =>
            {
                ActionSheetConfig config = new ActionSheetConfig();
                config.Add(manager.GetString("createProducer", CustomSettings.Settings.Instance.Culture), () => CreateNewProducer());
                config.Add(manager.GetString("getProducer", CustomSettings.Settings.Instance.Culture), async () => await App.NavigationService.NavigateAsync("TMDBPersonPage", "Producer"));
                config.SetCancel();
                UserDialogs.Instance.ActionSheet(config);
            });
            SearchProducerCommand = new Command(async () => await App.NavigationService.NavigateAsync("SearchProducersPage", this));

            UnapplyCommand = new Command(() => Producers = ProducerCollection.Instance.Producers);
            SortByNameCommand = new Command(() =>
            {
                if (_sortedByName)
                {
                    Producers = new CustomObservableCollection<Producer>(Producers.OrderByDescending(p => p.Name));
                }
                else
                {
                    Producers = new CustomObservableCollection<Producer>(Producers.OrderBy(p => p.Name));
                }
                _sortedByName = !_sortedByName;
            });
            SortByDateCommand = new Command(() =>
            {
                if (_sortedByDate)
                {
                    Producers = new CustomObservableCollection<Producer>(Producers.OrderByDescending(p => DateTime.Parse(p.DateOfBirth)));
                }
                else
                {
                    Producers = new CustomObservableCollection<Producer>(Producers.OrderBy(p => DateTime.Parse(p.DateOfBirth)));
                }
                _sortedByDate = !_sortedByDate;
            });
            SortByCountCommand = new Command(() =>
            {
                if (_sortedByCount)
                {
                    Producers = new CustomObservableCollection<Producer>(Producers.OrderByDescending(p => p.Films.Count));
                }
                else
                {
                    Producers = new CustomObservableCollection<Producer>(Producers.OrderBy(p => p.Films.Count));
                }
                _sortedByCount = !_sortedByCount;
            });
        }

        public CustomObservableCollection<Producer> Producers
        {
            get { return _producers; }
            set
            {
                _producers = value;
                OnPropertyChanged("Producers");
            }
        }

        public Producer SelectedProducer
        {
            get { return _selectedProducer; }
            set
            {
                _selectedProducer = value;
                OnPropertyChanged("SelectedProducer");
            }
        }

        private void CreateNewProducer()
        {
            Producer producer = new Producer("New Producer","01.01.1900", "no_photo.png");
            ProducerCollection.Instance.Producers.Add(producer);
        }
    }
}
