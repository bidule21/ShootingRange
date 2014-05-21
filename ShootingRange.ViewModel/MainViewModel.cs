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
using ShootingRange.Repository;
using ShootingRange.Repository.FakeRepositories;
using ShootingRange.Repository.Mapper;
using ShootingRange.Repository.Repositories;
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

        _shooterNumberService = config.GetShooterNumberService();
        _windowService = config.GetWindowService();
        _barcodePrintService = config.GetBarcodePrintService();
        _barcodeBuilderService = config.GetBarcodeBuilderService();
        _events = config.GetEvents();
        _uiEvents = config.GetUIEvents();
        _uiEvents.RequireSelectedPerson += () => _uiEvents.PersonSelected(SelectedUiPerson);
        _uiEvents.RequireSelectedShooter += () => _uiEvents.ShooterSelected(SelectedUiShooter);
        _uiEvents.PersonDataStoreChanged += LoadPersonList;
        _uiEvents.ShooterDataStoreChanged += LoadShooterList;
      }

      //IEnumerable<Person> people = new ObservableCollection<Person>(_personDataStore.GetAll());
      LoadPersonList();
      LoadShooterList();
      LoadParticipationList();
      //UiPeople =
      //  new ObservableCollection<UiPerson>(
      //    people.Select(_ => new UiPerson
      //    {
      //      PersonId = _.PersonId,
      //      FirstName = _.FirstName,
      //      LastName = _.LastName
      //    }));

      PersonSelectionChangedCommand = new RelayCommand<int>(ExecutePersonSelectionChangedCommand);

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
    }

    private void ExecutePrintBarcodeCommand(UiShooter uiShooter)
    {
      try
      {
        Person person = _personDataStore.FindById(uiShooter.PersonId);
        ShooterParticipationDetails particiapDetails = _shooterParticipationView.FindByShooterId(uiShooter.ShooterId).FirstOrDefault();
        BarcodeInfo barcodeInfo = new BarcodeInfo
        {
          FirstName = person.FirstName,
          LastName = person.LastName,
          DateOfBirth = person.DateOfBirth,
          GroupInfo = particiapDetails == null ? string.Empty : string.Format("Gruppenwettkampf:\r\n{0}", particiapDetails.ParticipationName),
          Barcode = _barcodeBuilderService.BuildBarcode(uiShooter.ShooterNumber, uiShooter.Legalization)
        };

        _barcodePrintService.Print(barcodeInfo);
      }
      catch (Exception e)
      {
        ReportException(e);
      }
    }

    private bool CanExecutePrintBarcodeCommand(UiShooter uiShooter)
    {
      return uiShooter != null;
    }

    #region Commands

    #region Person

    private void ExecutePersonSelectionChangedCommand(int selectedPersonId)
    {
      try
      {
        _events.SelectedPersonChanged(selectedPersonId);
      }
      catch (Exception e)
      {
        ReportException(e);
      }
    }

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

    private bool CanExecuteDeletePersonCommand(UiPerson uiPerson)
    {
      return uiPerson != null;
    }

    private void ExecuteDeletePersonCommand(UiPerson uiPerson)
    {
      try
      {
        _personDataStore.Delete(uiPerson.ToPerson());
        _uiEvents.PersonDataStoreChanged();
      }
      catch (Exception e)
      {
        ReportException(e);
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
        _windowService.ShowCreateShooterWindow();
      }
      catch (Exception e)
      {
        ReportException(e);
      }
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
        _shooterDataStore.Delete(uiShooter.ToShooter());
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

    #endregion

    public ICommand PersonSelectionChangedCommand { get; private set; }
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

    #region Properties


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

        Person person = _personDataStore.FindById(selectedUiShooter.PersonId);
        detailsStringBuilder.AppendFormat("{0}, {1} [{2}] : {3} [{4}]\r\n{5}\r\n", person.LastName, person.FirstName,
          person.PersonId, selectedUiShooter.ShooterNumber, selectedUiShooter.ShooterId, person.DateOfBirth.ToString("dd.MM.yyyy"));
        detailsStringBuilder.AppendLine();
        IEnumerable<SessionDetails> sessionDetails = _sessionDetailsView.FindByShooterId(selectedUiShooter.ShooterId).OrderBy(_ => _.ProgramNumber).ToList();

        foreach (SessionDetails sessionDetail in sessionDetails)
        {
          detailsStringBuilder.AppendFormat("{0} [{1}] = {2}\r\n", sessionDetail.SessionDescription, sessionDetail.SessionId, sessionDetail.Shots.Sum(_ => _.PrimaryScore));
        }

        DetailsView = detailsStringBuilder.ToString();

        SessionTreeViewItems =
          new ObservableCollection<SessionTreeViewItem>(sessionDetails.Select(_ => new SessionTreeViewItem
          {
            SessionHeader =
              _.SessionDescription + " (" + _.Shots.Count() + "): " + _.Shots.Sum(shot => shot.PrimaryScore),
            Shots = _.Shots.OrderBy(shot => shot.Ordinal).Select(shot => string.Format("{0}\t{1}", shot.Ordinal, shot.PrimaryScore))
          }));
      }
    }

    private void OnSelectedPersonItemChanged(UiPerson selectedUiPerson)
    {
      LoadShooterList();
      LoadParticipationList();
      SelectFirstShooter();
    }

    private void SelectFirstShooter()
    {
      SelectedUiShooter = ShooterListItems.FirstOrDefault();
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
          Person person = _personDataStore.FindById(shooter.PersonId);
          ShooterParticipation innerShooterParticipation = _shooterParticipationDataStore.FindByShooterId(shooter.ShooterId).First();
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
      UiPeople = new ObservableCollection<UiPerson>(_personDataStore.GetAll().Select(UiBusinessObjectMapper.ToUiPerson).OrderBy(_ => _.LastName));
    }

    private void LoadParticipationList()
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
        //participationTreeItems = _groupDetailsView.FindByShooterId(SelectedUiShooter.ShooterId).Select(selector);
        //participations = _shooterParticipationView.FindByShooterId(SelectedUiShooter.ShooterId);
        IEnumerable<ShooterParticipation> shooterParticipations = _shooterParticipationDataStore.FindByShooterId(SelectedUiShooter.ShooterId).ToList();
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
      
      Participations = new ObservableCollection<ShooterParticipationListItem>(participations.OrderBy(_ => _.ParticipationName).Select(_ => new ShooterParticipationListItem
      {
        ParticipationName = _.ParticipationName,
        ShooterParticipationId = _.ShooterParticipationId,
      }));
      //ParticipationTreeItems = new ObservableCollection<ParticipationTreeItem>(participationTreeItems.Where(_ => _.ParticipationNames.Any()));
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

    private void FillDetailsWithParticipation()
    {
      
    }

    private void LoadShooterList()
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

      IEnumerable<UiShooter> shooterListItems;
      if (SelectedUiPerson != null)
      {
        shooterListItems = _shooterDataStore.FindByPersonId(SelectedUiPerson.PersonId).Select(selector);
      }
      else
      {
        shooterListItems = _shooterDataStore.GetAll().Select(selector);
      }

      ShooterListItems = new ObservableCollection<UiShooter>(shooterListItems);
      SelectedUiShooter = ShooterListItems.SingleOrDefault(_ => _.ShooterId == selectedShooterId);
    }

    #endregion
  }
}
