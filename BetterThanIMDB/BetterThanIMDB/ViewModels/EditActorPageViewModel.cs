using BetterThanIMDB.Models;
using BetterThanIMDB.Models.Collections;
using BetterThanIMDB.ViewModels.Base;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Acr.UserDialogs;
using Plugin.Media;
using System.Reflection;
using System.Resources;

namespace BetterThanIMDB.ViewModels
{
    class EditActorPageViewModel : BaseViewModel
    {
        private Actor _actor;
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

        public EditActorPageViewModel(Actor actor)
        {
            ResourceManager manager = new ResourceManager("BetterThanIMDB.Resources.locale", typeof(FilmsPageViewModel).GetTypeInfo().Assembly);
            _actor = actor;
            Name = actor.Name;
            DateOfBirth = actor.DateOfBirth;
            Photo = actor.Photo;

            foreach(var film in actor.Films)
            {
                _tempFilms.Add(film);
            }

            ApplyCommand = new Command(async () =>
            {
                var col1 = _actor.Films.Except(_tempFilms).ToList();
                var col2 = _tempFilms.Except(_actor.Films).ToList();

                for(int i = 0; i < col1.Count(); i++)
                {
                    col1.ElementAt(i).RemoveActor(_actor);
                }
                for (int i = 0; i < col2.Count(); i++)
                {
                    col2.ElementAt(i).AddActor(_actor);
                }

                _actor.Name = Name;
                _actor.DateOfBirth = DateOfBirth;
                _actor.Photo = Photo;

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

                if(selectedImageFile != null)
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
