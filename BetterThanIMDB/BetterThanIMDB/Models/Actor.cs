using BetterThanIMDB.Models.Collections;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace BetterThanIMDB.Models
{
    public class Actor : INotifyPropertyChanged
    {
        #region fields
        private string _name;
        private string _dateOfBirth;
        private string _photo;
        private bool _isVisible = false;
        private CustomObservableCollection<Film> _films = new CustomObservableCollection<Film>();
        #endregion

        #region constructors
        public Actor(string name, string dateOfBirth)
        {
            ID = Guid.NewGuid();
            Name = name;
            DateOfBirth = dateOfBirth;
            //Films.CollectionChanged += FilmsListChanged;
        }

        public Actor(string name, string dateOfBirth, string photo) : this(name, dateOfBirth)
        {
            Photo = photo;
        }

        public Actor()
        {
        }
        #endregion

        #region properties
        public event PropertyChangedEventHandler PropertyChanged;

        public Guid ID { get; set; }

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

        [JsonIgnore]
        public CustomObservableCollection<Film> Films
        {
            get { return _films; }
            private set
            {
                _films = value;
                OnPropertyChanged("Films");
            }
        }

        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                OnPropertyChanged("IsVisible");
            }
        }
        #endregion

        #region methods
        public void OnPropertyChanged(string arg)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(arg));
        }

        public void AddFilm(in Film film)
        {
            Films.Add(film);
            film.Actors.Add(this);
            DataHelper.FilmToActorConnections[film.ID].Add(ID);
        }

        public void RemoveFilm(in Film film)
        {
            Films.Remove(film);
            film.Actors.Remove(this);
            DataHelper.FilmToActorConnections[film.ID].Remove(ID);
        }
        /*private void FilmsListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    Film film = Films[Films.Count - 1];
                    film.Actors.AddWithoutInvoking(this);
                    DataHelper.FilmToActorConnections[film.ID].Add(ID);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    Film f = (Film)e.OldItems[e.OldStartingIndex];
                    f.Actors.RemoveWithoutInvoking(this);
                    DataHelper.FilmToActorConnections[f.ID].Remove(ID);
                    break;
            }
        }*/
        #endregion

    }
}
