using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Gui.ViewModel;
using ShootingRange.BusinessObjects;
using ShootingRange.ConfigurationProvider;
using ShootingRange.Repository.RepositoryInterfaces;
using ShootingRange.Service.Interface;

namespace ShootingRange.ServiceDesk.ViewModel
{
  public class PersonsPageViewModel : INotifyPropertyChanged, ILoadable
  {
    private IPersonDataStore _personDataStore;
    private IShooterDataStore _shooterDataStore;
    private ICollectionShooterDataStore _collectionShooterDataStore;
    private IShooterCollectionDataStore _shooterCollectionDataStore;
    private IShooterCollectionParticipationDataStore _shooterCollectionParticipationDataStore;
    private IParticipationDataStore _pariticipationDataStore;
    private IShooterNumberService _shooterNumberService;

    private List<Person> _persons;

    public PersonsPageViewModel()
    {
      _personDataStore = ConfigurationSource.Configuration.GetPersonDataStore();
      _shooterDataStore = ConfigurationSource.Configuration.GetShooterDataStore();
      _shooterNumberService = ConfigurationSource.Configuration.GetShooterNumberService();
      _collectionShooterDataStore = ConfigurationSource.Configuration.GetCollectionShooterDataStore();
      _shooterCollectionDataStore = ConfigurationSource.Configuration.GetShooterCollectionDataStore();
      _shooterCollectionParticipationDataStore =
        ConfigurationSource.Configuration.GetShooterCollectionParticipationDataStore();
      _pariticipationDataStore = ConfigurationSource.Configuration.GetParticipationDataStore();

      Initialize();
      LoadPersons();
    }

    private void Initialize()
    {
      ShowCreatePersonCommand = new ViewModelCommand(x => ShowCreatePerson((IWindow)x));
      ShowCreatePersonCommand.RaiseCanExecuteChanged();

      ShowEditPersonCommand = new ViewModelCommand(x => ShowEditPerson((IWindow)x));
      ShowEditPersonCommand.AddGuard(x => SelectedPerson != null);
      ShowEditPersonCommand.RaiseCanExecuteChanged();

      DeletePersonCommand = new ViewModelCommand(x => DeletePerson((IWindow)x));
      DeletePersonCommand.AddGuard(x => SelectedPerson != null);
      DeletePersonCommand.RaiseCanExecuteChanged();

      CreateShooterCommand = new ViewModelCommand(x => CreateShooter(SelectedPerson));
      CreateShooterCommand.AddGuard(x => SelectedPerson != null);
      CreateShooterCommand.RaiseCanExecuteChanged();

      DeleteShooterCommand = new ViewModelCommand(x => DeleteShooter((IWindow)x));
      DeleteShooterCommand.AddGuard(x => SelectedShooter != null);
      DeleteShooterCommand.RaiseCanExecuteChanged();

      ShowSelectParticipationCommand = new ViewModelCommand(x => { });
      ShowSelectParticipationCommand.RaiseCanExecuteChanged();

      PrintBarcodeCommand = new ViewModelCommand(x => PrintBarcode((IWindow) x));
      PrintBarcodeCommand.AddGuard(x => SelectedShooter != null);
      PrintBarcodeCommand.RaiseCanExecuteChanged();
    }

    private void PrintBarcode(IWindow window)
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

        IEnumerable<string> gruppenstichGruppennamen = from sc in _shooterCollectionDataStore.GetAll()
          join cs in _collectionShooterDataStore.FindByShooterId(SelectedShooter.Shooter.ShooterId) on
            sc.ShooterCollectionId equals cs.ShooterCollectionId
          join scp in _shooterCollectionParticipationDataStore.GetAll() on cs.ShooterCollectionId equals
            scp.ShooterCollectionId
          join p in _pariticipationDataStore.GetAll() on scp.ParticipationId equals p.ParticipationId
          where p.ParticipationName == "Gruppenstich"
          select sc.CollectionName;

        string gruppenstichGruppenname = gruppenstichGruppennamen.SingleOrDefault(_ => true);

        IBarcodePrintService barcodeService = ConfigurationSource.Configuration.GetBarcodePrintService();
        IBarcodeBuilderService barcodeBuilderService = ConfigurationSource.Configuration.GetBarcodeBuilderService();

        BarcodeVolksschiessen barcode = new BarcodeVolksschiessen
        {
          FirstName = personShooter.FirstName,
          LastName = personShooter.LastName,
          DateOfBirth = personShooter.DateOfBirth,
          Barcode = barcodeBuilderService.BuildBarcode(personShooter.ShooterNumber, 0),
          Gruppenstich = gruppenstichGruppenname
        };

        try
        {
          barcodeService.Print(barcode);
        }
        catch (Exception e)
        {
          DialogHelper.ShowErrorDialog(window, "Barcode Print Error", "Fehler beim Drucken des Barcodes.\r\n\r\n" + e.Message);
        }
      }
    }

    #region Commands

    public ViewModelCommand ShowCreatePersonCommand { get; private set; }

    public void ShowCreatePerson(IWindow parent)
    {
      CreatePersonViewModel vm = new CreatePersonViewModel
      {
        Title = "Neuer Schütze"
      };
      bool? dialogResult = DialogHelper.ShowDialog(parent, vm, vm.Title);

      if (dialogResult.HasValue && dialogResult == true)
      {
        if (string.IsNullOrWhiteSpace(vm.Person.FirstName) || string.IsNullOrWhiteSpace(vm.Person.LastName))
        {
          ErrorInfoPersonEdit(parent);
          return;
        }

        _personDataStore.Create(vm.Person);
        SelectedPerson = vm.Person;
        LoadPersons();
        CreateShooter(SelectedPerson);
      }
    }

    public ViewModelCommand ShowEditPersonCommand { get; private set; }

    private void ShowEditPerson(IWindow parent)
    {
      CreatePersonViewModel vm = new CreatePersonViewModel
      {
        Person = new Person(SelectedPerson),
        Title = "Person editieren"
      };
      bool? dialogResult = DialogHelper.ShowDialog(parent, vm, vm.Title);

      if (dialogResult.HasValue && dialogResult == true)
      {
        if (string.IsNullOrWhiteSpace(vm.Person.FirstName) || string.IsNullOrWhiteSpace(vm.Person.LastName))
        {
          ErrorInfoPersonEdit(parent);
          return;
        }

        _personDataStore.Update(vm.Person);
        LoadPersons();
        SetCurrentPerson(vm.Person);
      }
    }

    private static void ErrorInfoPersonEdit(IWindow parent)
    {
      ErrorDialogViewModel errorVm = new ErrorDialogViewModel()
      {
        Caption = "Vorgang nicht möglich",
        MessageText = "Vorname und Nachname dürfen nicht leer sein."
      };

      ViewServiceLocator.ViewService.ExecuteAction(parent, errorVm);
    }

    public ViewModelCommand DeletePersonCommand { get; private set; }

    private void DeletePerson(IWindow window)
    {
      YesNoMessageBoxViewModel vm = new YesNoMessageBoxViewModel
      {
        DefaultYes = false,
        Caption = "Person löschen?",
        MessageBoxText =
          string.Format("Möchtest du die Person '{0} {1}' wirklich löschen?",
            SelectedPerson.FirstName,
            SelectedPerson.LastName)
      };

      bool? result = ViewServiceLocator.ViewService.ExecuteFunction<YesNoMessageBoxViewModel, bool?>(window, vm);
      if (result == true)
      {
        _personDataStore.Delete(SelectedPerson);
        LoadPersons();
      }
    }

    public ViewModelCommand CreateShooterCommand { get; private set; }

    private void CreateShooter(Person person)
    {
      int shooterNumber = _shooterNumberService.GetShooterNumber();
      Shooter shooter = new Shooter
      {
        PersonId = person.PersonId,
        ShooterNumber = shooterNumber
      };

      ISsvShooterDataWriterService ssvShooterDataWriterService =
        ConfigurationSource.Configuration.GetSsvShooterDataWriterService();
      ssvShooterDataWriterService.WriteShooterData(new SsvShooterData
      {
        FirstName = person.FirstName,
        LastName = person.LastName,
        LicenseNumber = (uint)shooter.ShooterNumber
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

    private void DeleteShooter(IWindow window)
    {
      YesNoMessageBoxViewModel vm = new YesNoMessageBoxViewModel
      {
        DefaultYes = false,
        Caption = "Person löschen?",
        MessageBoxText =
          string.Format("Möchtest du den Schützen '{0}' wirklich löschen?",
            SelectedShooter.Shooter.ShooterNumber)
      };

      bool? result = ViewServiceLocator.ViewService.ExecuteFunction<YesNoMessageBoxViewModel, bool?>(window, vm);

      if (result == true)
      {
        _shooterDataStore.Delete(SelectedShooter.Shooter);
        LoadShooters(SelectedPerson);
      }
    }

    #endregion


    private void LoadShooters(Person person)
    {
      Shooter[] shooters = _shooterDataStore.FindByPersonId(person.PersonId).ToArray();

      Shooters =
        new ObservableCollection<ShooterViewModel>(
          shooters.Select(shooter => new ShooterViewModel(shooter)));
      SelectedShooter = Shooters.FirstOrDefault();
    }

    private void ClearShooters()
    {
      Shooters = new ObservableCollection<ShooterViewModel>();
      SelectedShooter = null;
    }

    public void Load()
    {
      LoadPersons();
    }

    public void LoadPersons()
    {
      _persons = _personDataStore.GetAll().OrderBy(person => person.LastName).ThenBy(person => person.FirstName).ToList();
      UpdateFilteredPersons();
      SelectedPersonChanged();
    }


    private void SetCurrentPerson(Person person)
    {
      SelectedPerson = person;
    }

    private void UpdateFilteredPersons()
    {
      FilteredPersons = new ObservableCollection<Person>(FilterPersons(_persons, PersonFilterText));
    }

    private IEnumerable<Person> FilterPersons(IEnumerable<Person> persons, string filterText)
    {
      if (filterText == null) filterText = string.Empty;
      string[] split = filterText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
 
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
          UpdateFilteredPersons();
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
          OnPropertyChanged("FilteredPersons");
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

          SelectedPersonChanged();
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



    private ObservableCollection<ShooterViewModel> _shooters;
    public ObservableCollection<ShooterViewModel> Shooters
    {
      get { return _shooters; }
      set
      {
        if (value != _shooters)
        {
          _shooters = value;
          OnPropertyChanged("Shooters");
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
          OnPropertyChanged("SelectedShooter");

          SelectedShooterChanged();
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
          OnPropertyChanged("Groups");
        }
      }
    }
    #endregion

    private void SelectedShooterChanged()
    {
      PrintBarcodeCommand.RaiseCanExecuteChanged();
      DeleteShooterCommand.RaiseCanExecuteChanged();
    }

    private void SelectedPersonChanged()
    {
      DeletePersonCommand.RaiseCanExecuteChanged();
      ShowEditPersonCommand.RaiseCanExecuteChanged();
      CreateShooterCommand.RaiseCanExecuteChanged();

      if (SelectedPerson != null)
      {
        LoadShooters(SelectedPerson);
      }
      else
      {
        ClearShooters();
      }
    }

    #region Fetch

    #endregion

    #region INotifyPropertyChanged Members

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      var handler = PropertyChanged;
      if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
  }

  public interface ILoadable
  {
    void Load();
  }
}