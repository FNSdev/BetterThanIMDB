using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BetterThanIMDB.ViewModels.Base
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string arg)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(arg));
        }
    }
}
