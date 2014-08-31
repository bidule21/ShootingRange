using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using DotNetToolbox.RelayCommand;
using ShootingRange.BusinessObjects;
using ShootingRange.Common;
using ShootingRange.ConfigurationProvider;
using ShootingRange.Repository.Repositories;
using ShootingRange.Repository.RepositoryInterfaces;
using ShootingRange.Service.Interface;
using ShootingRange.UiBusinessObjects;

namespace ShootingRange.ViewModel
{
  public class PersonEditViewModel : INotifyPropertyChanged
  {
    private UIEvents _uiEvents;
    private IPersonDataStore _personDataStore;
    private IShooterDataStore _shooterDataStore;
    private IShooterNumberService _shooterNumberService;
    private IWindowService _windowService;

    public PersonEditViewModel()
    {
      EditPersonCommand = new RelayCommand<UiPerson>(ExecuteEditPersonCommand, CanExecuteEditPersonCommand);
      CancelCommand = new RelayCommand<object>(ExecuteCloseCommand);
      //CreateShooterCommand = new RelayCommand<UiPerson>(ExecuteCreateShooterCommand, CanExecuteCreateShooterCommand);

      if (!DesignTimeHelper.IsInDesignMode)
      {
        IConfiguration config = ConfigurationSource.Configuration;
        _personDataStore = config.GetPersonDataStore();
        _shooterDataStore = config.GetShooterDataStore();
        _uiEvents = config.GetUIEvents();
        _windowService = config.GetWindowService();
        _shooterNumberService = config.GetShooterNumberService();
        _uiEvents.PersonSelected += person => { UiPerson = person ?? new UiPerson(); };
        _uiEvents.RequireSelectedPerson();
      }
    }

    //private void ExecuteCreateShooterCommand(UiPerson uiPerson)
    //{
    //  if (CanExecuteCreateShooterCommand(uiPerson))
    //  {
    //    int shooterNumber = _shooterNumberService.GetShooterNumber();
    //    Shooter shooter = new Shooter
    //    {
    //      PersonId = uiPerson.PersonId,
    //      ShooterNumber = shooterNumber,
    //      // TODO create with proper group information.
    //    };

    //    _shooterDataStore.Create(shooter);
    //    _windowService.ShowMessage("Shooter created.", string.Format("Shooter successfully created with ID {0}", shooter.ShooterId));
    //  }
    //  else
    //  {
    //    _windowService.ShowErrorMessage("Error", "Unable to create shooter.");
    //  }

    //}

    //private bool CanExecuteCreateShooterCommand(UiPerson uiPerson)
    //{
    //  return uiPerson != null && uiPerson.PersonId != default(int);
    //}

    private void ExecuteCloseCommand(object obj)
    {
      _windowService.CloseEditPersonWindow();
    }

    private void ExecuteEditPersonCommand(UiPerson uiPerson)
    {
      _personDataStore.Update(uiPerson.ToPerson());
      _uiEvents.PersonDataStoreChanged();
      _windowService.CloseEditPersonWindow();
    }

    private bool CanExecuteEditPersonCommand(UiPerson uiPerson)
    {
      return (uiPerson != null && !string.IsNullOrWhiteSpace(uiPerson.FirstName) && !string.IsNullOrWhiteSpace(uiPerson.LastName));
    }

    public ICommand EditPersonCommand { get; private set; }
    //public ICommand CreateShooterCommand { get; private set; }
    public ICommand CancelCommand { get; private set; }

    #region Properties

    private UiPerson _uiPerson;

    public UiPerson UiPerson
    {
      get { return _uiPerson; }
      set
      {
        if (value != _uiPerson)
        {
          _uiPerson = value;
          OnPropertyChanged("UiPerson");
        }
      }
    }


    private ObservableCollection<ParticipationTreeItem> _shootersGroups;
    public ObservableCollection<ParticipationTreeItem> ShootersGroups
    {
      get { return _shootersGroups; }
      set
      {
        if (value != _shootersGroups)
        {
          _shootersGroups = value;
          OnPropertyChanged("ShootersGroups");
        }
      }
    }

    private ObservableCollection<ParticipationTreeItem> _availableGroups;

    public ObservableCollection<ParticipationTreeItem> AvailableGroups
    {
      get { return _availableGroups; }
      set
      {
        if (value != _availableGroups)
        {
          _availableGroups = value;
          OnPropertyChanged("AvailableAvailableGroups");
        }
      }
    }


    private ObservableCollection<UiShooter> _shooters;

    public ObservableCollection<UiShooter> Shooters
    {
      get { return _shooters; }
      set
      {
        if (value != _shooters)
        {
          _shooters = value;
          OnPropertyChanged("Shooters");
        }
      }
    }

    #endregion

    public event PropertyChangedEventHandler PropertyChanged;

    [Annotations.NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
