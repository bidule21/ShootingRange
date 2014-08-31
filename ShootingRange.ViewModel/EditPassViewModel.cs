using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using DotNetToolbox.RelayCommand;
using ShootingRange.BusinessObjects;
using ShootingRange.Common;
using ShootingRange.ConfigurationProvider;
using ShootingRange.Repository.Repositories;
using ShootingRange.Service.Interface;
using ShootingRange.UiBusinessObjects;
using ShootingRange.UiBusinessObjects.Annotations;

namespace ShootingRange.ViewModel
{
  public class EditPassViewModel : INotifyPropertyChanged
  {
    private IShooterDataStore _shooterDatastore;
    private ISessionDataStore _sessionDatastore;
    private IProgramItemDataStore _programItemDatastore;
    private IPersonDataStore _personDatastore;
    private IWindowService _windowService;
    private UIEvents _events;

    public EditPassViewModel()
    {
      IConfiguration config = ConfigurationSource.Configuration;
      _shooterDatastore = config.GetShooterDataStore();
      _sessionDatastore = config.GetSessionDataStore();
      _programItemDatastore = config.GetProgramItemDataStore();
      _personDatastore = config.GetPersonDataStore();
      _windowService = config.GetWindowService();
      _events = config.GetUIEvents();

      List<UiShooter> shooters = _shooterDatastore.GetAll().Select(UiBusinessObjectMapper.ToUiShooter).ToList();
      shooters.ForEach(_ => { if (_.PersonId != null) _.FetchPerson(_personDatastore.FindById((int) _.PersonId)); });
      UiShooters = new ObservableCollection<UiShooter>(shooters.OrderBy(_ => _.LastName).ThenBy(_ => _.FirstName));

      SearchShooterCommand = new RelayCommand<string>(ExecuteSearchShooterCommand, CanExecuteSearchShooterCommand);
      DeleteCommand = new RelayCommand<UiSession>(ExecuteDeleteCommand, CanExecuteDeleteCommand);
      CancelCommand = new RelayCommand<object>(ExecuteCancelCommand);
      SaveCommand = new RelayCommand<UiShooter>(ExecuteSaveCommand, CanExecuteSaveCommand);
      UiShooter selectedUiShooter = _events.FetchSelectedShooter() ;
      if (selectedUiShooter != null)
      {
        ShooterNumber = string.Format("{0}", selectedUiShooter.ShooterNumber);
        ExecuteSearchShooterCommand(ShooterNumber);
      }
    }

    private void ExecuteSaveCommand(UiShooter obj)
    {
      _selectedUiSession.ShooterId = obj.ShooterId;
      _sessionDatastore.Update(UiBusinessObjectMapper.ToSession(SelectedUiSession));
      ExecuteSearchShooterCommand(ShooterNumber);
      _events.ShooterDataStoreChanged();
    }

    private bool CanExecuteSaveCommand(UiShooter uiShooter)
    {
      return uiShooter != null && _selectedUiSession != null;
    }

    private void ExecuteCancelCommand(object obj)
    {
      _windowService.CloseEditPassWindow();
    }

    private bool CanExecuteDeleteCommand(UiSession session)
    {
      return session != null;
    }

    private void ExecuteDeleteCommand(UiSession session)
    {
      _sessionDatastore.Delete(UiBusinessObjectMapper.ToSession(session));
      ExecuteSearchShooterCommand(ShooterNumber);
      _events.ShooterDataStoreChanged();
    }


    private bool CanExecuteSearchShooterCommand(string shooterNumber)
    {
      return !string.IsNullOrWhiteSpace(shooterNumber);
    }

    private void ExecuteSearchShooterCommand(string obj)
    {
      int shooterNumber;
      if (int.TryParse(obj, out shooterNumber))
      {
        Shooter shooter = _shooterDatastore.FindByShooterNumber(shooterNumber);
        if (shooter != null)
        {
          if (shooter.PersonId != null)
          {
            UiShooter uiShooter = UiBusinessObjectMapper.ToUiShooter(shooter).FetchPerson(_personDatastore.FindById((int)shooter.PersonId));
            CurrentShooterLabel = string.Format("{0} {1} ({2})",uiShooter.LastName, uiShooter.FirstName, uiShooter.ShooterNumber);
          }
          int shooterId = shooter.ShooterId;
          List<UiSession> session = _sessionDatastore.FindByShooterId(shooterId).Select(UiBusinessObjectMapper.ToUiSession).ToList();
          session.ForEach(_ => _.ProgramDescription = string.Format("{0}", _programItemDatastore.FindById(_.ProgramItemId).ProgramName));
          UiSessions = new ObservableCollection<UiSession>(session);
          SelectedUiSession = UiSessions.FirstOrDefault();
        }
      }
    }

    public ICommand SearchShooterCommand { get; set; }
    public ICommand DeleteCommand { get; set; }
    public ICommand SaveCommand { get; set; }
    public ICommand CancelCommand { get; set; }

    
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
        }
      }
    }

    private UiSession _selectedUiSession;
    public UiSession SelectedUiSession
    {
      get { return _selectedUiSession; }
      set
      {
        if (value != _selectedUiSession)
        {
          _selectedUiSession = value;
          OnPropertyChanged("SelectedUiSession");
        }
      }
    }

    
    private string _currentShooterLabel;
    public string CurrentShooterLabel
    {
      get { return _currentShooterLabel; }
      set
      {
        if (value != _currentShooterLabel)
        {
          _currentShooterLabel = value;
          OnPropertyChanged("CurrentShooterLabel");
        }
      }
    }
    
    private string _shooterNumber;
    public string ShooterNumber
    {
      get { return _shooterNumber; }
      set
      {
        if (value != _shooterNumber)
        {
          _shooterNumber = value;
          OnPropertyChanged("ShooterNumber");
        }
      }
    }
    
    private ObservableCollection<UiSession> _uiSession;
    public ObservableCollection<UiSession> UiSessions
    {
      get { return _uiSession; }
      set
      {
        if (value != _uiSession)
        {
          _uiSession = value;
          OnPropertyChanged("UiSessions");
        }
      }
    }

    private ObservableCollection<UiShooter> _uiShooters;

    public ObservableCollection<UiShooter> UiShooters
    {
      get { return _uiShooters; }
      set
      {
        if (value != _uiShooters)
        {
          _uiShooters = value;
          OnPropertyChanged("UiShooters");
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
