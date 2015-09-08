using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Gui.ViewModel;
using Microsoft.Practices.ServiceLocation;
using ShootingRange.BusinessObjects;
using ShootingRange.Repository.RepositoryInterfaces;
using ShootingRange.Service.Interface;
using ShootingRange.ServiceDesk.ViewModel.MessageTypes;

namespace ShootingRange.ServiceDesk.ViewModel
{
    public class PersonsPageViewModel : Gui.ViewModel.ViewModel
    {
        private readonly IPersonDataStore _personDataStore;
        private readonly IShooterDataStore _shooterDataStore;
        private readonly IShooterNumberService _shooterNumberService;
        private ISsvShooterDataWriterService _shooterDataWriter;

        private List<Person> _allPersons;

        public PersonsPageViewModel()
        {
            _personDataStore = ServiceLocator.Current.GetInstance<IPersonDataStore>();
            _shooterDataStore = ServiceLocator.Current.GetInstance<IShooterDataStore>();
            _shooterNumberService = ServiceLocator.Current.GetInstance<IShooterNumberService>();
            _shooterDataWriter = ServiceLocator.Current.GetInstance<ISsvShooterDataWriterService>();

            MessengerInstance.Register<ShowPersonsPageMessage>(this, x => LoadPersons());
            MessengerInstance.Register<PersonSelectedMessage>(this,
                x =>
                {
                    UpdateCommandCanExecuteOnSelectedPersonChanged();
                    LoadShooters(x.Person);
                });
            MessengerInstance.Register<SetSelectedPersonMessage>(this,
                x =>
                {
                    SelectedPerson = FilteredPersons.FirstOrDefault(person => person.PersonId == x.PersonId);
                });
            MessengerInstance.Register<SetSelectedShooterMessage>(this,
                x =>
                {
                    SelectedShooter = Shooters.FirstOrDefault(s => s.Shooter.ShooterId == x.ShooterId);
                    if (SelectedShooter == null)
                        SelectedShooter = Shooters.FirstOrDefault();
                });

            MessengerInstance.Register<RefreshDataFromDatabase>(this,
                x =>
                {
                    Person selectedPerson = SelectedPerson;
                    ShooterViewModel selectedShooter = SelectedShooter;
                    LoadPersons();

                    if(selectedPerson != null)
                        MessengerInstance.Send(new SetSelectedPersonMessage(selectedPerson.PersonId));

                    if (selectedShooter != null)
                        MessengerInstance.Send(new SetSelectedShooterMessage(selectedShooter.Shooter.ShooterId));
                });

            Initialize();
        }

        private void Initialize()
        {
            ShowCreatePersonCommand = new ViewModelCommand(x => MessengerInstance.Send(new CreatePersonDialogMessage()));
            ShowCreatePersonCommand.RaiseCanExecuteChanged();

            ShowEditPersonCommand = new ViewModelCommand(x => MessengerInstance.Send(new EditPersonDialogMessage(SelectedPerson)));
            ShowEditPersonCommand.AddGuard(x => SelectedPerson != null);
            ShowEditPersonCommand.RaiseCanExecuteChanged();

            DeletePersonCommand = new ViewModelCommand(x => MessengerInstance.Send(new DeletePersonDialogMessage(SelectedPerson)));
            DeletePersonCommand.AddGuard(x => SelectedPerson != null);
            DeletePersonCommand.RaiseCanExecuteChanged();

            CreateShooterCommand = new ViewModelCommand(x => CreateShooter(SelectedPerson));
            CreateShooterCommand.AddGuard(x => SelectedPerson != null);
            CreateShooterCommand.RaiseCanExecuteChanged();

            DeleteShooterCommand = new ViewModelCommand(x => MessengerInstance.Send(new DeleteShooterDialogMessage(SelectedShooter.Shooter.ShooterNumber)));
            DeleteShooterCommand.AddGuard(x => SelectedShooter != null);
            DeleteShooterCommand.RaiseCanExecuteChanged();

            ShowSelectParticipationCommand = new ViewModelCommand(x => { });
            ShowSelectParticipationCommand.RaiseCanExecuteChanged();

            PrintBarcodeCommand = new ViewModelCommand(x => PrintBarcode());
            PrintBarcodeCommand.AddGuard(x => SelectedShooter != null);
            PrintBarcodeCommand.RaiseCanExecuteChanged();
        }

        private void PrintBarcode()
        {
            IBarcodePrintService barcodeService = ServiceLocator.Current.GetInstance<IBarcodePrintService>();

            BarcodeFruehlingsschiessen barcode = new BarcodeFruehlingsschiessen();

            try
            {
                barcodeService.Print(barcode);
            }
            catch (Exception e)
            {
                MessengerInstance.Send(new DialogMessage("Barcode Print Error", "Fehler beim Drucken des Barcodes.\r\n\r\n" + e.ToString(), MessageIcon.Error));
            }
        }

        #region Commands

        public ViewModelCommand ShowCreatePersonCommand { get; private set; }

        public ViewModelCommand ShowEditPersonCommand { get; private set; }

        public ViewModelCommand DeletePersonCommand { get; private set; }

        public ViewModelCommand CreateShooterCommand { get; private set; }

        private void CreateShooter(Person person)
        {
            int shooterNumber = _shooterNumberService.GetShooterNumber();
            Shooter shooter = new Shooter
            {
                PersonId = person.PersonId,
                ShooterNumber = shooterNumber
            };

            _shooterDataWriter.WriteShooterData(new SsvShooterData
            {
                FirstName = person.FirstName,
                LastName = person.LastName,
                LicenseNumber = (uint) shooter.ShooterNumber
            });

            _shooterDataStore.Create(shooter);

            SetCurrentPerson(person);
            LoadShooters(person);
            SelectedShooter = Shooters.FirstOrDefault(s => s.Shooter.ShooterNumber == shooterNumber);
        }

        public ViewModelCommand PrintBarcodeCommand { get; private set; }

        public ViewModelCommand DeleteShooterCommand { get; private set; }

        public ViewModelCommand ShowSelectParticipationCommand { get; private set; }

        public ViewModelCommand DeleteParticipationCommand { get; private set; }

        #endregion

        private void LoadShooters(Person person)
        {
            if (person == null)
            {
                Shooters = new ObservableCollection<ShooterViewModel>();
                SelectedShooter = null;

            }
            else
            {
                Shooter[] shooters = _shooterDataStore.FindByPersonId(person.PersonId).ToArray();

                Shooters =
                    new ObservableCollection<ShooterViewModel>(
                        shooters.Select(shooter => new ShooterViewModel(shooter)));
                SelectedShooter = Shooters.FirstOrDefault();
            }
        }


        public void LoadPersons()
        {
            _allPersons =
                _personDataStore.GetAll().OrderBy(person => person.LastName).ThenBy(person => person.FirstName).ToList();
            FilterPersons();
        }

        private void FilterPersons()
        {
            FilteredPersons = new ObservableCollection<Person>(FilterPersons(_allPersons, PersonFilterText));
        }


        private void SetCurrentPerson(Person person)
        {
            SelectedPerson = person;
        }

        private IEnumerable<Person> FilterPersons(IEnumerable<Person> persons, string filterText)
        {
            if (filterText == null) filterText = string.Empty;
            string[] split = filterText.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

            foreach (Person person in persons)
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
                                person.LastName.IndexOf(x, StringComparison.InvariantCultureIgnoreCase) != -1))
                    {
                        yield return person;
                    }
                }
            }
        }

        #region Properties

        private ObservableCollection<ProgramItem> _programItems;

        public ObservableCollection<ProgramItem> ProgramItems
        {
            get { return _programItems; }
            set
            {
                if (value != _programItems)
                {
                    _programItems = value;
                    OnPropertyChanged("ProgramItems");
                }
            }
        }

        private string _personFilterText;

        public string PersonFilterText
        {
            get { return _personFilterText; }
            set
            {
                if (value != _personFilterText)
                {
                    _personFilterText = value;
                    OnPropertyChanged("PersonFilterText");
                    FilterPersons();
                }
            }
        }

        private ObservableCollection<Person> _filteredPersons;

        public ObservableCollection<Person> FilteredPersons
        {
            get { return _filteredPersons; }
            set
            {
                if (value != _filteredPersons)
                {
                    _filteredPersons = value;
                    OnPropertyChanged();
                }
            }
        }

        private Person _selectedPerson;

        public Person SelectedPerson
        {
            get { return _selectedPerson; }
            set
            {
                if (value != _selectedPerson)
                {
                    _selectedPerson = value;
                    OnPropertyChanged("SelectedPerson");

                    MessengerInstance.Send(new PersonSelectedMessage(_selectedPerson));
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


        private ObservableCollection<ShooterViewModel> _shooters;

        public ObservableCollection<ShooterViewModel> Shooters
        {
            get { return _shooters; }
            set
            {
                if (value != _shooters)
                {
                    _shooters = value;
                    OnPropertyChanged();
                }
            }
        }

        private ShooterViewModel _selectedShooter;

        public ShooterViewModel SelectedShooter
        {
            get { return _selectedShooter; }
            set
            {
                if (value != _selectedShooter)
                {
                    _selectedShooter = value;
                    OnPropertyChanged();

                    PrintBarcodeCommand.RaiseCanExecuteChanged();
                    DeleteShooterCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private ObservableCollection<GroupingViewModel> _groups;

        public ObservableCollection<GroupingViewModel> Groups
        {
            get { return _groups; }
            set
            {
                if (value != _groups)
                {
                    _groups = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        private void UpdateCommandCanExecuteOnSelectedPersonChanged()
        {
            DeletePersonCommand.RaiseCanExecuteChanged();
            ShowEditPersonCommand.RaiseCanExecuteChanged();
            CreateShooterCommand.RaiseCanExecuteChanged();
        }

        #region Fetch

        #endregion

    }

    public interface ILoadable
    {
        void Load();
    }
}