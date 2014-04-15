using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using DotNetToolbox.RelayCommand;
using ShootingRange.BusinessObjects;
using ShootingRange.BusinessObjects.Annotations;
using ShootingRange.Common.Modules;
using ShootingRange.Engine;
using ShootingRange.Repository;

namespace ShootingRange.ViewModel
{
  public class MainViewModel : INotifyPropertyChanged
  {
    public MainViewModel()
    {
      IConfigurationFactory configFactory = ConfigurationFactoryProvider.GetConfigurationFactory();
      _repository = configFactory.GetPersonRepository();
      //_events = configFactory.GetEvents();

      People = new ObservableCollection<Person>(_repository.GetAll());
      PersonNames = new ObservableCollection<string>(People.Select(_ => _.FirstName));
      //PersonSelectionChangedCommand = new RelayCommand<int>(ExecutePersonSelectionChangedCommand);
    }

    private void ExecutePersonSelectionChangedCommand(int selectedPersonId)
    {
      _events.SelectedPersonChanged(selectedPersonId);
    }

    private IPersonDataStore _repository;
    private ShootingRangeEvents _events;

    public ICommand PersonSelectionChangedCommand { get; private set; }

    
    private Person _person;
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
    
    private ObservableCollection<string> _personNames;
    public ObservableCollection<string> PersonNames
    {
      get { return _personNames; }
      set
      {
        if (value != _personNames)
        {
          _personNames = value;
          OnPropertyChanged("PersonNames");
        }
      }
    }

    private ObservableCollection<Person> _people;
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
