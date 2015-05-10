using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Gui.ViewModel;
using ShootingRange.BusinessObjects;
using ShootingRange.BusinessObjects.Properties;
using ShootingRange.ConfigurationProvider;
using ShootingRange.Repository.RepositoryInterfaces;
using ShootingRange.Service.Interface;

namespace ShootingRange.ServiceDesk.ViewModel
{
  public class ShooterViewModel : INotifyPropertyChanged
  {
    private IShooterParticipationDataStore _shooterParticipationDataStore;
    private IShooterDataStore _shooterDataStore;
    private IParticipationDataStore _participationDataStore;
    private ICollectionShooterDataStore _collectionShooterDataStore;
    private IShooterCollectionDataStore _shooterCollectionDataStore;
    private IShooterCollectionParticipationDataStore _shooterCollectionParticipationDataStore;

    public ShooterViewModel(Shooter shooter)
    {
      ShowSelectGroupingCommand = new ViewModelCommand(x => ShowSelectGrouping((IWindow)x));
      ShowSelectGroupingCommand.RaiseCanExecuteChanged();

      ShowSelectParticipationCommand = new ViewModelCommand(x => ShowSelectParticipation((IWindow)x));
      ShowSelectParticipationCommand.RaiseCanExecuteChanged();

      DeleteGroupingCommand = new ViewModelCommand(x => RemoveShooterFromGrouping((IWindow)x));
      DeleteGroupingCommand.AddGuard(x => SelectedGrouping != null);
      DeleteGroupingCommand.RaiseCanExecuteChanged();

      DeleteParticipationCommand = new ViewModelCommand(x => DeleteParticipation((IWindow)x));
      DeleteParticipationCommand.AddGuard(x => SelectedParticipation != null);
      DeleteParticipationCommand.RaiseCanExecuteChanged();

      ShowCreateGroupingCommand = new ViewModelCommand(x => ShowCreateGrouping((IWindow)x));
      ShowCreateGroupingCommand.RaiseCanExecuteChanged();

      _shooterParticipationDataStore = ConfigurationSource.Configuration.GetShooterParticipationDataStore();
      _shooterDataStore = ConfigurationSource.Configuration.GetShooterDataStore();
      _participationDataStore = ConfigurationSource.Configuration.GetParticipationDataStore();
      _collectionShooterDataStore = ConfigurationSource.Configuration.GetCollectionShooterDataStore();
      _shooterCollectionDataStore = ConfigurationSource.Configuration.GetShooterCollectionDataStore();
      _shooterCollectionParticipationDataStore = ConfigurationSource.Configuration.GetShooterCollectionParticipationDataStore();

      Initialize(shooter);
      SelectedParticipation = null;
    }

    private void ShowCreateGrouping(IWindow parent)
    {
      GroupingHelper.ShowCreateGrouping(parent);
    }

    private void RemoveShooterFromGrouping(IWindow parent)
    {
      YesNoMessageBoxViewModel vm = new YesNoMessageBoxViewModel
      {
        DefaultYes = false,
        Caption = "Gruppierungszugehörigkeit löschen?",
        MessageBoxText =
          string.Format("Möchtest du die Gruppierungszugehörigkeit '{0}' wirklich löschen?", SelectedGrouping.GroupingName)
      };

      bool? result = ViewServiceLocator.ViewService.ExecuteFunction<YesNoMessageBoxViewModel, bool?>(parent, vm);
      if (result.HasValue && result.Value)
      {
        CollectionShooter cs = _collectionShooterDataStore.FindById(SelectedGrouping.CollectionShooterId);
        _collectionShooterDataStore.Delete(cs);
        Initialize(Shooter);
      }
    }

    private void DeleteParticipation(IWindow window)
    {
      YesNoMessageBoxViewModel vm = new YesNoMessageBoxViewModel
      {
        DefaultYes = false,
        Caption = "Wettkampfteilnahme löschen?",
        MessageBoxText =
          string.Format("Möchtest du die Wettkampfteilnahme '{0}' wirklich löschen?", SelectedParticipation.ProgramName)
      };

      bool? result = ViewServiceLocator.ViewService.ExecuteFunction<YesNoMessageBoxViewModel, bool?>(window, vm);
      if (result.HasValue && result.Value)
      {
        ShooterParticipation sp = (from shooterParticipation in _shooterParticipationDataStore.GetAll()
          where
            shooterParticipation.ShooterId == Shooter.ShooterId &&
            shooterParticipation.ParticipationId == SelectedParticipation.ParticipationId
          select shooterParticipation).FirstOrDefault();
        if (sp != null)
        {
          _shooterParticipationDataStore.Delete(sp);
          Initialize(Shooter);
        }
      }
    }

    private void ShowSelectGrouping(IWindow parent)
    {
      GroupingHelper.ShowSelectGrouping(parent, Shooter);
      Initialize(Shooter);
    }

    private void ShowSelectParticipation(IWindow window)
    {
      SelectParticipationViewModel vm = new SelectParticipationViewModel
      {
        Title = "Wettkampf auswählen"
      };
      vm.Initialize();

      bool? dialogResult = DialogHelper.ShowDialog(window, vm, vm.Title);
      if (dialogResult.HasValue && dialogResult.Value)
      {
        ShooterParticipation sp = new ShooterParticipation
        {
          ShooterId = Shooter.ShooterId,
          ParticipationId = vm.SelectedParticipation.ParticipationId
        };

        try
        {
          _shooterParticipationDataStore.Create(sp);
        }
        catch (Exception e)
        {
          DialogHelper.ShowErrorDialog(window,
            "Fehler beim speichern",
            string.Format("Der Datensatz konnte nicht gespeichert werden.\r\n\r\n{0}", e));
        }

        Initialize(Shooter);
      }
    }

    private void Initialize(Shooter shooter)
    {
      Shooter = shooter;
      Participations = new ObservableCollection<ParticipationViewModel>(FetchParticipationsByShooter(Shooter));
      Groupings = new ObservableCollection<GroupingViewModel>(FetchGroupsByShooter(Shooter));
    }

    private IEnumerable<GroupingViewModel> FetchGroupsByShooter(Shooter shooter)
    {
      return from collectionShooter in _collectionShooterDataStore.FindByShooterId(shooter.ShooterId)
        join shooterCollection in _shooterCollectionDataStore.GetAll() on
          collectionShooter.ShooterCollectionId equals shooterCollection.ShooterCollectionId
        join shooterCollectionParticipation in _shooterCollectionParticipationDataStore.GetAll() on
          shooterCollection.ShooterCollectionId equals shooterCollectionParticipation.ShooterCollectionId
        join participation in _participationDataStore.GetAll() on shooterCollectionParticipation.ParticipationId equals
          participation.ParticipationId
        orderby shooterCollection.CollectionName
        select new GroupingViewModel
        {
          CollectionShooterId = collectionShooter.CollectionShooterId,
          GroupingName = shooterCollection.CollectionName,
          ParticipationName = participation.ParticipationName
        };
    }

    private IEnumerable<ParticipationViewModel> FetchParticipationsByShooter(Shooter shooter)
    {
      return from shooterParticipation in _shooterParticipationDataStore.FindByShooterId(shooter.ShooterId)
        join participation in _participationDataStore.GetAll() on shooterParticipation.ParticipationId equals
          participation.ParticipationId
        orderby participation.ParticipationName
        select new ParticipationViewModel(participation.ParticipationId)
        {
          ProgramName = participation.ParticipationName
        };
    }

    public ViewModelCommand ShowSelectGroupingCommand { get; private set; }
    public ViewModelCommand ShowSelectParticipationCommand { get; private set; }

    public ViewModelCommand DeleteGroupingCommand { get; private set; }
    public ViewModelCommand DeleteParticipationCommand { get; private set; }

    public ViewModelCommand ShowCreateGroupingCommand { get; private set; }
    #region Properties


    private GroupingViewModel _selectedGrouping;
    public GroupingViewModel SelectedGrouping
    {
      get { return _selectedGrouping; }
      set
      {
        if (value != _selectedGrouping)
        {
          _selectedGrouping = value;
          OnPropertyChanged("SelectedGrouping");

          DeleteGroupingCommand.RaiseCanExecuteChanged();
        }
      }
    }

    private ObservableCollection<GroupingViewModel> _groupings;

    public ObservableCollection<GroupingViewModel> Groupings
    {
      get { return _groupings; }
      set
      {
        if (value != _groupings)
        {
          _groupings = value;
          OnPropertyChanged("Groupings");
        }
      }
    }


    private ParticipationViewModel _selectedParticipation;
    public ParticipationViewModel SelectedParticipation
    {
      get { return _selectedParticipation; }
      set
      {
        if (value != _selectedParticipation)
        {
          _selectedParticipation = value;
          OnPropertyChanged("SelectedParticipation");

          DeleteParticipationCommand.RaiseCanExecuteChanged();
        }
      }
    }

    private ObservableCollection<ParticipationViewModel> _participations;

    public ObservableCollection<ParticipationViewModel> Participations
    {
      get { return _participations; }
      set
      {
        if (value != _participations)
        {
          _participations = value;
          OnPropertyChanged("Participations");
        }
      }
    }

    private Shooter _shooter;

    public Shooter Shooter
    {
      get { return _shooter; }
      set
      {
        if (value != _shooter)
        {
          _shooter = value;
          OnPropertyChanged("Shooter");
        }
      }
    }

    #endregion


    #region INotifyPropertyChanged Members

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      var handler = PropertyChanged;
      if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
  }
}