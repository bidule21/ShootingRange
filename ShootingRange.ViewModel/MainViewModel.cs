using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using DotNetToolbox.RelayCommand;
using ShootingRange.BusinessObjects;
using ShootingRange.BusinessObjects.Properties;
using ShootingRange.Common;
using ShootingRange.Common.Modules;
using ShootingRange.ConfigurationProvider;
using ShootingRange.Repository.FakeRepositories;
using ShootingRange.Repository.Repositories;
using ShootingRange.Repository.RepositoryInterfaces;
using ShootingRange.Service.Interface;
using ShootingRange.UiBusinessObjects;

namespace ShootingRange.ViewModel
{
  public class MainViewModel : INotifyPropertyChanged
  {
    private IPersonDataStore _personDataStore;
    private IShooterDataStore _shooterDataStore;
    private IParticipationDataStore _participationDataStore;
    private IGroupMemberDetailsView _groupMemberDetailsView;
    private IGroupDetailsView _groupDetailsView;
    private IShooterParticipationDataStore _shooterParticipationDataStore;
    private IShooterParticipationView _shooterParticipationView;
    private ISessionDetailsView _sessionDetailsView;
    private ISsvShooterDataWriterService _shooterDataWriterService;
    private ICollectionShooterDataStore _collectionShooterDataStore;
    private IShooterCollectionDataStore _shooterCollectionDataStore;
    private IShooterCollectionParticipationDataStore _shooterCollectionParticipationDataStore;
    private ShootingRangeEvents _events;

    private IWindowService _windowService;
    private IBarcodePrintService _barcodePrintService;
    private IShooterNumberService _shooterNumberService;
    private IBarcodeBuilderService _barcodeBuilderService;
    private UIEvents _uiEvents;

    public MainViewModel()
    {
      if (DesignTimeHelper.IsInDesignMode)
      {
        _personDataStore = new FakePersonDataStore();
        _shooterDataStore = new FakeShooterDataStore();
        _participationDataStore = new FakeParticipationDataStore();
        _groupMemberDetailsView = new FakeGroupMemberDetailsView();
        _groupDetailsView = new FakeGroupDetailsView();
      }
      else
      {
        IConfiguration config = ConfigurationSource.Configuration;
        _personDataStore = config.GetPersonDataStore();
        _shooterDataStore = config.GetShooterDataStore();
        _participationDataStore = config.GetParticipationDataStore();
        _shooterParticipationDataStore = config.GetShooterParticipationDataStore();
        _groupMemberDetailsView = config.GetGroupMemberDetailsView();
        _shooterParticipationView = config.GetShooterParticipationView();
        _groupDetailsView = config.GetGroupDetailsView();
        _sessionDetailsView = config.GetSessionDetailsView();
        _shooterDataWriterService = config.GetSsvShooterDataWriterService();
        _collectionShooterDataStore = config.GetCollectionShooterDataStore();
        _shooterCollectionDataStore = config.GetShooterCollectionDataStore();
        _shooterCollectionParticipationDataStore = config.GetShooterCollectionParticipationDataStore();
        _shooterNumberService = config.GetShooterNumberService();
        _windowService = config.GetWindowService();
        _barcodePrintService = config.GetBarcodePrintService();
        _barcodeBuilderService = config.GetBarcodeBuilderService();
        _events = config.GetEvents();
        _uiEvents = config.GetUIEvents();
        _uiEvents.RequireSelectedPerson += () => _uiEvents.PersonSelected(SelectedUiPerson);
        _uiEvents.SelectPersonById += (id) => { SelectedUiPerson = UiPeople.FirstOrDefault(_ => _.PersonId == id); };
        _uiEvents.RequireSelectedShooter += () => _uiEvents.ShooterSelected(SelectedUiShooter);
        _uiEvents.FetchSelectedShooter += () => SelectedUiShooter;
        _uiEvents.PersonDataStoreChanged += LoadPersonList;
        _uiEvents.ShooterDataStoreChanged += LoadShooterList;
        _shooterNumberService.Configure(_shooterDataStore);

        LoadPersonList();
        LoadShooterList();
        LoadParticipationList();
      }

      CreatePersonCommand = new RelayCommand<object>(ExecuteCreatePersonCommand);
      EditPersonCommand = new RelayCommand<UiPerson>(ExecuteEditPersonCommand, CanExecuteEditPersonCommand);
      DeletePersonCommand = new RelayCommand<UiPerson>(ExecuteDeletePersonCommand, CanExecuteDeletePersonCommand);
      
      CreateShooterCommand = new RelayCommand<UiPerson>(ExecuteCreateShooterCommand, CanExecuteCreateShooterCommand);
      EditShooterCommand = new RelayCommand<UiShooter>(ExecuteEditShooterCommand, CanExecuteEditShooterCommand);
      DeleteShooterCommand = new RelayCommand<UiShooter>(ExecuteDeleteShooterCommand, CanExecuteDeleteShooterCommand);

      CreateParticipationCommand = new RelayCommand<object>(ExecuteCreateParticipationCommand);
      //EditParticipationCommand = new RelayCommand<UiParticipation>
      //DeleteParticipationCommand = new RelayCommand<UiParticipation>

      PrintBarcodeCommand = new RelayCommand<UiShooter>(ExecutePrintBarcodeCommand, CanExecutePrintBarcodeCommand);
      EditPassCommand = new RelayCommand<SessionDetails>(ExecuteEditPassCommand);
    }

    #region Commands

    #region Person

    private void ExecuteCreatePersonCommand(object obj)
    {
      try
      {
        _windowService.ShowCreatePersonWindow();
      }
      catch (Exception e)
      {
        ReportException(e);
      }
      finally
      {
        LoadShooterList();
      }
    }

    private bool CanExecuteEditPersonCommand(UiPerson uiPerson)
    {
      return uiPerson != null;
    }

    private void ExecuteEditPersonCommand(UiPerson uiPerson)
    {
      try
      {
        _windowService.ShowEditPersonWindow();
      }
      catch (Exception e)
      {
        ReportException(e);
      }
    }

    private void ExecuteEditPassCommand(SessionDetails obj)
    {
      try
      {
        _windowService.ShowEditPassWindow();
      }
      catch (Exception e)
      {
        ReportException(e);
      }
    }

    private bool CanExecuteDeletePersonCommand(UiPerson uiPerson)
    {
      return uiPerson != null;
    }

    private void ExecuteDeletePersonCommand(UiPerson uiPerson)
    {
      try
      {
        bool yes = _windowService.ShowYesNoMessasge("Person löschen",
          string.Format("Möchtest du die Person '{0} {1}' wirklich löschen?", uiPerson.LastName, uiPerson.FirstName));

        if (yes)
        {
          _personDataStore.Delete(uiPerson.ToPerson());
        }
      }
      catch (Exception e)
      {
        ReportException(e);
        _personDataStore.Revert();
      }
      finally
      {
        _uiEvents.PersonDataStoreChanged();
      }
    }

    #endregion

    #region Shooter

    private bool CanExecuteCreateShooterCommand(UiPerson uiPerson)
    {
      return uiPerson != null;
    }

    private void ExecuteCreateShooterCommand(UiPerson uiPerson)
    {
      try
      {
        Shooter shooter = new Shooter();
        shooter.ShooterNumber = _shooterNumberService.GetShooterNumber();
        shooter.PersonId = uiPerson.PersonId;
        _shooterDataStore.Create(shooter);
        _shooterDataWriterService.WriteShooterData(new SsvShooterData
        {
          FirstName = uiPerson.FirstName,
          LastName = uiPerson.LastName,
          LicenseNumber = (uint)shooter.ShooterNumber
        });
        _windowService.ShowMessage("Schütze erstellt", string.Format("Schütze mit Schützennummer '{0}' erfolgreich erstellt.", shooter.ShooterNumber));
      }
      catch (Exception e)
      {
        ReportException(e);
        _shooterDataStore.Revert();
      }
      finally
      {
        _uiEvents.ShooterDataStoreChanged();
      }

      //try
      //{
      //  _windowService.ShowCreateShooterWindow();
      //}
      //catch (Exception e)
      //{
      //  ReportException(e);
      //}
    }

    private bool CanExecuteEditShooterCommand(UiShooter uiShooter)
    {
      return uiShooter != null;
    }

    private void ExecuteEditShooterCommand(UiShooter uiShooter)
    {
      try
      {
        _windowService.ShowEditShooterWindow();
        _uiEvents.ShooterDataStoreChanged();
      }
      catch (Exception e)
      { 
        ReportException(e);
      }
    }

    private bool CanExecuteDeleteShooterCommand(UiShooter uiShooter)
    {
      return uiShooter != null;
    }

    private void ExecuteDeleteShooterCommand(UiShooter uiShooter)
    {
      try
      {
        bool yes = _windowService.ShowYesNoMessasge("Schütze löschen", string.Format("Möchtest du den Schützen mit der Nummer '{0}' wirklich löschen?", uiShooter.ShooterNumber));
        if (yes)
        {
          _shooterDataStore.Delete(uiShooter.ToShooter());
        }
      }
      catch (Exception e)
      {
        ReportException(e);
        _shooterDataStore.Revert();
      }
      finally
      {
        _uiEvents.ShooterDataStoreChanged();
      }
    }

    private void ReportException(Exception e)
    {
      _windowService.ShowErrorMessage("Error", e.ToString());
    }

    #endregion

    #region Participation

    private void ExecuteCreateParticipationCommand(object obj)
    {
      _windowService.ShowCreateParticipationWindow();
    }

    #endregion

    #region Barcode

    private bool CanExecutePrintBarcodeCommand(UiShooter uiShooter)
    {
      return uiShooter != null;
    }

    private void ExecutePrintBarcodeCommand(UiShooter uiShooter)
    {
      try
      {
        bool isNachwuchs = (from sp in _shooterParticipationDataStore.GetAll()
          join p in _participationDataStore.GetAll() on sp.ParticipationId equals p.ParticipationId
          where p.ParticipationName == "Nachwuchsstich" && sp.ShooterId == uiShooter.ShooterId
          select p.ParticipationId).
          Any();

        bool isGruppe = (from sp in _shooterParticipationDataStore.GetAll()
                            join p in _participationDataStore.GetAll() on sp.ParticipationId equals p.ParticipationId
                         where p.ParticipationName == "Gruppenstich" && sp.ShooterId == uiShooter.ShooterId
                            select p.ParticipationId).
  Any();

        bool isSieUndEr = (from sp in _shooterParticipationDataStore.GetAll()
                            join p in _participationDataStore.GetAll() on sp.ParticipationId equals p.ParticipationId
                           where p.ParticipationName == "Sie & Er" && sp.ShooterId == uiShooter.ShooterId
                            select p.ParticipationId).
  Any();

        bool isWorschtUndBrot = (from sp in _shooterParticipationDataStore.GetAll()
                            join p in _participationDataStore.GetAll() on sp.ParticipationId equals p.ParticipationId
                                 where p.ParticipationName == "Worscht & Brot" && sp.ShooterId == uiShooter.ShooterId
                            select p.ParticipationId).
  Any();

        string groupName = (from cs in _collectionShooterDataStore.GetAll()
          join sc in _shooterCollectionDataStore.GetAll() on cs.ShooterCollectionId equals sc.ShooterCollectionId
          join scp in _shooterCollectionParticipationDataStore.GetAll() on cs.ShooterCollectionId equals
            scp.ShooterCollectionId
          join p in _participationDataStore.GetAll() on scp.ParticipationId equals p.ParticipationId
          where p.ParticipationName == "Gruppenstich" && cs.ShooterId == uiShooter.ShooterId
          select sc.CollectionName).SingleOrDefault();

        string sieUndErName = (from cs in _collectionShooterDataStore.GetAll()
                            join sc in _shooterCollectionDataStore.GetAll() on cs.ShooterCollectionId equals sc.ShooterCollectionId
                            join scp in _shooterCollectionParticipationDataStore.GetAll() on cs.ShooterCollectionId equals
                              scp.ShooterCollectionId
                            join p in _participationDataStore.GetAll() on scp.ParticipationId equals p.ParticipationId
                            where p.ParticipationName == "Sie & Er" && cs.ShooterId == uiShooter.ShooterId
                            select sc.CollectionName).SingleOrDefault();

        Person person = uiShooter.PersonId == null
          ? new Person() {FirstName = "unknown", LastName = "unknown"}
          : _personDataStore.FindById((int) uiShooter.PersonId);
        BarcodeInfo barcodeInfo = new BarcodeInfo
        {
          FirstName = person.FirstName,
          LastName = person.LastName,
          DateOfBirth = person.DateOfBirth,
          Gruppenstich = groupName ?? string.Empty,
          SieUndEr = sieUndErName ?? string.Empty,
          Barcode = _barcodeBuilderService.BuildBarcode(uiShooter.ShooterNumber, uiShooter.Legalization),
          IsGruppenstich = isGruppe,
          IsNachwuchsstich = isNachwuchs,
          IsWorschtUndBrot = isWorschtUndBrot,
          IsSieUndEr = isSieUndEr
        };

        _barcodePrintService.Print(barcodeInfo);
      }
      catch (Exception e)
      {
        ReportException(e);
      }
    }

    #endregion

    #endregion

    #region Command Properties

    public ICommand CreatePersonCommand { get; private set; }
    public ICommand EditPersonCommand { get; private set; }
    public ICommand DeletePersonCommand { get; private set; }

    public ICommand CreateShooterCommand { get; private set; }
    public ICommand EditShooterCommand { get; private set; }
    public ICommand DeleteShooterCommand { get; private set; }

    public ICommand CreateParticipationCommand { get; private set; }
    public ICommand EditParticipationCommand { get; private set; }
    public ICommand DeleteParticipationCommand { get; private set; }

    public ICommand PrintBarcodeCommand { get; private set; }
    public ICommand EditPassCommand { get; private set; }

    #endregion

    #region Properties


    private string _logMessage;
    public string LogMessage
    {
      get { return _logMessage; }
      set
      {
        if (value != _logMessage)
        {
          _logMessage = value;
          OnPropertyChanged("LogMessage");
        }
      }
    }

    private string _detailsView;
    public string DetailsView
    {
      get { return _detailsView; }
      set
      {
        if (value != _detailsView)
        {
          _detailsView = value;
          OnPropertyChanged("DetailsView");
        }
      }
    }

    private UiShooter _selectedUiShooter;
    public UiShooter SelectedUiShooter
    {
      get { return _selectedUiShooter; }
      set
      {
        if (value != _selectedUiShooter)
        {
          _selectedUiShooter = value;
          OnPropertyChanged("SelectedUiShooter");
          OnSelectedShooterItemChanged(_selectedUiShooter);
        }
      }
    }

    private ObservableCollection<UiShooter> _shooterListItems;
    public ObservableCollection<UiShooter> ShooterListItems
    {
      get { return _shooterListItems; }
      set
      {
        if (value != _shooterListItems)
        {
          _shooterListItems = value;
          OnPropertyChanged("ShooterListItems");
        }
      }
    }

    private UiPerson _selectedUiPerson;
    public UiPerson SelectedUiPerson
    {
      get { return _selectedUiPerson; }
      set
      {
        if (value != _selectedUiPerson)
        {
          _selectedUiPerson = value;
          OnPropertyChanged("SelectedUiPerson");

          OnSelectedPersonItemChanged(SelectedUiPerson);
        }
      }
    }

    private ObservableCollection<UiPerson> _uiPeople;

    public ObservableCollection<UiPerson> UiPeople
    {
      get { return _uiPeople; }
      set
      {
        if (value != _uiPeople)
        {
          _uiPeople = value;
          OnPropertyChanged("UiPeople");
        }
      }
    }

    private ShooterParticipationListItem _selectedUiShooterParticipation;

    public ShooterParticipationListItem SelectedUiShooterParticipation
    {
      get { return _selectedUiShooterParticipation; }
      set
      {
        if (value != _selectedUiShooterParticipation)
        {
          _selectedUiShooterParticipation = value;
          OnPropertyChanged("SelectedUiShooterParticipation");
          OnSelectedShooterParticipationItemChanged(SelectedUiShooterParticipation);
        }
      }
    }

    private ObservableCollection<ShooterParticipationListItem> _participations;

    public ObservableCollection<ShooterParticipationListItem> Participations
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

    //private ObservableCollection<ParticipationTreeItem> _participationTreeItems;
    //public ObservableCollection<ParticipationTreeItem> ParticipationTreeItems
    //{
    //  get { return _participationTreeItems; }
    //  set
    //  {
    //    if (value != _participationTreeItems)
    //    {
    //      _participationTreeItems = value;
    //      OnPropertyChanged("ParticipationTreeItems");
    //    }
    //  }
    //}

    private ObservableCollection<SessionTreeViewItem> _sessionTreeViewItems;

    public ObservableCollection<SessionTreeViewItem> SessionTreeViewItems
    {
      get { return _sessionTreeViewItems; }
      set
      {
        if (value != _sessionTreeViewItems)
        {
          _sessionTreeViewItems = value;
          OnPropertyChanged("SessionTreeViewItems");
        }
      }
    } 

    #endregion

    #region Property changed handler

    private void OnSelectedShooterItemChanged(UiShooter selectedUiShooter)
    {
      LoadParticipationList();
      if (selectedUiShooter != null)
      {
        StringBuilder detailsStringBuilder = new StringBuilder();

        Person person = selectedUiShooter.PersonId == null
          ? new Person() {FirstName = "unknown", LastName = "unknown"}
          : _personDataStore.FindById((int) selectedUiShooter.PersonId);
        detailsStringBuilder.AppendFormat("{0}, {1} [{2}] : {3} [{4}]\r\n{5}\r\n", person.LastName, person.FirstName,
          person.PersonId, selectedUiShooter.ShooterNumber, selectedUiShooter.ShooterId, person.DateOfBirth != null ? ((DateTime)person.DateOfBirth).ToString("dd.MM.yyyy") : string.Empty);
        detailsStringBuilder.AppendLine();
        IEnumerable<SessionDetails> sessionDetails = _sessionDetailsView.FindByShooterId(selectedUiShooter.ShooterId).OrderBy(_ => _.ProgramNumber).ToList();

        foreach (SessionDetails sessionDetail in sessionDetails)
        {
          detailsStringBuilder.AppendFormat("{0} [{1}] = {2}\r\n",
            sessionDetail.SessionDescription,
            sessionDetail.SessionId,
            sessionDetail.SubSessions.Sum(sd => sd.Shots.Sum(_ => _.PrimaryScore)));
        }

        DetailsView = detailsStringBuilder.ToString();

        SessionTreeViewItems =
          new ObservableCollection<SessionTreeViewItem>(sessionDetails.Select(sd => new SessionTreeViewItem
          {
            SessionHeader =
              sd.SessionDescription + " (" + sd.SubSessions.Sum(_ => _.Shots.Count()) + "): " +
              sd.SubSessions.Sum(_ => _.Shots.Sum(shot => shot.PrimaryScore)),
            Subsessions =
              sd.SubSessions.Select(
                ss =>
                  new Subsession
                  {
                    Shots =
                      ss.Shots.OrderBy(shot => shot.Ordinal)
                        .Select(shot => string.Format("{0}\t{1}", shot.Ordinal, shot.PrimaryScore)),
                    SubsessionHeader = string.Format("Gruppe {0} | T={1}", ss.Ordinal, ss.Shots.Sum(_ => _.PrimaryScore))
                  })
          }));
      }
    }

    private void OnSelectedPersonItemChanged(UiPerson selectedUiPerson)
    {
      LoadShooterList();
      LoadParticipationList();
      SelectFirstShooter();
    }

    private void OnSelectedShooterParticipationItemChanged(ShooterParticipationListItem selectedUiShooterParticipation)
    {
      if (selectedUiShooterParticipation != null)
      {
        ShooterParticipation shooterParticipation = _shooterParticipationDataStore.FindById(selectedUiShooterParticipation.ShooterParticipationId);
        IEnumerable<ShooterParticipation> shooterParticipations = _shooterParticipationDataStore.FindByParticipationId(shooterParticipation.ParticipationId).ToList();

        StringBuilder detailsStringBuilder = new StringBuilder();
        detailsStringBuilder.AppendFormat("{0}\r\n", selectedUiShooterParticipation.ParticipationName);
        foreach (ShooterParticipation participation in shooterParticipations)
        {
          Shooter shooter = _shooterDataStore.FindById(participation.ShooterId);
          ShooterParticipation innerShooterParticipation = _shooterParticipationDataStore.FindByShooterId(shooter.ShooterId).First();
          Person person = shooter.PersonId == null
            ? new Person() {FirstName = "unknown", LastName = "unknown"}
            : _personDataStore.FindById((int) shooter.PersonId);
          detailsStringBuilder.AppendFormat("\t{0}, {1} [{2}]\r\n", person.LastName, person.FirstName, innerShooterParticipation.ShooterParticipationId);
        }

        DetailsView = detailsStringBuilder.ToString();
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion

    #region Loader

    private void LoadPersonList()
    {
      try
      {
        UiPeople =
          new ObservableCollection<UiPerson>(
            _personDataStore.GetAll().Select(UiBusinessObjectMapper.ToUiPerson).OrderBy(_ => _.LastName));
      }
      catch (Exception e)
      {
        ReportException(e);
      }
    }

    private void LoadParticipationList()
    {
      try
      {
        Func<ParticipationDetails, ParticipationTreeItem> selector =
          (gmd) =>
            new ParticipationTreeItem
            {
              ParticipationNames = gmd.ParticipationNames,
              ParticipationDescription = gmd.ParticipationDescription
            };

        //IEnumerable<ParticipationTreeItem> participationTreeItems;
        List<ShooterParticipationDetails> participations = new List<ShooterParticipationDetails>();
        if (SelectedUiShooter != null)
        {
          //participationTreeItems = _groupDetailsView.FindByShooterId(SelectedAvailableUiShooter.ShooterId).Select(selector);
          //participations = _shooterParticipationView.FindByShooterId(SelectedAvailableUiShooter.ShooterId);
          IEnumerable<ShooterParticipation> shooterParticipations =
            _shooterParticipationDataStore.FindByShooterId(SelectedUiShooter.ShooterId).ToList();
          foreach (ShooterParticipation shooterParticipation in shooterParticipations)
          {
            Participation participation = _participationDataStore.FindById(shooterParticipation.ParticipationId);
            participations.Add(new ShooterParticipationDetails
            {
              ParticipationName = participation.ParticipationName,
              ShooterParticipationId = shooterParticipation.ShooterParticipationId
            });
          }

        }
          //else if (SelectedUiPerson != null)
          //{
          //  //participationTreeItems = _groupDetailsView.FindByPersonId(SelectedUiPerson.PersonId).Select(selector);
          //}
        else
        {
          //participationTreeItems = _groupDetailsView.GetAll().Select(selector);
          IEnumerable<ShooterParticipation> shooterParticipations = _shooterParticipationDataStore.GetAll().ToList();
          foreach (ShooterParticipation shooterParticipation in shooterParticipations)
          {
            Participation participation = _participationDataStore.FindById(shooterParticipation.ParticipationId);
            participations.Add(new ShooterParticipationDetails
            {
              ParticipationName = participation.ParticipationName,
              ShooterParticipationId = shooterParticipation.ShooterParticipationId
            });
          }
        }

        Participations =
          new ObservableCollection<ShooterParticipationListItem>(
            participations.OrderBy(_ => _.ParticipationName).Select(_ => new ShooterParticipationListItem
            {
              ParticipationName = _.ParticipationName,
              ShooterParticipationId = _.ShooterParticipationId,
            }));
      }
      catch (Exception e)
      {
        ReportException(e);
      }
      //ParticipationTreeItems = new ObservableCollection<ParticipationTreeItem>(participationTreeItems.Where(_ => _.ParticipationNames.Any()));
    }

    private void LoadShooterList()
    {
      try
      {
        int selectedShooterId = default (int);
        if (SelectedUiShooter != null)
        {
          selectedShooterId = SelectedUiShooter.ShooterId;
        }

        Func<Shooter, UiShooter> selector = shooter => new UiShooter
        {
          PersonId = shooter.PersonId,
          ShooterNumber = shooter.ShooterNumber,
          ShooterId = shooter.ShooterId
        };

        List<UiShooter> shooterListItems;
        if (SelectedUiPerson != null)
        {
          shooterListItems = _shooterDataStore.FindByPersonId(SelectedUiPerson.PersonId).Select(selector).ToList();
        }
        else
        {
          shooterListItems = _shooterDataStore.GetAll().Select(selector).ToList();
        }

        ShooterListItems = new ObservableCollection<UiShooter>(shooterListItems.Select(_ => _.FetchPerson(_personDataStore)));
        SelectedUiShooter = ShooterListItems.SingleOrDefault(_ => _.ShooterId == selectedShooterId);
      }
      catch (Exception e)
      {
        ReportException(e);
      }
    }

    #endregion

    #region Private methods

    private void SelectFirstShooter()
    {
      if (ShooterListItems != null)
        SelectedUiShooter = ShooterListItems.FirstOrDefault();
    }

    #endregion
  }
}
