using System;
using System.Collections.Generic;
using System.Text;

namespace BetterThanIMDB.Models.Collections
{
    public class FilmCollection
    {
        private static FilmCollection _instance;

        public static FilmCollection Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new FilmCollection();
                return _instance;
            }
        }

        public Film Find(Guid id)
        {
            foreach(var f in Films)
            {
                if (f.ID == id)
                    return f;
            }
            throw new KeyNotFoundException();
        }

        public void DeleteFilm(Film film)
        {
            if (Films.Contains(film))
            {
                Films.Remove(film);
            }

            foreach(var actor in film.Actors)
            {
                actor.Films.Remove(film);
            }

            foreach(var producer in film.Producers)
            {
                producer.Films.Remove(film);
            }

            DataHelper.FilmToActorConnections.Remove(film.ID);
            DataHelper.FilmToProducerConnections.Remove(film.ID);
        }

        public CustomObservableCollection<Film> Films { get; set; } = new CustomObservableCollection<Film>();
        /*{
            new Film("Title1", 111, "01.01.2000",
                "It is a place, where some info about this film will be available, when you will actually place it here",
                "no_poster.jpeg"),
            new Film("Title2", 122, "01.01.2000",
                "It is a place, where some info about this film will be available, when you will actually place it here",
                "no_poster.jpeg"),
            new Film("Title3", 133, "01.01.2000",
                "It is a place, where some info about this film will be available, when you will actually place it here",
                "no_poster.jpeg"),
            new Film("Title4", 144, "01.01.2000",
                "It is a place, where some info about this film will be available, when you will actually place it here",
                "no_poster.jpeg"),
            new Film("Title5", 155, "01.01.2000",
                "It is a place, where some info about this film will be available, when you will actually place it here",
                "no_poster.jpeg"),
        };*/
    }
}
