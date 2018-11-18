using Acr.UserDialogs;
using BetterThanIMDB.Models;
using BetterThanIMDB.Models.Collections;
using BetterThanIMDB.ViewModels.Base;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace BetterThanIMDB.ViewModels
{
    class EditFilmPageViewModel : BaseViewModel
    {
        private Film _film;
        private string  _releaseDate;
        private short _duration;
        private string _title;
        private string _poster;
        private string _description;

        private CustomObservableCollection<Actor> _tempActors = new CustomObservableCollection<Actor>();
        private CustomObservableCollection<Actor> _allActors = ActorCollection.Instance.Actors;
        private CustomObservableCollection<Producer> _tempProducers = new CustomObservableCollection<Producer>();
        private CustomObservableCollection<Producer> _allProducers = ProducerCollection.Instance.Producers;
        private List<Genres> _tempGenres = new List<Genres>();
        private List<Genres> _allGenres = new List<Genres>();

        public ICommand AddActorCommand { get; private set; }
        public ICommand RemoveActorCommand { get; private set; }
        public ICommand AddProducerCommand { get; private set; }
        public ICommand RemoveProducerCommand { get; private set; }
        public ICommand AddGenreCommand { get; private set; }
        public ICommand RemoveGenreCommand { get; private set; }
        public ICommand ApplyCommand { get; private set; }
        public ICommand SelectDateCommand { get; private set; }
        public ICommand SelectImageCommand { get; private set; }

        public EditFilmPageViewModel(Film film)
        {
            ResourceManager manager = new ResourceManager("BetterThanIMDB.Resources.locale", typeof(FilmsPageViewModel).GetTypeInfo().Assembly);
            _film = film;
            ReleaseDate = film.ReleaseDate;
            Duration = film.Duration;
            Title = film.Title;
            Poster = film.Poster;
            Description = film.Description;
            foreach(var a in _film.Actors)
            {
                _tempActors.Add(a);
            }
            foreach(var p in _film.Producers)
            {
                _tempProducers.Add(p);
            }
            foreach(var g in _film.GenresList)
            {
                _tempGenres.Add(g);
            }
            foreach(var g in Enum.GetValues(typeof(Genres)))
            {
                _allGenres.Add((Genres)g);
            }

            ApplyCommand = new Command(async () =>
            {
                var col1 = _film.Actors.Except(_tempActors).ToList();
                var col2 = _tempActors.Except(_film.Actors).ToList();

                for (int i = 0; i < col1.Count(); i++)
                {
                    col1.ElementAt(i).RemoveFilm(_film);
                }
                for (int i = 0; i < col2.Count(); i++)
                {
                    col2.ElementAt(i).AddFilm(_film);
                }

                var col3 = _film.Producers.Except(_tempProducers).ToList();
                var col4 = _tempProducers.Except(_film.Producers).ToList();

                for (int i = 0; i < col3.Count(); i++)
                {
                    col3.ElementAt(i).RemoveFilm(_film);
                }
                for (int i = 0; i < col4.Count(); i++)
                {
                    col4.ElementAt(i).AddFilm(_film);
                }

                _film.Title = Title;
                _film.ReleaseDate = ReleaseDate;
                _film.Duration = Duration;
                _film.GenresList = _tempGenres;
                _film.Poster = Poster;

                await App.NavigationService.GoBackAsync();
            });

            RemoveActorCommand = new Command(() =>
            {
                ActionSheetConfig config = new ActionSheetConfig();
                foreach (var actor in _tempActors)
                {
                    config.Add(actor.Name, () =>
                    {
                        _tempActors.Remove(actor);

                    });
                }
                config.SetCancel();
                config.Title = manager.GetString("removeActor", CustomSettings.Settings.Instance.Culture);
                UserDialogs.Instance.ActionSheet(config);
            });

            AddActorCommand = new Command(() =>
            {
                ActionSheetConfig config = new ActionSheetConfig();
                var actorsExceptAlreadyAdded = _allActors.Except(_tempActors);
                foreach (var actor in actorsExceptAlreadyAdded)
                {
                    config.Add(actor.Name, () => _tempActors.Add(actor));
                }
                config.SetCancel();
                config.Title = manager.GetString("addActor", CustomSettings.Settings.Instance.Culture);
                UserDialogs.Instance.ActionSheet(config);
            });

            AddProducerCommand = new Command(() =>
            {
                ActionSheetConfig config = new ActionSheetConfig();
                var producersExceptAlreadyAdded = _allProducers.Except(_tempProducers);
                foreach (var producer in producersExceptAlreadyAdded)
                {
                    config.Add(producer.Name, () => _tempProducers.Add(producer));
                }
                config.SetCancel();
                config.Title = manager.GetString("addProducer", CustomSettings.Settings.Instance.Culture);
                UserDialogs.Instance.ActionSheet(config);
            });

            RemoveProducerCommand = new Command(() =>
            {
                ActionSheetConfig config = new ActionSheetConfig();
                foreach (var producer in _tempProducers)
                {
                    config.Add(producer.Name, () => _tempProducers.Remove(producer));
                }
                config.SetCancel();
                config.Title = manager.GetString("removeProducer", CustomSettings.Settings.Instance.Culture);
                UserDialogs.Instance.ActionSheet(config);
            });

            AddGenreCommand = new Command(() =>
            {
                ActionSheetConfig config = new ActionSheetConfig();
                var genresExceptAlreadyAdded = _allGenres.Except(_tempGenres);
                foreach (var genre in genresExceptAlreadyAdded)
                {
                    config.Add(genre.ToString(), () => _tempGenres.Add(genre));
                }
                config.SetCancel();
                config.Title = manager.GetString("addGenre", CustomSettings.Settings.Instance.Culture);
                UserDialogs.Instance.ActionSheet(config);
            });

            RemoveGenreCommand = new Command(() =>
            {
                ActionSheetConfig config = new ActionSheetConfig();
                foreach (var genre in _tempGenres)
                {
                    config.Add(genre.ToString(), () => _tempGenres.Remove(genre));
                }
                config.SetCancel();
                config.Title = manager.GetString("removeGenre", CustomSettings.Settings.Instance.Culture);
                UserDialogs.Instance.ActionSheet(config);
            });

            SelectDateCommand = new Command(async () =>
            {
                DateTime.TryParse(ReleaseDate, out DateTime currentDate);
                var result = await UserDialogs.Instance.DatePromptAsync(new DatePromptConfig()
                {
                    SelectedDate = currentDate
                });
                if (result.Ok)
                {
                    ReleaseDate = result.SelectedDate.ToString("dd.MM.yyyy");
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

                /*string fileName = Title + "_poster.jpeg";
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), fileName);

                using (var memoryStream = new MemoryStream())
                {
                    selectedImageFile.GetStream().CopyTo(memoryStream);
                    selectedImageFile.Dispose();

                    byte[] arr = memoryStream.ToArray();

                    using (FileStream file = File.Create(path))
                    {
                        await file.WriteAsync(arr, 0, arr.Length);
                    }
                }*/
                if (selectedImageFile != null)
                    Poster = selectedImageFile.Path;
            });
        }

        public string ReleaseDate
        {
            get { return _releaseDate; }
            set
            {
                _releaseDate = value;
                OnPropertyChanged("ReleaseDate");
            }
        }

        public short Duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                OnPropertyChanged("Duration");
            }
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
        
        public string Poster
        {
            get { return _poster; }
            set
            {
                _poster = value;
                OnPropertyChanged("Poster");
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }
    }
}
