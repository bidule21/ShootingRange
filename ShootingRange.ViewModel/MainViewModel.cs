using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using DotNetToolbox.RelayCommand;
using ShootingRange.BusinessObjects;
using ShootingRange.BusinessObjects.Properties;
using ShootingRange.Common;
using ShootingRange.Common.BusinessObjects;
using ShootingRange.Common.Modules;
using ShootingRange.ConfigurationProvider;
using ShootingRange.Repository;
using ShootingRange.Repository.FakeRepositories;
using ShootingRange.Repository.Mapper;
using ShootingRange.Repository.Repositories;
using ShootingRange.Service.Interface;

namespace ShootingRange.ViewModel
{
  public class MainViewModel : INotifyPropertyChanged
  {
    private IPersonDataStore _personDataStore;
    private IShooterDataStore _shooterDataStore;
    private IGroupDataStore _groupDataStore;
    private IGroupMemberDetailsView _groupMemberDetailsView;
    private IGroupDetailsView _groupDetailsView;
    private ShootingRangeEvents _events;

    private IWindowService _windowService;
    private IShooterNumberService _shooterNumberService;
    private UIEvents _uiEvents;

    public MainViewModel()
    {
      if (DesignTimeHelper.IsInDesignMode)
      {
        _personDataStore = new FakePersonDataStore();
        _shooterDataStore = new FakeShooterDataStore();
        _groupDataStore = new FakeGroupDataStore();
        _groupMemberDetailsView = new FakeGroupMemberDetailsView();
        _groupDetailsView = new FakeGroupDetailsView();
      }
      else
      {
        IConfiguration config = ConfigurationSource.Configuration;
        _personDataStore = config.GetPersonDataStore();
        _shooterDataStore = config.GetShooterDataStore();
        _groupDataStore = config.GetGroupDataStore();
        _groupMemberDetailsView = config.GetGroupMemberDetailsView();
        _groupDetailsView = config.GetGroupDetailsView();

        _shooterNumberService = config.GetShooterNumberService();
        _windowService = config.GetWindowService();
        _events = config.GetEvents();
        _uiEvents = config.GetUIEvents();
      }

      IEnumerable<Person> people = new ObservableCollection<Person>(_personDataStore.GetAll());
      PersonListItems =
        new ObservableCollection<PersonListItem>(
          people.Select(_ => new PersonListItem
          {
            PersonId = _.PersonId,
            FirstName = _.FirstName,
            LastName = _.LastName
          }));

      PersonSelectionChangedCommand = new RelayCommand<int>(ExecutePersonSelectionChangedCommand);
      OpenPersonEditCommand = new RelayCommand<PersonListItem>(ExecuteOpenPersonEditCommand, CanExecuteOpenPersonEditCommand);
      CreatePersonCommand = new RelayCommand<object>(ExecuteCreatePersonCommand);
      CreateShooterCommand = new RelayCommand<PersonListItem>(ExecuteCreateShooterCommand, CanExecuteCreateShooterCommand);
    }

    private bool CanExecuteCreateShooterCommand(PersonListItem personListItem)
    {
      return personListItem != null;
    }

    private void ExecuteCreateShooterCommand(PersonListItem personListItem)
    {
      Shooter shooter = new Shooter();
      shooter.ShooterNumber = _shooterNumberService.GetShooterNumber();
      shooter.PersonId = personListItem.PersonId;
      _shooterDataStore.Create(shooter);
      OnSelectedPersonItemChanged(personListItem);
    }

    public ICommand PersonSelectionChangedCommand { get; private set; }
    public ICommand OpenPersonEditCommand { get; private set; }
    public ICommand CreatePersonCommand { get; private set; }
    public ICommand CreateShooterCommand { get; private set; }

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
      Person entity = _personDataStore.FindById(person.PersonId);
      _uiEvents.PersonSelected(entity);
    }

    private void ExecutePersonSelectionChangedCommand(int selectedPersonId)
    {
      _events.SelectedPersonChanged(selectedPersonId);
    }

    #region Properties


    private ShooterListItem _selectedShooterItem;
    public ShooterListItem SelectedShooterItem
    {
      get { return _selectedShooterItem; }
      set
      {
        if (value != _selectedShooterItem)
        {
          _selectedShooterItem = value;
          OnPropertyChanged("SelectedShooterItem");
          OnSelectedShooterItemChanged(_selectedShooterItem);
        }
      }
    }

    private ObservableCollection<ShooterListItem> _shooterListItems;
    public ObservableCollection<ShooterListItem> ShooterListItems
    {
      get { return _shooterListItems; }
      set
      {
        if (value != _shooterListItems)
        {
          _shooterListItems = value;
          OnPropertyChanged("ShooterListItems");
        }
      }
    }

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

          OnSelectedPersonItemChanged(SelectedPersonItem);
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

    private ObservableCollection<GroupTreeItem> _groupTreeItems;
    public ObservableCollection<GroupTreeItem> GroupTreeItems
    {
      get { return _groupTreeItems; }
      set
      {
        if (value != _groupTreeItems)
        {
          _groupTreeItems = value;
          OnPropertyChanged("GroupTreeItems");
        }
      }
    }

    #endregion

    #region Property changed handler

    private void OnSelectedShooterItemChanged(ShooterListItem selectedShooter)
    {
      LoadGroupList();
    }

    private void OnSelectedPersonItemChanged(PersonListItem selectedPerson)
    {
      LoadShooterList();
      LoadGroupList();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion

    #region Loader

    private void LoadGroupList()
    {
      Func<GroupDetails, GroupTreeItem> selector =
        (gmd) =>
          new GroupTreeItem
          {
            GroupNames = gmd.GroupNames,
            GroupDescription = gmd.GroupDescription
          };

      IEnumerable<GroupTreeItem> groupListItems;
      if (SelectedShooterItem != null)
      {
        groupListItems = _groupDetailsView.FindByShooterId(SelectedShooterItem.ShooterId).Select(selector);
      }
      else if (SelectedPersonItem != null)
      {
        groupListItems = _groupDetailsView.FindByPersonId(SelectedPersonItem.PersonId).Select(selector);
      }
      else
      {
        groupListItems = _groupDetailsView.GetAll().Select(selector);
      }

      GroupTreeItems = new ObservableCollection<GroupTreeItem>(groupListItems);
    }

    private void LoadShooterList()
    {
      Func<Shooter, ShooterListItem> selector = shooter => new ShooterListItem
      {
        ShooterNumber = shooter.ShooterNumber,
        ShooterId = shooter.ShooterId
      };

      IEnumerable<ShooterListItem> shooterListItems;
      if (SelectedPersonItem != null)
      {
        shooterListItems = _shooterDataStore.FindByPersonId(SelectedPersonItem.PersonId).Select(selector);
      }
      else
      {
        shooterListItems = _shooterDataStore.GetAll().Select(selector);
      }

      ShooterListItems = new ObservableCollection<ShooterListItem>(shooterListItems);
    }

    #endregion
  }
}
