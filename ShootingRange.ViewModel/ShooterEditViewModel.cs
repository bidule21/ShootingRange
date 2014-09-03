using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using DotNetToolbox.RelayCommand;
using ShootingRange.BusinessObjects;
using ShootingRange.Common;
using ShootingRange.ConfigurationProvider;
using ShootingRange.Repository.Repositories;
using ShootingRange.Repository.RepositoryInterfaces;
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
    private IShooterCollectionDataStore _shooterCollectionDataStore;
    private IPersonDataStore _personDataStore;
    private ICollectionShooterDataStore _collectionShooterDataStore;
    private IShooterCollectionParticipationDataStore _shooterCollectionParticipationDataStore;

    public ShooterEditViewModel()
    {
            AddToAssingedParticipationCommand = new RelayCommand<ParticipationListItem>(ExecuteAddToAssignedParticipationCommand,
        CanExecuteAddtoAssignedParticipationCommand);
      RemoveFromAssingedParticipationCommand =
        new RelayCommand<ShooterParticipationListItem>(ExecuteRemoveFromAssignedParticipationCommand,
          CanExecuteRemoveFromAssignedParticipationCommand);
      CancelCommand = new RelayCommand<object>(ExecuteCloseCommand);
      AssignShooterCollectionCommand = new RelayCommand<UiShooterCollection>(ExecuteAssignShooterCommand,
        CanExecuteAssignShooterCommand);

      if (!DesignTimeHelper.IsInDesignMode)
      {
        IConfiguration config = ConfigurationSource.Configuration;
        _participationDataStore = config.GetParticipationDataStore();
        _windowService = config.GetWindowService();
        _shooterParticipationDataStore = config.GetShooterParticipationDataStore();
        _collectionShooterDataStore = config.GetCollectionShooterDataStore();
        _personDataStore = config.GetPersonDataStore();
        _uiEvents = config.GetUIEvents();
        _uiEvents.ShooterSelected += shooter => { UiShooter = shooter; };
        _uiEvents.RequireSelectedShooter();
        _shooterNumberService = config.GetShooterNumberService();
        _shooterDataStore = config.GetShooterDataStore();
        _shooterParticipationView = config.GetShooterParticipationView();
        _shooterCollectionDataStore = config.GetShooterCollectionDataStore();
        _shooterCollectionParticipationDataStore = config.GetShooterCollectionParticipationDataStore();
        LoadData();

        UiShooterCollections = new ObservableCollection<UiShooterCollection>(_shooterCollectionDataStore.GetAll().Select(UiBusinessObjectMapper.ToUiShooterCollection).OrderBy(_ => _.CollectionName));
      }
    }

    private bool CanExecuteAssignShooterCommand(UiShooterCollection obj)
    {
      return obj != null && UiShooter != null;
    }

    private void ExecuteAssignShooterCommand(UiShooterCollection obj)
    {
      CollectionShooter collectionShooter = new CollectionShooter
      {
        ShooterId = UiShooter.ShooterId,
        ShooterCollectionId = UiShooterCollection.ShooterCollectionId
      };

      _collectionShooterDataStore.Create(collectionShooter);
      _windowService.CloseEditShooterWindow();
    }

    public ICommand AssignShooterCollectionCommand { get; set; }

    
    private bool _isGroupAssignmentEnabled;
    public bool IsGroupAssignmentEnabled
    {
      get { return _isGroupAssignmentEnabled; }
      set
      {
        if (value != _isGroupAssignmentEnabled)
        {
          _isGroupAssignmentEnabled = value;
          OnPropertyChanged("IsGroupAssignmentEnabled");
        }
      }
    }

    private void LoadAvailableParticipationList()
    {
      Func<Participation, ParticipationListItem> selector = participation => new ParticipationListItem()
      {
        ParticipationId = participation.ParticipationId,
        ParticipationName = participation.ParticipationName,
        GroupListItems = new List<GroupListItem>() { }
      };

      AvailableParticipations =
        new ObservableCollection<ParticipationListItem>(
          _participationDataStore.GetAll()
            .Where(_ => AssignedParticipations.All(a => a.ParticipationName != _.ParticipationName))
            .Select(selector));
    }

    
    private ObservableCollection<UiShooterCollection> _uiShooterCollections;
    public ObservableCollection<UiShooterCollection> UiShooterCollections
    {
      get { return _uiShooterCollections; }
      set
      {
        if (value != _uiShooterCollections)
        {
          _uiShooterCollections = value;
          OnPropertyChanged("UiShooterCollections");
        }
      }
    }

    private UiShooterCollection _uiShooterCollection;
    public UiShooterCollection UiShooterCollection
    {
      get { return _uiShooterCollection; }
      set
      {
        if (value != _uiShooterCollection)
        {
          _uiShooterCollection = value;
          OnPropertyChanged("UiShooterCollection");
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
          if (_uiShooter != null && _personDataStore != null)
          {
            _uiShooter.FetchPerson(_personDataStore);
            UiShooterInfo = string.Format("{0} {1} ({2})", _uiShooter.FirstName, _uiShooter.LastName, _uiShooter.ShooterNumber);
          }
          OnPropertyChanged("UiShooter");
        }
      }
    }


    private string _uiShooterInfo;
    public string UiShooterInfo
    {
      get { return _uiShooterInfo; }
      set
      {
        if (value != _uiShooterInfo)
        {
          _uiShooterInfo = value;
          OnPropertyChanged("UiShooterInfo");
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

      try
      {
        _shooterParticipationDataStore.Create(sp);

      }
      catch (Exception e)
      {
        _shooterParticipationDataStore.Revert();
        ReportException(e);
      }
      finally
      {
        LoadData();
      }
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
        _shooterParticipationDataStore.Revert();
        ReportException(e);
      }
      finally
      {
        LoadData();
      }
    }

    private void LoadData()
    {
      LoadAssignedParticipationList();
      LoadAvailableParticipationList();
    }

    private void ExecuteCloseCommand(object obj)
    {
      _windowService.CloseEditShooterWindow();
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
          FetchShooterCollection();
          OnPropertyChanged("SelectedAssignedParticipation");
        }
      }
    }

    private void FetchShooterCollection()
    {
      if (_selectedAssignedParticipation != null)
      {
        //UiShooterCollection = _shooterCollectionDataStore.FindById(_selectedAssignedParticipation.ShooterParticipationId)
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
