using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ShootingRange.BusinessObjects;
using ShootingRange.Engine;
using ShootingRange.Repository;
using ShootingRange.ViewModel.Annotations;

namespace ShootingRange.ViewModel
{
    public class PersonListViewModel : INotifyPropertyChanged
    {
      public PersonListViewModel()
      {
        IConfigurationFactory configFactory = ConfigurationFactoryProvider.GetConfigurationFactory();
        _repository = configFactory.GetPersonRepository();

        People = new ObservableCollection<Person>(_repository.GetAll());
      }

      private ObservableCollection<Person> _people;
      private IPersonDataStore _repository;

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
