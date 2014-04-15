using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ShootAdmin.ViewModel;
using ShootingRange.BusinessObjects;
using ShootingRange.BusinessObjects.Annotations;
using ShootingRange.Engine;
using ShootingRange.Repository;

namespace ShootingRange.View
{
  class PersonEditViewModel : INotifyPropertyChanged
  {
    public ICommand UpdatePersonCommand { get; private set; }

    public PersonEditViewModel()
    {
      IConfigurationFactory configFactory = ConfigurationFactoryProvider.GetConfigurationFactory();
      _repository = configFactory.GetPersonRepository();
    }

    private void InitializeCommands()
    {
      UpdatePersonCommand = new RelayCommand<Person>(ExecuteUpdatePersonCommand, CanExecuteUpdatePersonCommand);
    }

    private bool CanExecuteUpdatePersonCommand(Person person)
    {
      return person != null;
    }

    private void ExecuteUpdatePersonCommand(Person person)
    {
      _repository.Update(person);
    }

    private Person _person;
    private IPersonDataStore _repository;

    public Person Person
    {
      get { return _person; }
      set
      {
        if (value != _person)
        {
          _person = value;
          OnPropertyChanged("Person");
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
