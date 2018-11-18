using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace BetterThanIMDB.Models.Collections
{
    public class CustomObservableCollection<T> : ObservableCollection<T>
    {
        public CustomObservableCollection() : base()
        {
        }

        public CustomObservableCollection(IEnumerable<T> col) : base(col)
        {
        }

        public void AddWithoutInvoking(T item)
        {
            Items.Add(item);
        }

        public void RemoveWithoutInvoking(T item)
        {
            Items.Remove(item);
        }

        public void AddMany(CustomObservableCollection<T> col)
        {
            Clear();
            foreach(var el in col)
            {
                Add(el);
                //Console.WriteLine("ELEMENT!!!!");
            }           
        }

        public void Concat(CustomObservableCollection<T> collection)
        {
            foreach (var i in collection)
                Add(i);
        }
    }
}
