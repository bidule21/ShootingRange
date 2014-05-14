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
using ShootingRange.UiBusinessObjects;

namespace ShootingRange.ViewModel
{
  public class MainViewModel : INotifyPropertyChanged
  {
    private IPersonDataStore _personDataStore;
    private IShooterDataStore _shooterDataStore;
    private IParticipationDataStore _participationDataStore;
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
        _participationDataStore = new FakeParticipationDataStore();
        _groupMemberDetailsView = new FakeGroupMemberDetailsView();
        _groupDetailsView = new FakeGroupDetailsView();
      }
      else
      {
        IConfiguration config = ConfigurationSource.Configuration;
        _personDataStore = config.GetPersonDataStore();
        _shooterDataStore = config.GetShooterDataStore();
        _participationDataStore = config.GetParticipationDataStore();
        _groupMemberDetailsView = config.GetGroupMemberDetailsView();
        _groupDetailsView = config.GetGroupDetailsView();

        _shooterNumberService = config.GetShooterNumberService();
        _windowService = config.GetWindowService();
        _events = config.GetEvents();
        _uiEvents = config.GetUIEvents();
        _uiEvents.RequireSelectedPerson += () => _uiEvents.PersonSelected(SelectedUiPerson);
        _uiEvents.RequireSelectedShooter += () => _uiEvents.ShooterSelected(SelectedUiShooter);
        _uiEvents.PersonDataStoreChanged += LoadPersonList;
        _uiEvents.ShooterDataStoreChanged += LoadShooterList;
      }

      //IEnumerable<Person> people = new ObservableCollection<Person>(_personDataStore.GetAll());
      LoadPersonList();
      LoadShooterList();
      LoadParticipationList();
      //UiPeople =
      //  new ObservableCollection<UiPerson>(
      //    people.Select(_ => new UiPerson
      //    {
      //      PersonId = _.PersonId,
      //      FirstName = _.FirstName,
      //      LastName = _.LastName
      //    }));

      PersonSelectionChangedCommand = new RelayCommand<int>(ExecutePersonSelectionChangedCommand);

      CreatePersonCommand = new RelayCommand<object>(ExecuteCreatePersonCommand);
      EditPersonCommand = new RelayCommand<UiPerson>(ExecuteEditPersonCommand, CanExecuteEditPersonCommand);
      DeletePersonCommand = new RelayCommand<UiPerson>(ExecuteDeletePersonCommand, CanExecuteDeletePersonCommand);
      
      CreateShooterCommand = new RelayCommand<UiPerson>(ExecuteCreateShooterCommand, CanExecuteCreateShooterCommand);
      EditShooterCommand = new RelayCommand<UiShooter>(ExecuteEditShooterCommand, CanExecuteEditShooterCommand);
      DeleteShooterCommand = new RelayCommand<UiShooter>(ExecuteDeleteShooterCommand, CanExecuteDeleteShooterCommand);

      CreateParticipationCommand = new RelayCommand<UiShooter>(ExecuteCreateParticipationCommand,
        CanExecuteCreateParticipationCommand);
      //EditParticipationCommand = new RelayCommand<UiParticipation>
      //DeleteParticipationCommand = new RelayCommand<UiParticipation>
    }

    #region Commands

    #region Person

    private void ExecutePersonSelectionChangedCommand(int selectedPersonId)
    {
      _events.SelectedPersonChanged(selectedPersonId);
    }

    private void ExecuteCreatePersonCommand(object obj)
    {
      _windowService.ShowCreatePersonWindow();
      LoadShooterList();
    }

    private bool CanExecuteEditPersonCommand(UiPerson uiPerson)
    {
      return uiPerson != null;
    }

    private void ExecuteEditPersonCommand(UiPerson uiPerson)
    {
      _windowService.ShowEditPersonWindow();
    }

    private bool CanExecuteDeletePersonCommand(UiPerson uiPerson)
    {
      return uiPerson != null;
    }

    private void ExecuteDeletePersonCommand(UiPerson uiPerson)
    {
      _personDataStore.Delete(uiPerson.ToPerson());
      _uiEvents.PersonDataStoreChanged();
    }

    #endregion

    #region Shooter

    private bool CanExecuteCreateShooterCommand(UiPerson uiPerson)
    {
      return uiPerson != null;
    }

    private void ExecuteCreateShooterCommand(UiPerson uiPerson)
    {
      _windowService.ShowCreateShooterWindow();
    }

    private bool CanExecuteEditShooterCommand(UiShooter uiShooter)
    {
      return uiShooter != null;
    }

    private void ExecuteEditShooterCommand(UiShooter uiShooter)
    {
      _windowService.ShowEditShooterWindow();
      _uiEvents.ShooterDataStoreChanged();
    }

    private bool CanExecuteDeleteShooterCommand(UiShooter uiShooter)
    {
      return uiShooter != null;
    }

    private void ExecuteDeleteShooterCommand(UiShooter uiShooter)
    {
      try
      {
        _shooterDataStore.Delete(uiShooter.ToShooter());
      }
      catch (Exception e)
      {
        ReportException(e);
        _shooterDataStore.Revert();
      }
      finally
      {
        _uiEvents.ShooterDataStoreChanged();
      }
    }

    private void ReportException(Exception e)
    {
      _windowService.ShowErrorMessage("Error", e.ToString());
    }

    #endregion

    #region Participation

    private bool CanExecuteCreateParticipationCommand(UiShooter uiShooter)
    {
      return uiShooter != null;
    }

    private void ExecuteCreateParticipationCommand(UiShooter uiShooter)
    {
      throw new NotImplementedException();
    }

    #endregion

    #endregion

    public ICommand PersonSelectionChangedCommand { get; private set; }
    public ICommand CreatePersonCommand { get; private set; }
    public ICommand EditPersonCommand { get; private set; }
    public ICommand DeletePersonCommand { get; private set; }

    public ICommand CreateShooterCommand { get; private set; }
    public ICommand EditShooterCommand { get; private set; }
    public ICommand DeleteShooterCommand { get; private set; }

    public ICommand CreateParticipationCommand { get; private set; }
    public ICommand EditParticipationCommand { get; private set; }
    public ICommand DeleteParticipationCommand { get; private set; }

    #region Properties


    private UiShooter _selectedUiShooter;
    public UiShooter SelectedUiShooter
    {
      get { return _selectedUiShooter; }
      set
      {
        if (value != _selectedUiShooter)
        {
          _selectedUiShooter = value;
          OnPropertyChanged("SelectedUiShooter");
          OnSelectedShooterItemChanged(_selectedUiShooter);
        }
      }
    }

    private ObservableCollection<UiShooter> _shooterListItems;
    public ObservableCollection<UiShooter> ShooterListItems
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

    private UiPerson _selectedUiPerson;
    public UiPerson SelectedUiPerson
    {
      get { return _selectedUiPerson; }
      set
      {
        if (value != _selectedUiPerson)
        {
          _selectedUiPerson = value;
          OnPropertyChanged("SelectedUiPerson");

          OnSelectedPersonItemChanged(SelectedUiPerson);
        }
      }
    }

    private ObservableCollection<UiPerson> _uiPeople;

    public ObservableCollection<UiPerson> UiPeople
    {
      get { return _uiPeople; }
      set
      {
        if (value != _uiPeople)
        {
          _uiPeople = value;
          OnPropertyChanged("UiPeople");
        }
      }
    }

    private ObservableCollection<ParticipationTreeItem> _participationTreeItems;
    public ObservableCollection<ParticipationTreeItem> ParticipationTreeItems
    {
      get { return _participationTreeItems; }
      set
      {
        if (value != _participationTreeItems)
        {
          _participationTreeItems = value;
          OnPropertyChanged("ParticipationTreeItems");
        }
      }
    }

    #endregion

    #region Property changed handler

    private void OnSelectedShooterItemChanged(UiShooter selectedUiShooter)
    {
      LoadParticipationList();
    }

    private void OnSelectedPersonItemChanged(UiPerson selectedUiPerson)
    {
      LoadShooterList();
      LoadParticipationList();
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

    private void LoadPersonList()
    {
      UiPeople = new ObservableCollection<UiPerson>(_personDataStore.GetAll().Select(UiBusinessObjectMapper.ToUiPerson).OrderBy(_ => _.LastName));
    }

    private void LoadParticipationList()
    {
      Func<ParticipationDetails, ParticipationTreeItem> selector =
        (gmd) =>
          new ParticipationTreeItem
          {
            ParticipationNames = gmd.ParticipationNames,
            ParticipationDescription = gmd.ParticipationDescription
          };

      IEnumerable<ParticipationTreeItem> participationTreeItems;
      if (SelectedUiShooter != null)
      {
        participationTreeItems = _groupDetailsView.FindByShooterId(SelectedUiShooter.ShooterId).Select(selector);
      }
      else if (SelectedUiPerson != null)
      {
        participationTreeItems = _groupDetailsView.FindByPersonId(SelectedUiPerson.PersonId).Select(selector);
      }
      else
      {
        participationTreeItems = _groupDetailsView.GetAll().Select(selector);
      }

      ParticipationTreeItems = new ObservableCollection<ParticipationTreeItem>(participationTreeItems.Where(_ => _.ParticipationNames.Any()));
    }

    private void LoadShooterList()
    {
      Func<Shooter, UiShooter> selector = shooter => new UiShooter
      {
        ShooterNumber = shooter.ShooterNumber,
        ShooterId = shooter.ShooterId
      };

      IEnumerable<UiShooter> shooterListItems;
      if (SelectedUiPerson != null)
      {
        shooterListItems = _shooterDataStore.FindByPersonId(SelectedUiPerson.PersonId).Select(selector);
      }
      else
      {
        shooterListItems = _shooterDataStore.GetAll().Select(selector);
      }

      ShooterListItems = new ObservableCollection<UiShooter>(shooterListItems);
    }

    #endregion
  }
}
