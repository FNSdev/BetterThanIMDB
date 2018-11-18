using System;
using System.Collections.Generic;
using System.Text;

namespace BetterThanIMDB.Models.Collections
{
    public class ProducerCollection
    {
        private static ProducerCollection _instance;
        public static ProducerCollection Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ProducerCollection();
                return _instance;
            }
        }

        public Producer Find(Guid id)
        {
            foreach (var p in Producers)
            {
                if (p.ID == id)
                    return p;
            }
            throw new KeyNotFoundException();
        }

        public void DeleteProducer(Producer producer)
        {
            if (Producers.Contains(producer))
            {
                Producers.Remove(producer);
            }

            foreach(var film in producer.Films)
            {
                film.Producers.Remove(producer);
                DataHelper.FilmToProducerConnections[film.ID].Remove(producer.ID);
            }
        }

        public CustomObservableCollection<Producer> Producers { get; set; } = new CustomObservableCollection<Producer>();
        /*{
            new Producer("Producer #1", "01.01.1900", "no_photo.png"),
            new Producer("Producer #2", "01.01.1900", "no_photo.png"),
            new Producer("Producer #3", "01.01.1900", "no_photo.png"),
            new Producer("Producer #4", "01.01.1900", "no_photo.png"),
            new Producer("Producer #5", "01.01.1900", "no_photo.png"),
        };*/
    }
}
