using BetterThanIMDB.Services;
using BetterThanIMDB.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Search;
using Xamarin.Forms;

namespace BetterThanIMDB.ViewModels
{
    class TMDBPersonPageViewModel : BaseViewModel
    {
        private string _option;
        private string _name = "";
        private ObservableCollection<Person> _persons;
        private Person _selectedPerson;
        public ICommand SearchCommand { get; private set; }
        public ICommand SelectItemCommand { get; private set; }

        public TMDBPersonPageViewModel(string option)
        {
            _option = option;
            SearchCommand = new Command(async() => await Search());
            SelectItemCommand = new Command(async () => await AddPerson());
        }

        public ObservableCollection<Person> Persons
        {
            get { return _persons; }
            set
            {
                _persons = value;
                OnPropertyChanged("Persons");
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public Person SelectedPerson
        {
            get { return _selectedPerson; }
            set
            {
                _selectedPerson = value;
                OnPropertyChanged("SelectedPerson");
            }
        }

        private async Task Search()
        {
            TMDBService service = TMDBService.Instance;
            await service.SearchForPerson(Name);
            Persons = service.Persons;
        }

        private async Task AddPerson()
        {
            await TMDBService.Instance.AddPerson(SelectedPerson.Id, _option);
        }
    }
}
