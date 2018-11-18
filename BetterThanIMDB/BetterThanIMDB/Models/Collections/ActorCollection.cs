using System;
using System.Collections.Generic;
using System.Text;

namespace BetterThanIMDB.Models.Collections
{
    public class ActorCollection
    {
        private static ActorCollection _instance;
        public static ActorCollection Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ActorCollection();
                return _instance;
            }
        }

        public Actor Find(Guid id)
        {
            foreach (var a in Actors)
            {
                if (a.ID == id)
                    return a;
            }
            throw new KeyNotFoundException();
        }

        public void DeleteActor(Actor actor)
        {
            if (Actors.Contains(actor))
            {
                Actors.Remove(actor);
            }

            foreach (var film in actor.Films)
            {
                film.Actors.Remove(actor);
                DataHelper.FilmToActorConnections[film.ID].Remove(actor.ID);
            }

        }

        public CustomObservableCollection<Actor> Actors { get; set; } = new CustomObservableCollection<Actor>();
        /*{
            new Actor("Actor #1", "12.12.1977", "no_photo.png"), 
            new Actor("Actor #2", "12.01.1982", "no_photo.png"),
            new Actor("Actor #3", "23.02.1994", "no_photo.png"),
            new Actor("Actor #4", "10.09.1964", "no_photo.png"),
            new Actor("Actor #5", "14.05.1957", "no_photo.png"),
            new Actor("Actor #6", "27.11.1984", "no_photo.png"),
        };*/
    }
}
