using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Gui.ViewModel;
using Microsoft.Practices.ServiceLocation;
using ShootingRange.BusinessObjects;
using ShootingRange.ConfigurationProvider;
using ShootingRange.Repository.RepositoryInterfaces;
using ShootingRange.Service.Interface;
using ShootingRange.ServiceDesk.ViewModel.MessageTypes;

namespace ShootingRange.ServiceDesk.ViewModel
{
    public class PersonsPageViewModel : Gui.ViewModel.ViewModel
    {
        private IPersonDataStore _personDataStore;
        private IShooterDataStore _shooterDataStore;
        private IShooterNumberService _shooterNumberService;
        private ISsvShooterDataWriterService _shooterDataWriter;
        private ICollectionShooterDataStore _collectionShooterDataStore;
        private IShooterCollectionDataStore _shooterCollectionDataStore;
        private ServiceDeskConfiguration _serviceDeskConfiguration;

        public PersonsPageViewModel()
        {
            ShowCreatePersonCommand = new ViewModelCommand(x => MessengerInstance.Send(new CreatePersonDialogMessage()));
            ShowCreatePersonCommand.RaiseCanExecuteChanged();

            ShowEditPersonCommand =
                new ViewModelCommand(x => MessengerInstance.Send(new EditPersonDialogMessage(SelectedPerson)));
            ShowEditPersonCommand.AddGuard(x => SelectedPerson != null);
            ShowEditPersonCommand.RaiseCanExecuteChanged();

            DeletePersonCommand =
                new ViewModelCommand(x => MessengerInstance.Send(new DeletePersonDialogMessage(SelectedPerson)));
            DeletePersonCommand.AddGuard(x => SelectedPerson != null);
            DeletePersonCommand.RaiseCanExecuteChanged();

            CreateShooterCommand = new ViewModelCommand(x => CreateShooter(SelectedPerson));
            CreateShooterCommand.AddGuard(x => SelectedPerson != null);
            CreateShooterCommand.RaiseCanExecuteChanged();

            DeleteShooterCommand =
                new ViewModelCommand(
                    x => MessengerInstance.Send(new DeleteShooterDialogMessage(SelectedShooter.Shooter.ShooterNumber)));
            DeleteShooterCommand.AddGuard(x => SelectedShooter != null);
            DeleteShooterCommand.RaiseCanExecuteChanged();

            ShowSelectParticipationCommand = new ViewModelCommand(x => { });
            ShowSelectParticipationCommand.RaiseCanExecuteChanged();

            PrintBarcodeCommand = new ViewModelCommand(x => PrintBarcode());
            PrintBarcodeCommand.AddGuard(x => SelectedShooter != null);
            PrintBarcodeCommand.RaiseCanExecuteChanged();
        }

        public List<UiPerson> AllPersons { get; set; }

        public void Initialize()
        {
            _personDataStore = ServiceLocator.Current.GetInstance<IPersonDataStore>();
            _shooterDataStore = ServiceLocator.Current.GetInstance<IShooterDataStore>();
            _shooterNumberService = ServiceLocator.Current.GetInstance<IShooterNumberService>();
            _shooterDataWriter = ServiceLocator.Current.GetInstance<ISsvShooterDataWriterService>();
            _collectionShooterDataStore = ServiceLocator.Current.GetInstance<ICollectionShooterDataStore>();
            _shooterCollectionDataStore = ServiceLocator.Current.GetInstance<IShooterCollectionDataStore>();
            _serviceDeskConfiguration = ServiceLocator.Current.GetInstance<ServiceDeskConfiguration>();

            MessengerInstance.Register<PersonSelectedMessage>(this,
                x =>
                {
                    UpdateCommandCanExecuteOnSelectedPersonChanged();
                    LoadShooters(x.PersonId);
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

            MessengerInstance.Register<RefreshDataFromRepositoriesMessage>(this,
                x =>
                {
                    UiPerson selectedPerson = SelectedPerson;
                    ShooterViewModel selectedShooter = SelectedShooter;
                    LoadPersons();

                    if (selectedPerson != null)
                        MessengerInstance.Send(new SetSelectedPersonMessage(selectedPerson.PersonId));

                    if (selectedShooter != null)
                        MessengerInstance.Send(new SetSelectedShooterMessage(selectedShooter.Shooter.ShooterId));
                });
        }

        private void PrintBarcode()
        {
            if (SelectedShooter != null)
            {
                var personShooter = (from shooter in _shooterDataStore.GetAll()
                                     join person in _personDataStore.GetAll() on shooter.PersonId equals person.PersonId
                                     where shooter.ShooterId == SelectedShooter.Shooter.ShooterId
                                     select new
                                     {
                                         person.FirstName,
                                         person.LastName,
                                         person.DateOfBirth,
                                         shooter.ShooterNumber
                                     }).Single();

                IBarcodeBuilderService barcodeBuilderService = ServiceLocator.Current.GetInstance<IBarcodeBuilderService>();
                string barcode = barcodeBuilderService.BuildBarcode(personShooter.ShooterNumber, 0);


                var shooterCollections = from sc in _shooterCollectionDataStore.GetAll()
                    join cs in _collectionShooterDataStore.GetAll() on
                        sc.ShooterCollectionId equals cs.ShooterCollectionId
                    join p in _serviceDeskConfiguration.ParticipationDescriptions.GetAll() on
                        sc.ProgramNumber.ToString() equals p.ProgramNumber
                    where p.AllowShooterCollectionParticipation && cs.ShooterId == SelectedShooter.Shooter.ShooterId
                    select new
                    {
                        sc.CollectionName,
                        p.ProgramName,
                        p.ProgramNumber
                    };

                Dictionary<string, Tuple<string, string>> grouped = (from sc in shooterCollections
                    group sc by sc.ProgramNumber
                    into g
                    select new
                    {
                        ProgramNumber = g.Key,
                        CollectionName = g.Single().CollectionName,
                        ProgramName = g.Single().ProgramName
                    }).ToDictionary(x => x.ProgramNumber,
                        x => new Tuple<string, string>(x.ProgramName, x.CollectionName));

                IBarcodePrintService barcodeService = ServiceLocator.Current.GetInstance<IBarcodePrintService>();

                GenericBarcode_20150909 genericBarcode = new GenericBarcode_20150909
                {
                    FirstName = personShooter.FirstName,
                    LastName = personShooter.LastName,
                    DateOfBirth = personShooter.DateOfBirth,
                    Barcode = barcode,
                    ParticipationTypeToCollectionName = grouped.Values.Take(2).ToList(),
                    Participations = _serviceDeskConfiguration.ParticipationDescriptions.GetAll().Select(x => x.ProgramName).Take(5).ToList()
                };

                try
                {
                    barcodeService.Print(genericBarcode);
                }
                catch (Exception e)
                {
                    MessengerInstance.Send(new DialogMessage("Barcode Print Error",
                        "Fehler beim Drucken des Barcodes.\r\n\r\n" + e.ToString(),
                        MessageIcon.Error));
                }
            }
        }

        #region Commands

        public ViewModelCommand ShowCreatePersonCommand { get; private set; }

        public ViewModelCommand ShowEditPersonCommand { get; private set; }

        public ViewModelCommand DeletePersonCommand { get; private set; }

        public ViewModelCommand CreateShooterCommand { get; private set; }

        private void CreateShooter(UiPerson person)
        {
            if (person != null)
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

                MessengerInstance.Send(new RefreshDataFromRepositoriesMessage());
                MessengerInstance.Send(new SetSelectedPersonMessage(person.PersonId));
                MessengerInstance.Send(new SetSelectedShooterMessage(shooter.ShooterId));
            }
        }

        public ViewModelCommand PrintBarcodeCommand { get; private set; }

        public ViewModelCommand DeleteShooterCommand { get; private set; }

        public ViewModelCommand ShowSelectParticipationCommand { get; private set; }

        public ViewModelCommand DeleteParticipationCommand { get; private set; }

        #endregion

        private void LoadShooters(int personId)
        {
            Shooter[] shooters = _shooterDataStore.FindByPersonId(personId).ToArray();

            Shooters =
                new ObservableCollection<ShooterViewModel>(
                    shooters.Select(shooter =>
                    {
                        ShooterViewModel vm = new ShooterViewModel();
                        vm.Initialize(shooter);
                        return vm;
                    }));
            SelectedShooter = Shooters.FirstOrDefault();
        }


        public void LoadPersons()
        {
            UiPerson selectedPerson = SelectedPerson;
            AllPersons = (from p in _personDataStore.GetAll()
                join s in _shooterDataStore.GetAll() on p.PersonId equals s.PersonId into gj
                orderby p.FirstName
                orderby p.LastName
                from subs in gj.DefaultIfEmpty()
                group gj by p
                into grouping
                select new UiPerson
                {
                    PersonId = grouping.Key.PersonId,
                    FirstName = grouping.Key.FirstName,
                    LastName = grouping.Key.LastName,
                    DateOfBirth = grouping.Key.DateOfBirth,
                    HasShooters = grouping.SelectMany(x => x).Any()
                }).ToList();

            FilterPersons();

            if (selectedPerson != null)
                SelectedPerson = FilteredPersons.FirstOrDefault(x => selectedPerson.PersonId == x.PersonId);
        }

        [DebuggerDisplay("{Person.FirstName} {Person.LastName}")]
        internal class Foo
        {
            public Person Person { get; set; }
            public IEnumerable<Shooter> Shooters { get; set; }
        }

        private void FilterPersons()
        {
            FilteredPersons = new ObservableCollection<UiPerson>(FilterPersons(AllPersons, PersonFilterText));
        }

        private IEnumerable<UiPerson> FilterPersons(IEnumerable<UiPerson> persons, string filterText)
        {
            if (filterText == null) filterText = string.Empty;
            string[] split = filterText.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

            foreach (UiPerson person in persons)
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

        private string _personFilterText;

        public string PersonFilterText
        {
            get { return _personFilterText; }
            set
            {
                if (value != _personFilterText)
                {
                    _personFilterText = value;
                    OnPropertyChanged();
                    FilterPersons();
                }
            }
        }

        private ObservableCollection<UiPerson> _filteredPersons;

        public ObservableCollection<UiPerson> FilteredPersons
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

        private UiPerson _selectedPerson;

        public UiPerson SelectedPerson
        {
            get { return _selectedPerson; }
            set
            {
                if (value != _selectedPerson)
                {
                    _selectedPerson = value;
                    OnPropertyChanged();

                    if (MessengerInstance != null && _selectedPerson != null)
                        MessengerInstance.Send(new PersonSelectedMessage(_selectedPerson.PersonId));
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

        #endregion

        private void UpdateCommandCanExecuteOnSelectedPersonChanged()
        {
            DeletePersonCommand.RaiseCanExecuteChanged();
            ShowEditPersonCommand.RaiseCanExecuteChanged();
            CreateShooterCommand.RaiseCanExecuteChanged();
        }

    }}