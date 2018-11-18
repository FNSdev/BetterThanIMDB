using Acr.UserDialogs;
using BetterThanIMDB.Models;
using BetterThanIMDB.Models.Collections;
using BetterThanIMDB.ViewModels.Base;
using Plugin.Media;
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
    class EditProducerPageViewModel : BaseViewModel
    {
        private Producer _producer;
        private string _name;
        private string _dateOfBirth;
        private string _photo;
        private CustomObservableCollection<Film> _tempFilms = new CustomObservableCollection<Film>();
        private CustomObservableCollection<Film> _allFilms = FilmCollection.Instance.Films;

        public ICommand ApplyCommand { get; private set; }
        public ICommand SelectDateCommand { get; private set; }
        public ICommand SelectImageCommand { get; private set; }
        public ICommand AddFilmCommand { get; private set; }
        public ICommand RemoveFilmCommand { get; private set; }

        public EditProducerPageViewModel(Producer producer)
        {
            ResourceManager manager = new ResourceManager("BetterThanIMDB.Resources.locale", typeof(FilmsPageViewModel).GetTypeInfo().Assembly);

            _producer = producer;
            Name = producer.Name;
            DateOfBirth = producer.DateOfBirth;
            Photo = producer.Photo;

            foreach (var film in producer.Films)
            {
                _tempFilms.Add(film);
            }

            ApplyCommand = new Command(async () =>
            {
                var col1 = _producer.Films.Except(_tempFilms).ToList();
                var col2 = _tempFilms.Except(_producer.Films).ToList();

                for (int i = 0; i < col1.Count(); i++)
                {
                    col1.ElementAt(i).RemoveProducer(_producer);
                }
                for (int i = 0; i < col2.Count(); i++)
                {
                    col2.ElementAt(i).AddProducer(_producer);
                }

                _producer.Name = Name;
                _producer.DateOfBirth = DateOfBirth;
                _producer.Photo = Photo;

                await App.NavigationService.GoBackAsync();
            });
            SelectDateCommand = new Command(async () =>
            {
                DateTime.TryParse(DateOfBirth, out DateTime currentDate);
                var result = await UserDialogs.Instance.DatePromptAsync(new DatePromptConfig()
                {
                    SelectedDate = currentDate
                });
                if (result.Ok)
                {
                    DateOfBirth = result.SelectedDate.ToString("dd.MM.yyyy");
                }
            });
            SelectImageCommand = new Command(async () =>
            {
                await CrossMedia.Current.Initialize();
                var selectedImageFile = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions()
                {
                    CompressionQuality = 92,
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                });

                if (selectedImageFile != null)
                {
                    Photo = selectedImageFile.Path;
                }
            });
            AddFilmCommand = new Command(() =>
            {
                ActionSheetConfig config = new ActionSheetConfig();
                foreach (var film in _allFilms.Except(_tempFilms))
                {
                    config.Add(film.Title, () => _tempFilms.Add(film));
                }
                config.SetCancel();
                config.Title = manager.GetString("addFilm", CustomSettings.Settings.Instance.Culture);
                UserDialogs.Instance.ActionSheet(config);
            });
            RemoveFilmCommand = new Command(() =>
            {
                ActionSheetConfig config = new ActionSheetConfig();
                foreach (var film in _tempFilms)
                {
                    config.Add(film.Title, () => _tempFilms.Remove(film));
                }
                config.SetCancel();
                config.Title = manager.GetString("removeFilm", CustomSettings.Settings.Instance.Culture);
                UserDialogs.Instance.ActionSheet(config);
            });

        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public string DateOfBirth
        {
            get { return _dateOfBirth; }
            set
            {
                _dateOfBirth = value;
                OnPropertyChanged("DateOfBirth");
            }
        }

        public string Photo
        {
            get { return _photo; }
            set
            {
                _photo = value;
                OnPropertyChanged("Photo");
            }
        }
    }
}
