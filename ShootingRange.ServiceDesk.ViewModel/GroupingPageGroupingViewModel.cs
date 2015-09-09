using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Gui.ViewModel;
using Microsoft.Practices.ServiceLocation;
using ShootingRange.BusinessObjects;
using ShootingRange.Repository.RepositoryInterfaces;
using ShootingRange.ServiceDesk.ViewModel.MessageTypes;

namespace ShootingRange.ServiceDesk.ViewModel
{
    public class GroupingPageGroupingViewModel : Gui.ViewModel.ViewModel
    {

        public GroupingPageGroupingViewModel()
        {
            NewGroupingCommand = new ViewModelCommand(x => ShowCreateGrouping((IWindow) x));
            NewGroupingCommand.RaiseCanExecuteChanged();

            EditGroupingCommand = new ViewModelCommand(x => MessengerInstance.Send(new EditGroupingDialogMessage(SelectedShooterCollection.ShooterCollectionId, SelectedShooterCollection.CollectionName)));
            EditGroupingCommand.AddGuard(x => SelectedShooterCollection != null);
            EditGroupingCommand.RaiseCanExecuteChanged();

            DeleteGroupingCommand = new ViewModelCommand(x => MessengerInstance.Send(new DeleteGroupingDialogMessage(SelectedShooterCollection)));
            DeleteGroupingCommand.AddGuard(x => SelectedShooterCollection != null);
            DeleteGroupingCommand.RaiseCanExecuteChanged();

            RemoveShooterFromGroupingCommand = new ViewModelCommand(x => RemoveShooterFromGrouping((IWindow) x));
            RemoveShooterFromGroupingCommand.AddGuard(x => SelectedShooterCollection != null && SelectedShooter != null);
            RemoveShooterFromGroupingCommand.RaiseCanExecuteChanged();

            AddShooterToGroupingCommand = new ViewModelCommand(x => MessengerInstance.Send(new AddShooterToGroupingDialogMessage(ProgramNumber, SelectedShooterCollection.ShooterCollectionId)));
            AddShooterToGroupingCommand.AddGuard(x => SelectedShooterCollection != null);
            AddShooterToGroupingCommand.RaiseCanExecuteChanged();

            MessengerInstance.Register<RefreshDataFromRepositories>(this,
                x =>
                {
                    LoadShooterCollections(ProgramNumber);
                });

            MessengerInstance.Register<SetSelectedShooterCollectionMessage>(this,
                x =>
                {
                    SelectedShooterCollection =
                        ShooterCollections.FirstOrDefault(sc => sc.ShooterCollectionId == x.ShooterCollectionId);
                });
        }

        private void RemoveShooterFromGrouping(IWindow window)
        {
            if (SelectedShooterCollection != null && SelectedShooter != null)
            {
                int shooterCollectionId = SelectedShooterCollection.ShooterCollectionId;
                ICollectionShooterDataStore collectionShooterDataStore = ServiceLocator.Current.GetInstance<ICollectionShooterDataStore>();

                CollectionShooter collectionShooter =
                    collectionShooterDataStore.FindByShooterId(SelectedShooter.ShooterId)
                        .SingleOrDefault(cs => cs.ShooterCollectionId == SelectedShooterCollection.ShooterCollectionId);

                collectionShooterDataStore.Delete(collectionShooter);
                MessengerInstance.Send(new RefreshDataFromRepositories());
                MessengerInstance.Send(new SetSelectedShooterCollectionMessage(shooterCollectionId));
            }
        }

        public ViewModelCommand NewGroupingCommand { get; private set; }

        public ViewModelCommand EditGroupingCommand { get; private set; }
        public ViewModelCommand DeleteGroupingCommand { get; private set; }

        public ViewModelCommand RemoveShooterFromGroupingCommand { get; private set; }

        public ViewModelCommand AddShooterToGroupingCommand { get; private set; }

        public void Load()
        {
            LoadShooterCollections(ProgramNumber);
        }

        private void ShowCreateGrouping(IWindow parent)
        {
            MessengerInstance.Send(new CreateGroupingDialogMessage(parent));
            LoadShooterCollections(ProgramNumber);
        }


        private PersonShooterViewModel _selectedShooter;
        public PersonShooterViewModel SelectedShooter
        {
            get { return _selectedShooter; }
            set
            {
                if (value != _selectedShooter)
                {
                    _selectedShooter = value;
                    OnPropertyChanged();

                    RemoveShooterFromGroupingCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private int _programNumber;

        public int ProgramNumber
        {
            get { return _programNumber; }
            set
            {
                if (value != _programNumber)
                {
                    _programNumber = value;
                    OnPropertyChanged("ProgramNumber");

                    LoadShooterCollections(value);
                }
            }
        }

        private void LoadShooterCollections(int programNumber)
        {
            IShooterCollectionDataStore shooterCollectionDataStore = ServiceLocator.Current.GetInstance<IShooterCollectionDataStore>();
            ICollectionShooterDataStore collectionShooterDataStore = ServiceLocator.Current.GetInstance<ICollectionShooterDataStore>();
            IShooterDataStore shooterDataStore = ServiceLocator.Current.GetInstance<IShooterDataStore>();
            IPersonDataStore personDataStore = ServiceLocator.Current.GetInstance<IPersonDataStore>();

            IEnumerable<ShooterCollectionViewModel> scvm = from sc in shooterCollectionDataStore.GetAll()
              where sc.ProgramNumber == programNumber
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

                    RemoveShooterFromGroupingCommand.RaiseCanExecuteChanged();
                    AddShooterToGroupingCommand.RaiseCanExecuteChanged();
                    EditGroupingCommand.RaiseCanExecuteChanged();
                    DeleteGroupingCommand.RaiseCanExecuteChanged();
                }
            }
        }
    }
}