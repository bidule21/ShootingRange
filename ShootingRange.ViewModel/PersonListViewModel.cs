using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ShootingRange.BusinessObjects;
using ShootingRange.Common.Modules;
using ShootingRange.Engine;
using ShootingRange.Repository;
using ShootingRange.ViewModel.Annotations;

namespace ShootingRange.ViewModel
{
    public class PersonListViewModel : INotifyPropertyChanged
    {
      public PersonListViewModel()
      {
        //IConfigurationFactory configFactory = ConfigurationFactoryProvider.GetConfigurationFactory();
        //_repository = configFactory.GetPersonRepository();
        //_events = configFactory.GetEvents();

        People = new ObservableCollection<Person>();
        //People = new ObservableCollection<Person>(_repository.GetAll());
        //PersonSelectionChangedCommand = new RelayCommand<int>(ExecutePersonSelectionChangedCommand);
      }

      //private void ExecutePersonSelectionChangedCommand(int selectedPersonId)
      //{
      //  _events.SelectedPersonChanged(selectedPersonId);
      //}

      private ObservableCollection<Person> _people;
      private IPersonDataStore _repository;
      private ShootingRangeEvents _events;

      //public ICommand PersonSelectionChangedCommand { get; private set; }

      public ObservableCollection<Person> People
      {
        get { return _people; }
        set
        {
          if (value != _people)
          {
            _people = value;
            OnPropertyChanged("People");
          }
        }
      }

      public event PropertyChangedEventHandler PropertyChanged;

      [NotifyPropertyChangedInvocator]
      protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
      {
        PropertyChangedEventHandler handler = PropertyChanged;
        if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
      }
    }
}
