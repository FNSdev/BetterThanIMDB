using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using System.Net.Http;
using Acr.UserDialogs;
using BetterThanIMDB.Models;
using BetterThanIMDB.Models.Collections;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;
using BetterThanIMDB.CustomSettings;

namespace BetterThanIMDB.Services
{
    public class TMDBService
    {
        private const string key = "014a954d47c7083bf8a9c392ddcdb634";
        private static TMDBService _instance = null;
        private const int pages = 1;

        private TMDbClient Client { get; set; }
        public ObservableCollection<SearchMovie> Films { get; } = new ObservableCollection<SearchMovie>();
        public ObservableCollection<Person> Persons { get; } = new ObservableCollection<Person>();

        public static TMDBService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TMDBService();
                }
                return _instance;
            }
        }

        private TMDBService()
        {
            //Client.GetConfig();
        }

        public void Initialize()
        {
            try
            {
                Client = new TMDbClient(key);
                Client.GetConfig();
            }
            catch (HttpRequestException)
            {
                
            }
        }

        public async Task SearchForMovie(string title)
        {
            Films.Clear();
            try
            {
                SearchContainer<SearchMovie> results = await Client.SearchMovieAsync(title);
                foreach (SearchMovie movie in results.Results.OrderByDescending(m => m.VoteAverage))
                {
                    Films.Add(movie);
                }
            }
            catch (HttpRequestException)
            {
                await UserDialogs.Instance.AlertAsync("Network Error");
            }

        }

        public async Task SearchForPerson(string name)
        {
            Persons.Clear();
            try
            {
                SearchContainer<SearchPerson> results = await Client.SearchPersonAsync(name);
                foreach(var person in results.Results)
                {
                    Person personDetailed = await Client.GetPersonAsync(person.Id);
                    Persons.Add(personDetailed);
                }
            }
            catch (HttpRequestException)
            {
                await UserDialogs.Instance.AlertAsync("Network Error");
            }
        }

        public async Task AddMovie(int id)
        {
            try
            {
                var movie = await Client.GetMovieAsync(id, MovieMethods.Credits);
                string date;

                if(movie.ReleaseDate == null)
                {
                    date = "No info available";
                }
                else
                {
                    date = ((DateTime)movie.ReleaseDate).ToString("dd.MM.yyyy");
                }

                string image = "no_poster.jpg";

                if (Settings.Instance.DownloadImages)
                {
                    image = Client.GetImageUrl("w500", movie.PosterPath).ToString();
                }

                Film film = new Film(movie.Title, (short)movie.Runtime, date, movie.Overview, image);

                foreach(var actor in movie.Credits.Cast)
                {
                    string name = actor.Name;
                    foreach (var existingActor in ActorCollection.Instance.Actors.Where(a => a.Name == name))
                    {
                        film.AddActor(existingActor);
                    }
                }

                foreach(var producer in movie.Credits.Crew)
                {
                    string name = producer.Name;
                    foreach (var existingProducer in ProducerCollection.Instance.Producers.Where
                        (p => p.Name == name && producer.Job == "Director"))
                    {
                        film.AddProducer(existingProducer);
                    }
                }

                foreach(var genre in movie.Genres)
                {
                    foreach (var g in Enum.GetValues(typeof(Genres)))
                    {
                        if (((Genres)g).ToString() == genre.Name.Replace(" ", "")) 
                        {
                            film.GenresList.Add((Genres)g);
                        }
                    }

                }                

                FilmCollection.Instance.Films.Add(film);
                UserDialogs.Instance.Toast("Success!");
            }
            catch (HttpRequestException)
            {
                UserDialogs.Instance.Toast("Network Error");
            }
            catch (Exception)
            {
                UserDialogs.Instance.Toast("Ooops! Something went wrong!");
            }
        }

        public async Task AddPerson(int id, string option)
        {
            try
            {
                Person person;
                string image = "no_photo.png";
                //Uri imageUri = Client.GetImageUrl("w500", person.Images.Profiles[0].FilePath);
                //person = await Client.GetPersonAsync(id, PersonMethods.MovieCredits);

                if (Settings.Instance.DownloadImages)
                {
                    person = await Client.GetPersonAsync(id, PersonMethods.Images);
                    image = Client.GetImageUrl("w500", person.Images.Profiles[0].FilePath).ToString();
                }

                person = await Client.GetPersonAsync(id, PersonMethods.MovieCredits);

                string date;
                if (person.Birthday == null)
                {
                    date = "No info available";
                }
                else
                {
                    date = ((DateTime)person.Birthday).ToString("dd.MM.yyyy");
                }

                switch (option)
                {
                    case "Actor":
                        Actor actor = new Actor(person.Name, date, image);
                        foreach(var film in person.MovieCredits.Cast)
                        {
                            string title = film.Title;
                            foreach (var existingFilm in FilmCollection.Instance.Films.Where(f => f.Title == title))
                            {
                                actor.AddFilm(existingFilm);
                            }
                        }
                        ActorCollection.Instance.Actors.Add(actor);
                        break;
                    case "Producer":
                        Producer producer = new Producer(person.Name, date, image);
                        foreach (var film in person.MovieCredits.Crew)
                        {
                            string title = film.Title;
                            foreach (var existingFilm in FilmCollection.Instance.Films.Where
                                (f => f.Title == title && film.Job == "Director"))
                            {
                                producer.AddFilm(existingFilm);
                            }
                        }
                        ProducerCollection.Instance.Producers.Add(producer);
                        break;
                }
                UserDialogs.Instance.Toast("Success!");
            }
            catch (HttpRequestException)
            {
                UserDialogs.Instance.Toast("Network Error");
            }
            catch (Exception)
            {
                UserDialogs.Instance.Toast("Ooops! Something went wrong!");
            }
        }
    }
}
