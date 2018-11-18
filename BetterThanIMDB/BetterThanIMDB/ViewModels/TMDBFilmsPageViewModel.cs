using Acr.UserDialogs;
using BetterThanIMDB.Services;
using BetterThanIMDB.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TMDbLib.Objects.Search;
using Xamarin.Forms;

namespace BetterThanIMDB.ViewModels
{
    class TMDBFilmsPageViewModel : BaseViewModel
    {
        public ObservableCollection<SearchMovie> _films;
        public ICommand SearchCommand { get; private set; }
        public ICommand SelectItemCommand { get; private set; }

        private string _title = "";
        private TMDBService _service;
        private SearchMovie _selectedFilm;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        public SearchMovie SelectedFilm
        {
            get { return _selectedFilm; }
            set
            {
                _selectedFilm = value;
                OnPropertyChanged("SelectedFilm");
            }
        }

        public ObservableCollection<SearchMovie> Films
        {
            get { return _films; }
            set
            {
                _films = value;
                OnPropertyChanged("Films");
            }
        }

        public TMDBFilmsPageViewModel()
        {
            SearchCommand = new Command(async () => await Search());
            SelectItemCommand = new Command(async () => await AddFilm());
        }

        private async Task Search()
        {
            _service = TMDBService.Instance;
            await _service.SearchForMovie(Title);
            Films = _service.Films;
        }

        private async Task AddFilm()
        {
            await _service.AddMovie(SelectedFilm.Id);
        }
    }
}
