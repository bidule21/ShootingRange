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

namespace ShootingRange.ServiceDesk.ViewModel
{
  public class GroupingPageGroupingViewModel : INotifyPropertyChanged
  {

    public GroupingPageGroupingViewModel()
    {
      NewGroupingCommand = new ViewModelCommand(x => ShowCreateGrouping((IWindow) x));
      NewGroupingCommand.RaiseCanExecuteChanged();

      DeleteGroupingCommand = new ViewModelCommand(x => DeleteGrouping((IWindow) x));
      DeleteGroupingCommand.AddGuard(x => SelectedShooterCollection != null);
      DeleteGroupingCommand.RaiseCanExecuteChanged();

      RemoveShooterFromGroupingCommand = new ViewModelCommand(x => RemoveShooterFromGrouping((IWindow) x));
      //RemoveShooterFromGroupingCommand.AddGuard(x => SelectedShooterCollection != null && SelectedShooterCollection.SelectedShooter != null);
      RemoveShooterFromGroupingCommand.RaiseCanExecuteChanged();

      AddShooterToGroupingCommand = new ViewModelCommand(x => AddShooter((IWindow)x));
      AddShooterToGroupingCommand.RaiseCanExecuteChanged();
    }

    private void AddShooter(IWindow parent)
    {
      if (SelectedShooterCollection != null)
      {
        SelectShooterViewModel vm = new SelectShooterViewModel
        {
          Title = "Schütze auswählen"
        };
        vm.Initialize(GroupTypeId);

        bool? dialogResult = DialogHelper.ShowDialog(parent, vm, vm.Title);

        if (dialogResult.HasValue && dialogResult.Value)
        {
          if (vm.SelectedShooter != null)
          {
            ICollectionShooterDataStore csds = ConfigurationSource.Configuration.GetCollectionShooterDataStore();
            CollectionShooter cs = new CollectionShooter
            {
              ShooterId = vm.SelectedShooter.ShooterId,
              ShooterCollectionId = SelectedShooterCollection.ShooterCollectionId
            };
            csds.Create(cs);
          }
        }

        int selectionBkp = SelectedShooterCollection.ShooterCollectionId;
        Load();
        SelectedShooterCollection = ShooterCollections.SingleOrDefault(sc => sc.ShooterCollectionId == selectionBkp);
      }
    }

    private void RemoveShooterFromGrouping(IWindow window)
    {
      if (SelectedShooterCollection != null && SelectedShooterCollection.SelectedShooter != null)
      {
        int bkp = SelectedShooterCollection.ShooterCollectionId;
        ICollectionShooterDataStore collectionShooterDataStore =
          ConfigurationSource.Configuration.GetCollectionShooterDataStore();

        CollectionShooter collectionShooter =
          collectionShooterDataStore.FindByShooterId(SelectedShooterCollection.SelectedShooter.ShooterId)
            .SingleOrDefault(cs => cs.ShooterCollectionId == SelectedShooterCollection.ShooterCollectionId);

        collectionShooterDataStore.Delete(collectionShooter);
        Load();
        SelectedShooterCollection = ShooterCollections.FirstOrDefault(sc => sc.ShooterCollectionId == bkp);
      }
    }

    private void DeleteGrouping(IWindow window)
    {
      if (SelectedShooterCollection != null)
      {
        YesNoMessageBoxViewModel vm = new YesNoMessageBoxViewModel
        {
          Caption = "Gruppierung löschen",
          MessageBoxText = string.Format("Wollen Sie die Gruppierung '{0}' wirklich löschen?", SelectedShooterCollection.CollectionName)
        };

        bool? result = ViewServiceLocator.ViewService.ExecuteFunction<YesNoMessageBoxViewModel, bool?>(window, vm);
        if (result.HasValue && result.Value)
        {
          IShooterCollectionDataStore shooterCollectionDataStore =
            ConfigurationSource.Configuration.GetShooterCollectionDataStore();

          ShooterCollection sc = new ShooterCollection
          {
            ShooterCollectionId = SelectedShooterCollection.ShooterCollectionId
          };

          shooterCollectionDataStore.Delete(sc);
          Load();
        }
      }
    }

    public ViewModelCommand NewGroupingCommand { get; private set; }

    public ViewModelCommand DeleteGroupingCommand { get; private set; }

    public ViewModelCommand RemoveShooterFromGroupingCommand { get; private set; }

    public ViewModelCommand AddShooterToGroupingCommand { get; private set; }

    public void Load()
    {
      LoadShooterCollections(GroupTypeId);
    }

    private void ShowCreateGrouping(IWindow parent)
    {
      GroupingHelper.ShowCreateGrouping(parent);
      LoadShooterCollections(GroupTypeId);
    }


    private int _groupTypeId;
    public int GroupTypeId
    {
      get { return _groupTypeId; }
      set
      {
        if (value != _groupTypeId)
        {
          _groupTypeId = value;
          OnPropertyChanged("GroupTypeId");

          LoadShooterCollections(value);
        }
      }
    }

    private void LoadShooterCollections(int groupTypeId)
    {
      IParticipationDataStore participationDataStore = ConfigurationSource.Configuration.GetParticipationDataStore();
      GroupingType = participationDataStore.GetAll().Single(p => p.ParticipationId == groupTypeId).ParticipationName;

      IShooterCollectionDataStore shooterCollectionDataStore =
        ConfigurationSource.Configuration.GetShooterCollectionDataStore();
      ICollectionShooterDataStore collectionShooterDataStore =
        ConfigurationSource.Configuration.GetCollectionShooterDataStore();
      IShooterDataStore shooterDataStore = ConfigurationSource.Configuration.GetShooterDataStore();
      IPersonDataStore personDataStore = ConfigurationSource.Configuration.GetPersonDataStore();
      IShooterCollectionParticipationDataStore shooterCollectionParticipationDataStore =
        ConfigurationSource.Configuration.GetShooterCollectionParticipationDataStore();

      IEnumerable<ShooterCollectionViewModel> scvm = from sc in shooterCollectionDataStore.GetAll()
        join scp in shooterCollectionParticipationDataStore.GetAll() on sc.ShooterCollectionId equals
          scp.ShooterCollectionId
        where scp.ParticipationId == groupTypeId
        select new ShooterCollectionViewModel
        {
          CollectionName = sc.CollectionName,
          ShooterCollectionId = sc.ShooterCollectionId,
          Shooters = new ObservableCollection<PersonShooterViewModel>(from cs in collectionShooterDataStore.GetAll()
            where cs.ShooterCollectionId == sc.ShooterCollectionId
            join s in shooterDataStore.GetAll() on cs.ShooterId equals s.ShooterId
            join p in personDataStore.GetAll() on s.PersonId equals p.PersonId
            orderby p.FirstName
            orderby p.LastName
            select new PersonShooterViewModel
            {
              ShooterId = s.ShooterId,
              PersonId = p.PersonId,
              FirstName = p.FirstName,
              LastName = p.LastName,
              ShooterNumber = s.ShooterNumber
            })
        };

      ShooterCollections = new ObservableCollection<ShooterCollectionViewModel>(scvm.OrderBy(_ => _.CollectionName));
    }

    private string _groupingType;
    public string GroupingType
    {
      get { return _groupingType; }
      set
      {
        if (value != _groupingType)
        {
          _groupingType = value;
          OnPropertyChanged("GroupingType");
        }
      }
    }

    private ObservableCollection<ShooterCollectionViewModel> _shooterCollections;
    public ObservableCollection<ShooterCollectionViewModel> ShooterCollections
    {
      get { return _shooterCollections; }
      set
      {
        if (value != _shooterCollections)
        {
          _shooterCollections = value;
          OnPropertyChanged("ShooterCollections");
        }
      }
    }


    private ShooterCollectionViewModel _selectedShooterCollection;
    public ShooterCollectionViewModel SelectedShooterCollection
    {
      get { return _selectedShooterCollection; }
      set
      {
        if (value != _selectedShooterCollection)
        {
          _selectedShooterCollection = value;
          OnPropertyChanged("SelectedShooterCollection");

          DeleteGroupingCommand.RaiseCanExecuteChanged();
        }
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      var handler = PropertyChanged;
      if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}