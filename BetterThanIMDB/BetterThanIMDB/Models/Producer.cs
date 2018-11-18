using BetterThanIMDB.Models.Collections;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace BetterThanIMDB.Models
{
    public class Producer : INotifyPropertyChanged
    {
        #region fields
        private string _name;
        private string _photo;
        private string _dateOfBirth;
        private bool _isVisible;
        private CustomObservableCollection<Film> _films = new CustomObservableCollection<Film>();
        #endregion

        #region constructors
        public Producer(string name, string dateOfBirth)
        {
            ID = Guid.NewGuid();
            Name = name;
            DateOfBirth = dateOfBirth;
            //Films.CollectionChanged += FilmsListChanged;
        }
        public Producer(string name, string dateOfBirth, string photo) : this(name, dateOfBirth)
        {
            Photo = photo;
        }

        public Producer()
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

        public string Photo
        {
            get { return _photo; }
            set
            {
                _photo = value;
                OnPropertyChanged("Photo");
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

        public string DateOfBirth
        {
            get { return _dateOfBirth; }
            set
            {
                _dateOfBirth = value;
                OnPropertyChanged("DateOfBirth");
            }
        }
        #endregion

        #region methods
        public void OnPropertyChanged(string arg)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(arg));
        }

        public void AddFilm(Film film)
        {
            Films.Add(film);
            film.Producers.Add(this);
            DataHelper.FilmToProducerConnections[film.ID].Add(ID);
        }

        public void RemoveFilm(Film film)
        {
            Films.Remove(film);
            film.Producers.Remove(this);
            DataHelper.FilmToProducerConnections[film.ID].Remove(ID);
        }

        /*private void FilmsListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    Film film = Films[Films.Count - 1];
                    film.Producers.AddWithoutInvoking(this);
                    DataHelper.FilmToProducerConnections[film.ID].Add(ID);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    Film f = (Film)e.OldItems[e.OldStartingIndex];
                    f.Producers.RemoveWithoutInvoking(this);
                    DataHelper.FilmToProducerConnections[f.ID].Remove(ID);
                    break;
            }
        }*/
        #endregion
    }
}
