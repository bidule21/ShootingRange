using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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
  public class ShooterCreateViewModel : INotifyPropertyChanged
  {
    private UIEvents _uiEvents;
    private IWindowService _windowService;
    private ObservableCollection<ParticipationListItem> _availableParticipations;
    private IShooterNumberService _shooterNumberService;
    private IShooterDataStore _shooterDataStore;
    private IShooterParticipationView _shooterParticipationView;
    private IParticipationDataStore _participationDataStore;
    private IShooterParticipationDataStore _shooterParticipationDataStore;
    private ISsvShooterDataWriterService _ssvShooterDataWriterService;

    public ShooterCreateViewModel()
    {
      AddToAssingedParticipationCommand = new RelayCommand<ParticipationListItem>(ExecuteAddToAssignedParticipationCommand,
        CanExecuteAddtoAssignedParticipationCommand);
      RemoveFromAssingedParticipationCommand =
        new RelayCommand<ShooterParticipationListItem>(ExecuteRemoveFromAssignedParticipationCommand,
          CanExecuteRemoveFromAssignedParticipationCommand);
      CancelCommand = new RelayCommand<object>(ExecuteCloseCommand);
      CreateShooterCommand = new RelayCommand<UiPerson>(ExecuteCreateShooterCommand, CanExecuteCreateShooterCommand);

      if (!DesignTimeHelper.IsInDesignMode)
      {
        IConfiguration config = ConfigurationSource.Configuration;
        _participationDataStore = config.GetParticipationDataStore();
        _windowService = config.GetWindowService();
        _shooterParticipationDataStore = config.GetShooterParticipationDataStore();
        _uiEvents = config.GetUIEvents();
        _uiEvents.PersonSelected += person => { UiPerson = person; };
        _uiEvents.RequireSelectedPerson();
        _shooterNumberService = config.GetShooterNumberService();
        _shooterDataStore = config.GetShooterDataStore();
        _shooterParticipationView = config.GetShooterParticipationView();
        _ssvShooterDataWriterService = config.GetSsvShooterDataWriterService();
        LoadAvailableParticipationList();
        LoadAssignedParticipationList();
      }
    }

    private bool CanExecuteRemoveFromAssignedParticipationCommand(ShooterParticipationListItem shooterParticipation)
    {
      return shooterParticipation != null;
    }

    private void ExecuteRemoveFromAssignedParticipationCommand(ShooterParticipationListItem shooterParticipation)
    {
      try
      {
        ShooterParticipation participation = _shooterParticipationDataStore.FindById(shooterParticipation.ShooterParticipationId);
        _shooterParticipationDataStore.Delete(participation);
      }
      catch (Exception e)
      {
        ReportException(e);
      }
      finally
      {
        LoadAssignedParticipationList();
        LoadAvailableParticipationList();
      }
    }

    private void ExecuteCloseCommand(object obj)
    {
      _windowService.CloseCreateShooterWindow();
    }

    private bool CanExecuteCreateShooterCommand(UiPerson uiPerson)
    {
      return uiPerson != null;
    }

    private void ExecuteCreateShooterCommand(UiPerson uiPerson)
    {
      try
      {
        Shooter shooter = new Shooter();
        shooter.ShooterNumber = _shooterNumberService.GetShooterNumber();
        shooter.PersonId = uiPerson.PersonId;
        _shooterDataStore.Create(shooter);
        _ssvShooterDataWriterService.WriteShooterData(new SsvShooterData
        {
          FirstName = uiPerson.FirstName,
          LastName = uiPerson.LastName,
          LicenseNumber = (uint)shooter.ShooterNumber
        });
        UiShooter = UiBusinessObjectMapper.ToUiShooter(_shooterDataStore.FindByShooterNumber(shooter.ShooterNumber));
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

    private bool CanExecuteAddtoAssignedParticipationCommand(ParticipationListItem participation)
    {
      return participation != null && UiShooter != null;
    }

    private void ExecuteAddToAssignedParticipationCommand(ParticipationListItem participation)
    {
      ShooterParticipation sp = new ShooterParticipation
      {
        ShooterId = UiShooter.ShooterId,
        ParticipationId = participation.ParticipationId
      };

      _shooterParticipationDataStore.Create(sp);
      LoadAvailableParticipationList();
      LoadAssignedParticipationList();
    }

    private void LoadAvailableParticipationList()
    {
      Func<Participation, ParticipationListItem> selector = participation => new ParticipationListItem()
      {
        ParticipationId = participation.ParticipationId,
        ParticipationName = participation.ParticipationName
      };
      AvailableParticipations = new ObservableCollection<ParticipationListItem>(_participationDataStore.GetAll().Select(selector));
    }

    private void LoadAssignedParticipationList()
    {
      if (UiShooter != null)
      {
        Func<ShooterParticipationDetails, ShooterParticipationListItem> selector = participation => new ShooterParticipationListItem()
        {
          ShooterParticipationId = participation.ShooterParticipationId,
          ParticipationName = participation.ParticipationName
        };
        AssignedParticipations = new ObservableCollection<ShooterParticipationListItem>(_shooterParticipationView.FindByShooterId(UiShooter.ShooterId).Select(selector));
      }
    }

    private void ReportException(Exception e)
    {
      _windowService.ShowErrorMessage("Error", e.ToString());
    }

    public ICommand AddToAssingedParticipationCommand { get; private set; }
    public ICommand RemoveFromAssingedParticipationCommand { get; private set; }
    public ICommand CancelCommand { get; private set; }
    public ICommand CreateShooterCommand { get; private set; }
    
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
    
    private UiShooter _uiShooter;
    public UiShooter UiShooter
    {
      get { return _uiShooter; }
      set
      {
        if (value != _uiShooter)
        {
          _uiShooter = value;
          OnPropertyChanged("UiShooter");
        }
      }
    }

    private ShooterParticipationListItem _selectedAssignedParticipation;
    public ShooterParticipationListItem SelectedAssignedParticipation
    {
      get { return _selectedAssignedParticipation; }
      set
      {
        if (value != _selectedAssignedParticipation)
        {
          _selectedAssignedParticipation = value;
          OnPropertyChanged("SelectedAssignedParticipation");
        }
      }
    }

    private ParticipationListItem _selectedAvailableParticipation;
    public ParticipationListItem SelectedAvailableParticipation
    {
      get { return _selectedAvailableParticipation; }
      set
      {
        if (value != _selectedAvailableParticipation)
        {
          _selectedAvailableParticipation = value;
          OnPropertyChanged("SelectedAvailableParticipation");
        }
      }
    }

    private ObservableCollection<ShooterParticipationListItem> _assignedParticipations;
    public ObservableCollection<ShooterParticipationListItem> AssignedParticipations
    {
      get { return _assignedParticipations; }
      set
      {
        if (value != _assignedParticipations)
        {
          _assignedParticipations = value;
          OnPropertyChanged("AssignedParticipations");
        }
      }
    }

    public ObservableCollection<ParticipationListItem> AvailableParticipations
    {
      get { return _availableParticipations; }
      set
      {
        if (value != _availableParticipations)
        {
          _availableParticipations = value;
          OnPropertyChanged("AvailableParticipations");
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
