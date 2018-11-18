using BetterThanIMDB.Models.Collections;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace BetterThanIMDB.Models
{
    public enum Genres : byte
    {
        Action, Adventure, Animation, Comedy, Crime, Documentary, Drama, Family, Fantasy, History, Horror, Music,
        Mystery, Romance, ScienceFiction, TVMovie, Thriller, War, Western
    }

    public class Film : INotifyPropertyChanged
    {
        #region fields
        private string _title;
        private CustomObservableCollection<Producer> _producers = new CustomObservableCollection<Producer>();
        private short _duration;
        private string _poster;
        private string _description;
        private List<Genres> _genres = new List<Genres>();
        private string _releaseDate;
        private bool _isVisible;
        private Guid _id;
        private CustomObservableCollection<Actor> _actors = new CustomObservableCollection<Actor>();
        #endregion

        #region constructors
        public Film(string title, short duration, string releaseDate, string description) 
        {
            ID = Guid.NewGuid();
            Title = title;
            Duration = duration;
            ReleaseDate = releaseDate;
            Description = description;
            //Actors.CollectionChanged += ActorsListChanged;
        }
        public Film(string title, short duration, string releaseDate, string description, string poster) :
               this(title, duration, releaseDate, description)
        {
            Poster = poster;
        }

        public Film()
        {
        }
        #endregion

        #region properties
        public Guid ID
        {
            get { return _id; }
            set
            {
                _id = value;
                DataHelper.FilmToActorConnections.Add(ID, new List<Guid>());
                DataHelper.FilmToProducerConnections.Add(ID, new List<Guid>());
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
        [JsonIgnore]
        public CustomObservableCollection<Producer> Producers
        {
            get { return _producers; }
            set
            {
                _producers = value;
                OnPropertyChanged("Producers");
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

        public List<Genres> GenresList
        {
            get { return _genres; }
            set
            {
                _genres = value;
                OnPropertyChanged("Genres");
            }
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

        [JsonIgnore ]
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                OnPropertyChanged("IsVisible");
            }
        }

        [JsonIgnore]
        public CustomObservableCollection<Actor> Actors
        {
            get { return _actors; }
            set
            {
                _actors = value;
                OnPropertyChanged("Actors");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region methods
        public void OnPropertyChanged(string arg)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(arg));
        }

        public void AddActor(in Actor actor)
        {
            Actors.Add(actor);
            actor.Films.Add(this);
            DataHelper.FilmToActorConnections[ID].Add(actor.ID);
        }

        public void RemoveActor(in Actor actor)
        {
            Actors.Remove(actor);
            actor.Films.Remove(this);
            DataHelper.FilmToActorConnections[ID].Remove(actor.ID);
        }

        public void AddProducer(in Producer producer)
        {
            Producers.Add(producer);
            producer.Films.Add(this);
            DataHelper.FilmToProducerConnections[ID].Add(producer.ID);
        }

        public void RemoveProducer(in Producer producer)
        {
            Producers.Remove(producer);
            producer.Films.Remove(this);
            DataHelper.FilmToProducerConnections[ID].Remove(producer.ID);
        }

        /*private void ActorsListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    Actor actor = Actors[Actors.Count - 1];
                    actor.Films.AddWithoutInvoking(this);
                    DataHelper.FilmToActorConnections[ID].Add(actor.ID);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    Actor a = (Actor)e.OldItems[e.OldStartingIndex];
                    a.Films.RemoveWithoutInvoking(this);
                    DataHelper.FilmToActorConnections[ID].Remove(a.ID);
                    break;
            }
        }
        private void ProducersListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    Producer producer = Producers[Producers.Count - 1];
                    producer.Films.AddWithoutInvoking(this);
                    DataHelper.FilmToProducerConnections[ID].Add(producer.ID);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    Producer p = (Producer)e.OldItems[e.OldStartingIndex];
                    p.Films.RemoveWithoutInvoking(this);
                    DataHelper.FilmToActorConnections[ID].Remove(p.ID);
                    break;
            }
        }*/
        #endregion
    }
}
