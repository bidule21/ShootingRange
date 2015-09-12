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
        private ISessionDataStore _sessionDataStore;
        private ISessionSubtotalDataStore _sessionSubtotalDataStore;
        private IShotDataStore _shotDataStore;
        private ServiceDeskConfiguration _sdk;

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

            ReassignSessionCommand = new ViewModelCommand(
        x => MessengerInstance.Send(new ShowReassignSessionDialogMessage(SelectedSession.SessionId)));
            ReassignSessionCommand.AddGuard(x => SelectedSession != null);
            ReassignSessionCommand.RaiseCanExecuteChanged();

            ReassignProgramNumberCommand = new ViewModelCommand(x => MessengerInstance.Send(new ShowReassignShooterNumberDialogMessage(SelectedSession.SessionId)));
            ReassignProgramNumberCommand.AddGuard(x => SelectedSession != null);
            ReassignProgramNumberCommand.RaiseCanExecuteChanged();
        }

        public void Initialize(Shooter shooter)
        {
            _shooterParticipationDataStore = ServiceLocator.Current.GetInstance<IShooterParticipationDataStore>();
            _collectionShooterDataStore = ServiceLocator.Current.GetInstance<ICollectionShooterDataStore>();
            _shooterCollectionDataStore = ServiceLocator.Current.GetInstance<IShooterCollectionDataStore>();
            _sessionDataStore = ServiceLocator.Current.GetInstance<ISessionDataStore>();
            _sessionSubtotalDataStore = ServiceLocator.Current.GetInstance<ISessionSubtotalDataStore>();
            _shotDataStore = ServiceLocator.Current.GetInstance<IShotDataStore>();
            _sdk = ServiceLocator.Current.GetInstance<ServiceDeskConfiguration>();

            SelectedGrouping = null;
            SelectedParticipation = null;
            Shooter = shooter;
            Participations = new ObservableCollection<ParticipationViewModel>(FetchParticipationsByShooter(Shooter));
            Groupings = new ObservableCollection<GroupingViewModel>(FetchGroupsByShooter(Shooter));
            SelectedPersonChanged(shooter.ShooterId);

            MessengerInstance.Register<RefreshDataFromRepositories>(this,
    x =>
    {
        Groupings = new ObservableCollection<GroupingViewModel>(FetchGroupsByShooter(Shooter));
        Participations = new ObservableCollection<ParticipationViewModel>(FetchParticipationsByShooter(Shooter));
        SelectedPersonChanged(Shooter.ShooterId);
    });
        }

        private void SelectedPersonChanged(int shooterId)
        {
            if (MessengerInstance == null) return;
            Dictionary<int, List<int>> sessionToSubsession = (from session in
                _sessionDataStore.FindByShooterId(shooterId)
                join subsession in _sessionSubtotalDataStore.GetAll() on session.SessionId equals
                    subsession.SessionId
                orderby session.ProgramNumber
                group subsession by session.SessionId).ToDictionary(_ => _.Key,
                    _ => _.Select(fo => fo.SessionSubtotalId).ToList());

            Sessions = new ObservableCollection<SessionViewModel>();
            foreach (KeyValuePair<int, List<int>> keyValuePair in sessionToSubsession)
            {
                Session session = _sessionDataStore.FindById(keyValuePair.Key);

                {
                    ParticipationDescription participation =
                        _sdk.ParticipationDescriptions.GetAll()
                            .SingleOrDefault(x => x.ProgramNumber == session.ProgramNumber.ToString());
                    string programName = participation == null ? "unknown" : participation.ProgramName;

                    SessionViewModel svm = new SessionViewModel
                    {
                        LaneNumber = session.LaneNumber,
                        ProgramName = string.Format("{0} [{1}]", programName, session.ProgramNumber),
                        SessionId = session.SessionId,
                        ShooterIsParticipating =
                            _shooterParticipationDataStore.FindByShooterId(shooterId)
                                .Any(x => x.ProgramNumber == session.ProgramNumber)
                    };

                    List<Shot> shots = new List<Shot>();
                    foreach (int subsessionId in keyValuePair.Value)
                    {
                        shots.AddRange(_shotDataStore.FindBySubSessionId(subsessionId).OrderBy(shot => shot.Ordinal));
                    }

                    svm.Shots = new ObservableCollection<Shot>(shots);
                    svm.Total = shots.Sum(s => s.PrimaryScore);
                    Sessions.Add(svm);
                }
            }
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

        #region Results

        public ViewModelCommand ReassignSessionCommand { get; private set; }

        public ViewModelCommand ReassignProgramNumberCommand { get; private set; }

        private SessionViewModel _selectedSession;

        public SessionViewModel SelectedSession
        {
            get { return _selectedSession; }
            set
            {
                if (value != _selectedSession)
                {
                    _selectedSession = value;
                    OnPropertyChanged();

                    ReassignSessionCommand.RaiseCanExecuteChanged();
                    ReassignProgramNumberCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private ObservableCollection<SessionViewModel> _sessions;

        public ObservableCollection<SessionViewModel> Sessions
        {
            get { return _sessions; }
            set
            {
                if (value != _sessions)
                {
                    _sessions = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion
    }
}