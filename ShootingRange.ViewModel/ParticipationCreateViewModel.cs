using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Documents;
using System.Windows.Input;
using DotNetToolbox.RelayCommand;
using ShootingRange.BusinessObjects;
using ShootingRange.Common;
using ShootingRange.Common.Modules;
using ShootingRange.ConfigurationProvider;
using ShootingRange.Repository.Repositories;
using ShootingRange.Repository.RepositoryInterfaces;
using ShootingRange.Service.Interface;
using ShootingRange.UiBusinessObjects;
using ShootingRange.UiBusinessObjects.Annotations;

namespace ShootingRange.ViewModel
{
  public class ParticipationCreateViewModel : INotifyPropertyChanged
  {
    private IParticipationDataStore _participationDataStore;
    private IShooterCollectionDataStore _shooterCollectionDataStore;
    private IShooterCollectionParticipationDataStore _shooterCollectionParticipationDataStore;
    private IWindowService _windowService;

    public ParticipationCreateViewModel()
    {
      if (!DesignTimeHelper.IsInDesignMode)
      {
        IConfiguration config = ConfigurationSource.Configuration;
        _participationDataStore = config.GetParticipationDataStore();
        _shooterCollectionDataStore = config.GetShooterCollectionDataStore();
        _shooterCollectionParticipationDataStore = config.GetShooterCollectionParticipationDataStore();
        _collectionShooterDataStore = config.GetCollectionShooterDataStore();
        _shooterDataStore = config.GetShooterDataStore();
        _personDataStore = config.GetPersonDataStore();
        _windowService = config.GetWindowService();
        _events = config.GetUIEvents();
        _events.FetchSelectedParticipation += () => _selectedUiParticipation;

        LoadParticipations();
        LoadAvailableShooters();
      }

      OkCommand = new RelayCommand<ParticipationDraft>(ExecuteCreateParticipationCommand, CanExecuteCreateParticipationCommand);
      CancelCommand = new RelayCommand<object>(ExecuteCancelCommand);

      AssignCommand = new RelayCommand<UiShooter>(ExecuteAssignCommand, CanExecuteAssignCommand);
      RemoveCommand = new RelayCommand<UiShooter>(ExecuteRemoveCommand, CanExecuteRemoveCommand);
      DeleteCommand = new RelayCommand<UiShooterCollection>(ExecuteDeleteShooterCollectionCommand,
        CanExecuteDeleteShooterCollectionCommand);
      CreateCommand = new RelayCommand<UiParticipation>(ExecuteCreateShooterCollectionParticipation,
        CanExecuteShooterCollectionParticipation);
    }

    private void ExecuteCreateShooterCollectionParticipation(UiParticipation obj)
    {
      _windowService.ShowTextBoxInputDialog(string.Format("'{0}' Gruppe erstellen", obj.ParticipationName), "Gruppenname eingeben");
      LoadData();
    }

    private bool CanExecuteShooterCollectionParticipation(UiParticipation obj)
    {
      return obj != null;
    }

    private void ExecuteDeleteShooterCollectionCommand(UiShooterCollection obj)
    {
      if (_windowService.ShowYesNoMessasge("Schützengruppe löschen",
        string.Format("Die Schützengruppe '{0}' wirklich löschen?", obj.CollectionName)))
      {
        _shooterCollectionDataStore.Delete(obj.ToShooterCollection());
        LoadData();
      }
    }

    private bool CanExecuteDeleteShooterCollectionCommand(UiShooterCollection obj)
    {
      return obj != null;
    }

    private void ExecuteRemoveCommand(UiShooter obj)
    {
      IEnumerable<CollectionShooter> collectionShooters =
        _collectionShooterDataStore.FindByShooterCollectionId(SelectedUiShooterCollection.ShooterCollectionId);

      CollectionShooter collectionShooter = collectionShooters.FirstOrDefault(_ => _.ShooterId == obj.ShooterId);
      _collectionShooterDataStore.Delete(collectionShooter);
      LoadData();
    }

    private void ExecuteAssignCommand(UiShooter obj)
    {
      CollectionShooter collectionShooter = new CollectionShooter
      {
        ShooterCollectionId = SelectedUiShooterCollection.ShooterCollectionId,
        ShooterId = obj.ShooterId
      };

      _collectionShooterDataStore.Create(collectionShooter);
      LoadData();
    }

    private bool CanExecuteAssignCommand(UiShooter obj)
    {
      return obj != null && SelectedUiShooterCollection != null;
    }

    private bool CanExecuteRemoveCommand(UiShooter obj)
    {
      return obj != null && SelectedUiShooterCollection != null;
    }

    private void LoadData()
    {
      int selectedParticipationId = 0;
      if (SelectedUiParticipation != null)
        selectedParticipationId = SelectedUiParticipation.ParticipationId;

      int selectedShooterCollectionId = 0;
      if (SelectedUiShooterCollection != null)
        selectedShooterCollectionId = SelectedUiShooterCollection.ShooterCollectionId;

      LoadParticipations();
      LoadAvailableShooters();

      SelectedUiParticipation = UiParticipations.FirstOrDefault(_ => _.ParticipationId == selectedParticipationId);

      if (SelectedUiParticipation != null)
        SelectedUiShooterCollection =
          SelectedUiParticipation.ShooterCollections.FirstOrDefault(
            _ => _.ShooterCollectionId == selectedShooterCollectionId);
    }

    private void LoadAvailableShooters()
    {
      IEnumerable<int> usedShooterIds = from s in _shooterDataStore.GetAll()
        join cs in _collectionShooterDataStore.GetAll() on s.ShooterId equals cs.ShooterId
        join scp in _shooterCollectionParticipationDataStore.GetAll() on cs.ShooterCollectionId equals
          scp.ShooterCollectionId
        where scp.ParticipationId == (SelectedUiParticipation == null ? 0 : SelectedUiParticipation.ParticipationId)
        select s.ShooterId;

      AvailableUiShooters =
        new ObservableCollection<UiShooter>(
          _shooterDataStore.GetAll().Where(_ => usedShooterIds.All(s => s != _.ShooterId))
            .Select(UiBusinessObjectMapper.ToUiShooter).Select(_ => _.FetchPerson(_personDataStore)));

      //if (SelectedUiShooterCollection != null)
      //{
      //  List<UiShooter> reducuedList =
      //    AvailableUiShooters.Except(SelectedUiShooterCollection.Shooters, new UiShooterComparer()).ToList();
      //  AvailableUiShooters = new ObservableCollection<UiShooter>(reducuedList);
      //}
    }

    private void LoadParticipations()
    {
      UiParticipations =
        new ObservableCollection<UiParticipation>(
          _participationDataStore.GetAll()
            .Where(_ => _.AllowCollectionParticipation)
            .Select(UiBusinessObjectMapper.ToUiParticipation)
            .Select(
              _ =>
                _.FetchShooters(_shooterCollectionParticipationDataStore,
                  _shooterCollectionDataStore,
                  _collectionShooterDataStore,
                  _shooterDataStore,
                  _personDataStore)));
    }

    #region Commands

    public ICommand AssignCommand { get; set; }
    public ICommand RemoveCommand { get; set; }

    public ICommand OkCommand { get; private set; }
    public ICommand CancelCommand { get; private set; }

    public ICommand DeleteCommand { get; private set; }
    public ICommand CreateCommand { get; private set; }

    #endregion

    private void ExecuteCancelCommand(object obj)
    {
      _windowService.CloseCreateParticipationWindow();
    }

    private void ExecuteCreateParticipationCommand(ParticipationDraft participationDraft)
    {
      ShooterCollection shooterCollection = new ShooterCollection
      {
        CollectionName = participationDraft.ParticipationName
      };
      _shooterCollectionDataStore.Create(shooterCollection);

      ShooterCollectionParticipation collectionParticipation = new ShooterCollectionParticipation
      {
        ParticipationId = participationDraft.ParticipationType.ParticipationTypeId,
        ShooterCollectionId = shooterCollection.ShooterCollectionId
      };

      _shooterCollectionParticipationDataStore.Create(collectionParticipation);
      _windowService.CloseCreateParticipationWindow();
    }

    private bool CanExecuteCreateParticipationCommand(ParticipationDraft participationDraft)
    {
      return participationDraft != null && !string.IsNullOrWhiteSpace(participationDraft.ParticipationName) &&
             participationDraft.ParticipationType != null;
    }

    #region Properties

    private ObservableCollection<UiShooter> _availableUiShooters;

    public ObservableCollection<UiShooter> AvailableUiShooters
    {
      get { return _availableUiShooters; }
      set
      {
        if (value != _availableUiShooters)
        {
          _availableUiShooters = value;
          OnPropertyChanged("AvailableUiShooters");
        }
      }
    }

    private ObservableCollection<UiParticipation> _uiParticipations;
    public ObservableCollection<UiParticipation> UiParticipations
    {
      get { return _uiParticipations; }
      set
      {
        if (value != _uiParticipations)
        {
          _uiParticipations = value;
          OnPropertyChanged("UiParticipations");
        }
      }
    }

    
    private UiShooter _selectedAvailableUiShooter;
    public UiShooter SelectedAvailableUiShooter
    {
      get { return _selectedAvailableUiShooter; }
      set
      {
        if (value != _selectedAvailableUiShooter)
        {
          _selectedAvailableUiShooter = value;
          OnPropertyChanged("SelectedAvailableUiShooter");
        }
      }
    }

    
    private UiShooter _selectedAssignedUiShooter;
    public UiShooter SelectedAssignedeUiShooter
    {
      get { return _selectedAssignedUiShooter; }
      set
      {
        if (value != _selectedAssignedUiShooter)
        {
          _selectedAssignedUiShooter = value;
          OnPropertyChanged("SelectedAssignedeUiShooter");
        }
      }
    }

    private UiShooterCollection _selectedUiShooterCollection;
    private ICollectionShooterDataStore _collectionShooterDataStore;
    private IShooterDataStore _shooterDataStore;
    private IPersonDataStore _personDataStore;

    public UiShooterCollection SelectedUiShooterCollection
    {
      get { return _selectedUiShooterCollection; }
      set
      {
        if (value != _selectedUiShooterCollection)
        {
          _selectedUiShooterCollection = value;
          OnPropertyChanged("SelectedUiShooterCollection");
          LoadAvailableShooters();
        }
      }
    }


    private UiParticipation _selectedUiParticipation;
    private UIEvents _events;

    public UiParticipation SelectedUiParticipation
    {
      get { return _selectedUiParticipation; }
      set
      {
        if (value != _selectedUiParticipation)
        {
          _selectedUiParticipation = value;
          OnPropertyChanged("SelectedUiParticipation");
          LoadAvailableShooters();
        }
      }
    }

    #endregion

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
