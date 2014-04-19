using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using DotNetToolbox.RelayCommand;
using ShootingRange.BusinessObjects;
using ShootingRange.BusinessObjects.Properties;
using ShootingRange.Common;
using ShootingRange.Common.Modules;
using ShootingRange.ConfigurationProvider;
using ShootingRange.Repository;
using ShootingRange.Service.Interface;

namespace ShootingRange.ViewModel
{
  public class MainViewModel : INotifyPropertyChanged
  {
    public MainViewModel()
    {
      if (DesignTimeHelper.IsInDesignMode)
      {
        _repository = new FakePersonDataStore();
      }
      else
      {
        IConfiguration config = ConfigurationSource.Configuration;
        _repository = config.GetPersonRepository();
        _windowService = config.GetWindowService();
        _events = config.GetEvents();
        _uiEvents = config.GetUIEvents();
      }

      People = new ObservableCollection<Person>(_repository.GetAll());
      PersonListItems =
        new ObservableCollection<PersonListItem>(
          People.Select(_ => new PersonListItem
          {
            Id = _.PersonId,
            FirstName = _.FirstName,
            LastName = _.LastName
          }));

      PersonSelectionChangedCommand = new RelayCommand<int>(ExecutePersonSelectionChangedCommand);
      OpenPersonEditCommand = new RelayCommand<PersonListItem>(ExecuteOpenPersonEditCommand, CanExecuteOpenPersonEditCommand);
      CreatePersonCommand = new RelayCommand<object>(ExecuteCreatePersonCommand);
    }

    private void ExecuteCreatePersonCommand(object obj)
    {
      _windowService.ShowPersonEditWindow();
    }

    private bool CanExecuteOpenPersonEditCommand(PersonListItem person)
    {
      return person != null;
    }

    private void ExecuteOpenPersonEditCommand(PersonListItem person)
    {
      _windowService.ShowPersonEditWindow();
      Person entity = _repository.FindById(person.Id);
      _uiEvents.PersonSelected(entity);
    }

    private void ExecutePersonSelectionChangedCommand(int selectedPersonId)
    {
      _events.SelectedPersonChanged(selectedPersonId);
    }

    private IPersonDataStore _repository;
    private ShootingRangeEvents _events;

    public ICommand PersonSelectionChangedCommand { get; private set; }
    public ICommand OpenPersonEditCommand { get; private set; }
    public ICommand CreatePersonCommand { get; private set; }

    private PersonListItem _selectedPersonItem;
    public PersonListItem SelectedPersonItem
    {
      get { return _selectedPersonItem; }
      set
      {
        if (value != _selectedPersonItem)
        {
          _selectedPersonItem = value;
          OnPropertyChanged("SelectedPersonItem");
        }
      }
    }

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

    private ObservableCollection<PersonListItem> _personListItems;
    public ObservableCollection<PersonListItem> PersonListItems
    {
      get { return _personListItems; }
      set
      {
        if (value != _personListItems)
        {
          _personListItems = value;
          OnPropertyChanged("PersonListItems");
        }
      }
    }

    private ObservableCollection<Person> _people;
    private IWindowService _windowService;
    private UIEvents _uiEvents;

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
