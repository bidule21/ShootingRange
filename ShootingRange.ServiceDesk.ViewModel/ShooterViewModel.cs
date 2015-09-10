using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Gui.ViewModel;
using Microsoft.Practices.ServiceLocation;
using ShootingRange.BusinessObjects;
using ShootingRange.ConfigurationProvider;
using ShootingRange.Repository.RepositoryInterfaces;
using ShootingRange.ServiceDesk.ViewModel.MessageTypes;

namespace ShootingRange.ServiceDesk.ViewModel
{
    public class ShooterViewModel : Gui.ViewModel.ViewModel
    {
        private IShooterParticipationDataStore _shooterParticipationDataStore;
        private ICollectionShooterDataStore _collectionShooterDataStore;
        private IShooterCollectionDataStore _shooterCollectionDataStore;

        public ShooterViewModel()
        {
            ShowSelectGroupingCommand = new ViewModelCommand(x => MessengerInstance.Send(new AddGroupingToShooterDialogMessage(Shooter.ShooterId)));
            ShowSelectGroupingCommand.RaiseCanExecuteChanged();

            ShowSelectParticipationCommand = new ViewModelCommand(x => MessengerInstance.Send(new AddParticipationToShooterDialogMessage(Shooter.ShooterId)));
            ShowSelectParticipationCommand.RaiseCanExecuteChanged();

            DeleteGroupingCommand = new ViewModelCommand(x => MessengerInstance.Send(new RemoveGroupingFromShooterDialogMessage(Shooter.ShooterId, SelectedGrouping)));
            DeleteGroupingCommand.AddGuard(x => SelectedGrouping != null);
            DeleteGroupingCommand.RaiseCanExecuteChanged();

            DeleteParticipationCommand = new ViewModelCommand(x => MessengerInstance.Send(new RemoveParticipationFromShooterDialogMessage(Shooter.ShooterId, SelectedParticipation)));
            DeleteParticipationCommand.AddGuard(x => SelectedParticipation != null);
            DeleteParticipationCommand.RaiseCanExecuteChanged();
        }

        public void Initialize(Shooter shooter)
        {
            _shooterParticipationDataStore = ServiceLocator.Current.GetInstance<IShooterParticipationDataStore>();
            _collectionShooterDataStore = ServiceLocator.Current.GetInstance<ICollectionShooterDataStore>();
            _shooterCollectionDataStore = ServiceLocator.Current.GetInstance<IShooterCollectionDataStore>();

            SelectedGrouping = null;
            SelectedParticipation = null;
            Shooter = shooter;
            Participations = new ObservableCollection<ParticipationViewModel>(FetchParticipationsByShooter(Shooter));
            Groupings = new ObservableCollection<GroupingViewModel>(FetchGroupsByShooter(Shooter));

            MessengerInstance.Register<RefreshDataFromRepositories>(this,
    x =>
    {
        Groupings = new ObservableCollection<GroupingViewModel>(FetchGroupsByShooter(Shooter));
        Participations = new ObservableCollection<ParticipationViewModel>(FetchParticipationsByShooter(Shooter));
    });
        }

        private IEnumerable<GroupingViewModel> FetchGroupsByShooter(Shooter shooter)
        {
            ServiceDeskConfiguration sdk = ServiceLocator.Current.GetInstance<ServiceDeskConfiguration>();
            ParticipationDescriptionCollection participations = sdk.ParticipationDescriptions;

            return from collectionShooter in _collectionShooterDataStore.FindByShooterId(shooter.ShooterId)
                join shooterCollection in _shooterCollectionDataStore.GetAll() on collectionShooter.ShooterCollectionId equals shooterCollection.ShooterCollectionId
                   join participation in participations.GetAll() on shooterCollection.ProgramNumber.ToString()
                    equals
                    participation.ProgramNumber
                orderby shooterCollection.CollectionName
                select new GroupingViewModel
                {
                    ShooterCollectionId = collectionShooter.CollectionShooterId,
                    GroupingName = shooterCollection.CollectionName,
                    ParticipationName = participation.ProgramName
                };
        }

        private IEnumerable<ParticipationViewModel> FetchParticipationsByShooter(Shooter shooter)
        {
            ServiceDeskConfiguration sdk = ServiceLocator.Current.GetInstance<ServiceDeskConfiguration>();
            ParticipationDescriptionCollection participations = sdk.ParticipationDescriptions;

            return from shooterParticipation in _shooterParticipationDataStore.FindByShooterId(shooter.ShooterId)
                join participation in participations.GetAll() on shooterParticipation.ProgramNumber.ToString() equals
                    participation.ProgramNumber
                orderby participation.ProgramNumber
                select new ParticipationViewModel
                {
                    ProgramName = participation.ProgramName,
                    ProgramNumber = shooterParticipation.ProgramNumber
                };
        }

        public ViewModelCommand ShowSelectGroupingCommand { get; private set; }
        public ViewModelCommand ShowSelectParticipationCommand { get; private set; }

        public ViewModelCommand DeleteGroupingCommand { get; private set; }
        public ViewModelCommand DeleteParticipationCommand { get; private set; }


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
                    OnPropertyChanged();

                    DeleteGroupingCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private ObservableCollection<GroupingViewModel> _groupings;

        public ObservableCollection<GroupingViewModel> Groupings
        {
            get
            {
                return _groupings;
            }
            set
            {
                if (value != _groupings)
                {
                    _groupings = value;
                    OnPropertyChanged();
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
                    OnPropertyChanged();

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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
                }
            }
        }

        #endregion

    }
}