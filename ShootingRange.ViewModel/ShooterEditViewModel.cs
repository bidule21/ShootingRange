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
using ShootingRange.ViewModel;

namespace ShootingRange.ViewModel
{
  public class ShooterEditViewModel : INotifyPropertyChanged
  {
    private UIEvents _uiEvents;
    private IWindowService _windowService;
    private ObservableCollection<ParticipationListItem> _availableParticipations;
    private IShooterNumberService _shooterNumberService;
    private IShooterDataStore _shooterDataStore;
    private IShooterParticipationView _shooterParticipationView;
    private IParticipationDataStore _participationDataStore;
    private IShooterParticipationDataStore _shooterParticipationDataStore;

    public ShooterEditViewModel()
    {
            AddToAssingedParticipationCommand = new RelayCommand<ParticipationListItem>(ExecuteAddToAssignedParticipationCommand,
        CanExecuteAddtoAssignedParticipationCommand);
      RemoveFromAssingedParticipationCommand =
        new RelayCommand<ShooterParticipationListItem>(ExecuteRemoveFromAssignedParticipationCommand,
          CanExecuteRemoveFromAssignedParticipationCommand);
      CancelCommand = new RelayCommand<object>(ExecuteCloseCommand);

      if (!DesignTimeHelper.IsInDesignMode)
      {
        IConfiguration config = ConfigurationSource.Configuration;
        _participationDataStore = config.GetParticipationDataStore();
        _windowService = config.GetWindowService();
        _shooterParticipationDataStore = config.GetShooterParticipationDataStore();
        _uiEvents = config.GetUIEvents();
        _uiEvents.ShooterSelected += shooter => { UiShooter = shooter; };
        _uiEvents.RequireSelectedShooter();
        _shooterNumberService = config.GetShooterNumberService();
        _shooterDataStore = config.GetShooterDataStore();
        _shooterParticipationView = config.GetShooterParticipationView();
        LoadAvailableParticipationList();
        LoadAssignedParticipationList();
      }
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


    private bool CanExecuteRemoveFromAssignedParticipationCommand(ShooterParticipationListItem shooterParticipation)
    {
      return shooterParticipation != null;
    }
    private void ReportException(Exception e)
    {
      _windowService.ShowErrorMessage("Error", e.ToString());
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


    public ICommand AddToAssingedParticipationCommand { get; private set; }
    public ICommand RemoveFromAssingedParticipationCommand { get; private set; }
    public ICommand CancelCommand { get; private set; }

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

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
