using System;
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
    public class ResultsPageViewModel : Gui.ViewModel.ViewModel
    {
        private string _personFilterText;
        private IPersonDataStore _personDataStore;
        private IShooterDataStore _shooterDataStore;
        private ISessionDataStore _sessionDataStore;
        private ISessionSubtotalDataStore _sessionSubtotalDataStore;
        private IShotDataStore _shotDataStore;
        private IShooterParticipationDataStore _shooterParticipationDataStore;
        private ServiceDeskConfiguration _sdk;

        public ResultsPageViewModel()
        {
            if (MessengerInstance != null)
            {
                MessengerInstance.Register<RefreshDataFromRepositoriesMessage>(this,
                    x =>
                    {
                        PersonShooterViewModel bkp = SelectedPerson;
                        LoadPersons();
                        SelectedPerson = bkp;
                        SelectedPersonChanged();
                    });

                ReassignSessionCommand =
                    new ViewModelCommand(
                        x => MessengerInstance.Send(new ShowReassignSessionDialogMessage(SelectedSession.SessionId)));
                ReassignSessionCommand.AddGuard(x => SelectedSession != null);
                ReassignSessionCommand.RaiseCanExecuteChanged();

                ReassignProgramNumberCommand = new ViewModelCommand(x => MessengerInstance.Send(new ShowReassignShooterNumberDialogMessage(SelectedSession.SessionId)));
                ReassignProgramNumberCommand.AddGuard(x => SelectedSession != null);
                ReassignProgramNumberCommand.RaiseCanExecuteChanged();
            }
        }

        public ViewModelCommand ReassignSessionCommand { get; private set; }

        public ViewModelCommand ReassignProgramNumberCommand { get; private set; }

        public void Initialize()
        {
            _personDataStore = ServiceLocator.Current.GetInstance<IPersonDataStore>();
            _shooterDataStore = ServiceLocator.Current.GetInstance<IShooterDataStore>();
            _sessionDataStore = ServiceLocator.Current.GetInstance<ISessionDataStore>();
            _sessionSubtotalDataStore = ServiceLocator.Current.GetInstance<ISessionSubtotalDataStore>();
            _shotDataStore = ServiceLocator.Current.GetInstance<IShotDataStore>();
            _shooterParticipationDataStore = ServiceLocator.Current.GetInstance<IShooterParticipationDataStore>();
            _sdk = ServiceLocator.Current.GetInstance<ServiceDeskConfiguration>();
        }

        public List<PersonShooterViewModel> AllPersons { get; set; }

        private void LoadPersons()
        {
            IEnumerable<PersonShooterViewModel> shooters = from shooter in _shooterDataStore.GetAll()
                join person in _personDataStore.GetAll() on shooter.PersonId equals person.PersonId
                orderby person.FirstName
                orderby person.LastName
                select new PersonShooterViewModel
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    DateOfBirth = person.DateOfBirth,
                    PersonId = person.PersonId,
                    ShooterId = shooter.ShooterId,
                    ShooterNumber = shooter.ShooterNumber
                };

            AllPersons = shooters.ToList();
            UpdateFilteredPersons();
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

        private PersonShooterViewModel _selectedPerson;

        public PersonShooterViewModel SelectedPerson
        {
            get { return _selectedPerson; }
            set
            {
                if (value != _selectedPerson)
                {
                    _selectedPerson = value;
                    OnPropertyChanged();

                    SelectedPersonChanged();
                }
            }
        }

        private void SelectedPersonChanged()
        {
            if (MessengerInstance == null) return;

            if (SelectedPerson != null)
            {
                Dictionary<int, List<int>> sessionToSubsession = (from session in
                    _sessionDataStore.FindByShooterId(SelectedPerson.ShooterId)
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
                            ShooterIsParticipating = _shooterParticipationDataStore.FindByShooterId(SelectedPerson.ShooterId).Any(x => x.ProgramNumber == session.ProgramNumber)
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
            else
            {
                Sessions = new ObservableCollection<SessionViewModel>();
            }
        }

        private ObservableCollection<PersonShooterViewModel> _filteredPersons;

        public ObservableCollection<PersonShooterViewModel> FilteredPersons
        {
            get { return _filteredPersons; }
            set
            {
                if (value != _filteredPersons)
                {
                    _filteredPersons = value;
                    OnPropertyChanged("FilteredPersons");
                }
            }
        }

        public string PersonFilterText
        {
            get { return _personFilterText; }
            set
            {
                if (value != _personFilterText)
                {
                    _personFilterText = value;
                    OnPropertyChanged("PersonFilterText");
                    UpdateFilteredPersons();
                }
            }
        }

        private IEnumerable<PersonShooterViewModel> FilterPersons(IEnumerable<PersonShooterViewModel> persons,
            string filterText)
        {
            if (filterText == null) filterText = string.Empty;
            string[] split = filterText.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

            foreach (PersonShooterViewModel person in persons)
            {
                if (string.IsNullOrWhiteSpace(filterText))
                {
                    yield return person;
                }
                else
                {
                    if (
                        split.All(
                            x =>
                                person.FirstName.IndexOf(x, StringComparison.InvariantCultureIgnoreCase) != -1 ||
                                person.LastName.IndexOf(x, StringComparison.InvariantCultureIgnoreCase) != -1 ||
                                person.ShooterNumber.ToString().IndexOf(x, StringComparison.CurrentCultureIgnoreCase) !=
                                -1))
                    {
                        yield return person;
                    }
                }
            }
        }

        private void UpdateFilteredPersons()
        {
            FilteredPersons =
                new ObservableCollection<PersonShooterViewModel>(FilterPersons(AllPersons, PersonFilterText));
            SelectedPersonChanged();
        }
    }
}