using System.Configuration;
using System.Linq;
using System.Windows;
using Autofac;
using Autofac.Extras.CommonServiceLocator;
using Gui.ViewModel;
using Gui.Wpf;
using Microsoft.Practices.ServiceLocation;
using ShootingRange.BarcodePrinter;
using ShootingRange.BusinessObjects;
using ShootingRange.ConfigurationProvider;
using ShootingRange.Persistence;
using ShootingRange.Repository.Repositories;
using ShootingRange.Repository.RepositoryInterfaces;
using ShootingRange.Service;
using ShootingRange.Service.Interface;
using ShootingRange.ServiceDesk.View;
using ShootingRange.ServiceDesk.View.Controls;
using ShootingRange.ServiceDesk.View.Dialogs;
using ShootingRange.ServiceDesk.ViewModel;
using ShootingRange.ServiceDesk.ViewModel.MessageTypes;
using SiusUtils;

namespace ShootingRange.ServiceDesk
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Configuration _config;
        private ViewService _vs;
        private IMessenger _messenger;
        private MainWindowViewModel _viewModel;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureContainer();
            ComposeObjects();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _config.Save();
            base.OnExit(e);
        }

        public void ConfigureContainer()
        {
            ShootingRangeEntities entities = new ShootingRangeEntities();

            _messenger = new Messenger();
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterInstance(_messenger).As<IMessenger>();

            IShooterNumberService shooterNumberService = new ShooterNumberService(new ShooterNumberConfigDataStore(entities)); 
            IPersonDataStore personDataStore = new PersonDataStore(entities);
            IShooterCollectionDataStore shooterCollectionDataStore = new ShooterCollectionDataStore(entities);
            ICollectionShooterDataStore collectionShooterDataStore = new CollectionShooterDataStore(entities);
            IShooterDataStore shooterDataStore = new ShooterDataStore(entities);
            IShooterParticipationDataStore shooterParticipationDataStore = new ShooterParticipationDataStore(entities);
            ISessionDataStore sessionDataStore = new SessionDataStore(entities);
            ISessionSubtotalDataStore sessionSubtotalDataStore = new SessionSubtotalDataStore(entities);
            IShotDataStore shotDataStore = new ShotDataStore(entities);

            IBarcodePrintService barcodePrinter = new PtouchBarcodePrinter();
            IBarcodeBuilderService barcodeBuilder = new Barcode2Of5InterleavedService();
            ISsvShooterDataWriterService shooterDataWriter = new SsvFileWriter(@"C:\Sius\SiusData\SSVDaten\SSV_schuetzen.txt");

            builder.RegisterInstance(shooterNumberService).As<IShooterNumberService>();
            builder.RegisterInstance(personDataStore).As<IPersonDataStore>();
            builder.RegisterInstance(shooterDataStore).As<IShooterDataStore>();
            builder.RegisterInstance(new ShooterParticipationDataStore(entities)).As<IShooterParticipationDataStore>();
            builder.RegisterInstance(shooterCollectionDataStore).As<IShooterCollectionDataStore>();
            builder.RegisterInstance(collectionShooterDataStore).As<ICollectionShooterDataStore>();
            builder.RegisterInstance(sessionDataStore).As<ISessionDataStore>();
            builder.RegisterInstance(sessionSubtotalDataStore).As<ISessionSubtotalDataStore>();
            builder.RegisterInstance(shotDataStore).As<IShotDataStore>();

            builder.RegisterInstance(barcodePrinter).As<IBarcodePrintService>();
            builder.RegisterInstance(barcodeBuilder).As<IBarcodeBuilderService>();
            builder.RegisterInstance(shooterDataWriter).As<ISsvShooterDataWriterService>();

            _vs = new ViewService();
            ViewServiceHandler vsh = new ViewServiceHandler();

            #region Windows and Dialogs

            _vs.RegisterFunction<MainWindowViewModel, IWindow>(
                (window, model) => vsh.GetOwnedWindow<MainWindow>((Window) window, model));

            #endregion

            _vs.RegisterFunction<PersonsPageViewModel, IPage>(
                (window, model) => vsh.GetUserControl<UcPersons>((Window) window, model));
            _vs.RegisterFunction<GroupsPageViewModel, IPage>(
                (window, model) => vsh.GetUserControl<UcGroups>((Window) window, model));
            _vs.RegisterFunction<ResultsPageViewModel, IPage>(
                (window, model) => vsh.GetUserControl<UcResults>((Window) window, model));
            _vs.RegisterFunction<RankViewModel, IPage>(
                (window, model) => vsh.GetUserControl<UcRankings>((Window) window, model));

            _vs.RegisterFunction<CreatePersonViewModel, IWindow>(
                (window, model) => vsh.GetOwnedWindow<CreatePerson>((Window) window, model));
            _vs.RegisterFunction<CreateGroupingViewModel, IWindow>(
                (window, model) => vsh.GetOwnedWindow<CreateGrouping>((Window) window, model));
            _vs.RegisterFunction<EditGroupingViewModel, IWindow>(
                (window, model) => vsh.GetOwnedWindow<EditGrouping>((Window) window, model));
            _vs.RegisterFunction<SelectParticipationViewModel, IWindow>(
                (window, model) => vsh.GetOwnedWindow<AddParticipationToShooterDialog>((Window) window, model));
            _vs.RegisterFunction<SelectGroupingViewModel, IWindow>(
                (window, model) => vsh.GetOwnedWindow<AddGroupingToShooterDialog>((Window) window, model));
            _vs.RegisterFunction<SelectShooterViewModel, IWindow>(
                (window, model) => vsh.GetOwnedWindow<AddShooterToGroupingDialog>((Window) window, model));
            _vs.RegisterFunction<ReassignSessionViewModel, IWindow>(
                (window, model) => vsh.GetOwnedWindow<ReassignSessionDialog>((Window) window, model));
            _vs.RegisterFunction<ReassignProgramNumberViewModel, IWindow>(
                (window, model) => vsh.GetOwnedWindow<ReassignProgramNumber>((Window) window, model));

            _vs.RegisterFunction<YesNoMessageBoxViewModel, IWindow>(
                (w, m) => vsh.GetOwnedWindow<YesNoMessageBox>((Window) w, m));
            _vs.RegisterFunction<MessageBoxViewModel, IWindow>((w, m) => vsh.GetOwnedWindow<OkMessageBox>((Window) w, m));

            InitializeServiceDeskConfiguration();
            _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ServiceDeskConfiguration serviceDeskConfiguration =
                _config.GetSection("ServiceDeskConfiguration") as ServiceDeskConfiguration;

            if (serviceDeskConfiguration == null)
            {
                serviceDeskConfiguration = new ServiceDeskConfiguration();
                _config.Sections.Add("ServiceDeskConfiguration", serviceDeskConfiguration);
            }

            builder.Register(c => serviceDeskConfiguration);
            
            IContainer container = builder.Build();
            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));

            PersonsPageViewModel personsPageViewModel = new PersonsPageViewModel();
            personsPageViewModel.Initialize();

            GroupsPageViewModel groupsPageViewModel = new GroupsPageViewModel();
            groupsPageViewModel.Initialize();

            ResultsPageViewModel resultsPageViewModel = new ResultsPageViewModel();
            resultsPageViewModel.Initialize();

            RankViewModel rankViewModel = new RankViewModel();
            rankViewModel.Initialize();

            RegisterCreatePersonDialog(personDataStore);
            RegisterEditPersonDialog(personDataStore);
            RegisterCreateGroupingDialog(shooterCollectionDataStore, serviceDeskConfiguration);
            RegisterEditGroupingDialog(shooterCollectionDataStore);
            RegisterDeletePersonDialog(personDataStore);
            RegisterDeleteGroupingDialog(shooterCollectionDataStore);
            RegisterAddShooterToGroupingDialog(collectionShooterDataStore);
            RegisterDeleteShooterDialog(shooterDataStore);
            RegisterAddGroupingToShooterDialog(collectionShooterDataStore);
            RegisterRemoveGroupingFromShooterDialog(collectionShooterDataStore);
            RegisterAddParticipationToShooterDialog(shooterParticipationDataStore);
            RegisterRemoveParticipationFromShooterDialog(shooterParticipationDataStore);
            RegisterMessageBoxDialog();
            RegisterReassignSessionDialog(sessionDataStore);
            RegisterReassignShooterNumberDialog(sessionDataStore);

            RegisterShowShooterPageMessage(personsPageViewModel);
            RegisterShowGroupsPageMessage(groupsPageViewModel);
            // RegisterShowResultsPageMessage(resultsPageViewModel);
            RegisterShowRankingsPageMessage(rankViewModel);
        }

        private void ComposeObjects()
        {
            _viewModel = new MainWindowViewModel();
            Window mainWindow = (Window) _vs.ExecuteFunction<MainWindowViewModel, IWindow>(null, _viewModel);
            Current.MainWindow = mainWindow;
            Current.MainWindow.Show();

            ServiceLocator.Current.GetInstance<IMessenger>().Send(new ShowPersonsPageMessage());
        }

        private static void InitializeServiceDeskConfiguration()
        {
            ServiceDeskConfiguration serviceDeskConfiguration = new ServiceDeskConfiguration();
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.GetSection("ServiceDeskConfiguration") == null)
            {
                config.Sections.Add("ServiceDeskConfiguration", serviceDeskConfiguration);

                serviceDeskConfiguration.SectionInformation.ForceSave = true;
                config.Save();
            }
        }

        #region Dialogs

        private void RegisterCreatePersonDialog(IPersonDataStore personDataStore)
        {
            _messenger.Register<CreatePersonDialogMessage>(this,
                x =>
                {
                    var m = new CreatePersonViewModel
                    {
                        Title = "Person erstellen"
                    };
                    IWindow w = _vs.ExecuteFunction<CreatePersonViewModel, IWindow>((IWindow) Current.MainWindow, m);
                    bool? result = w.ShowDialog();
                    if (!result.HasValue || !result.Value) return;

                    Person person = new Person
                    {
                        FirstName = m.FirstName,
                        LastName = m.LastName,
                        DateOfBirth = m.DateOfBirth,
                        Address = m.Address,
                        City = m.City,
                        ZipCode = m.ZipCode,
                        Email = m.Email,
                        Phone = m.Phone
                    };
                    personDataStore.Create(person);
                    _messenger.Send(new RefreshDataFromRepositoriesMessage());
                    _messenger.Send(new SetSelectedPersonMessage(person.PersonId));
                });
        }

        private void RegisterEditPersonDialog(IPersonDataStore personDataStore)
        {
            _messenger.Register<EditPersonDialogMessage>(this,
                x =>
                {
                    Person p = personDataStore.FindById(x.PersonId);
                    var m = new CreatePersonViewModel()
                    {
                        Title = "Person editieren",
                        PersonId = p.PersonId,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        DateOfBirth = p.DateOfBirth,
                        Address = p.Address,
                        City = p.City,
                        ZipCode = p.ZipCode,
                        Email = p.Email,
                        Phone = p.Phone
                    };

                    IWindow w = _vs.ExecuteFunction<CreatePersonViewModel, IWindow>((IWindow) Current.MainWindow, m);
                    bool? result = w.ShowDialog();
                    if (!result.HasValue || !result.Value) return;

                    personDataStore.Update(new Person
                    {
                        PersonId = m.PersonId,
                        FirstName = m.FirstName,
                        LastName = m.LastName,
                        DateOfBirth = m.DateOfBirth,
                        Address = m.Address,
                        City = m.City,
                        ZipCode = m.ZipCode,
                        Email = m.Email,
                        Phone = m.Phone
                    });

                    _messenger.Send(new RefreshDataFromRepositoriesMessage());
                    _messenger.Send(new SetSelectedPersonMessage(m.PersonId));
                });
        }

        private void RegisterDeletePersonDialog(IPersonDataStore personDataStore)
        {
            _messenger.Register<DeletePersonDialogMessage>(this,
                x =>
                {
                    YesNoMessageBoxViewModel vm = new YesNoMessageBoxViewModel
                    {
                        DefaultYes = false,
                        Caption = "Person löschen?",
                        Message =
                            string.Format("Möchtest du die Person '{0} {1}' wirklich löschen?",
                                x.FirstName,
                                x.LastName)
                    };

                    IWindow w = _vs.ExecuteFunction<YesNoMessageBoxViewModel, IWindow>((IWindow) Current.MainWindow, vm);
                    bool? result = w.ShowDialog();
                    if (!result.HasValue || !result.Value) return;

                    Person person = personDataStore.FindById(x.PersonId);
                    personDataStore.Delete(person);
                    _messenger.Send(new RefreshDataFromRepositoriesMessage());
                });
        }

        private void RegisterCreateGroupingDialog(IShooterCollectionDataStore shooterCollectionDataStore, ServiceDeskConfiguration serviceDeskConfiguration)
        {
            _messenger.Register<CreateGroupingDialogMessage>(this,
                x =>
                {
                    var m = new CreateGroupingViewModel(serviceDeskConfiguration)
                    {
                        Title = "Gruppierung erstellen"
                    };

                    IWindow w = _vs.ExecuteFunction<CreateGroupingViewModel, IWindow>((IWindow) Current.MainWindow, m);
                    bool? result = w.ShowDialog();
                    if (!result.HasValue || !result.Value) return;

                    ShooterCollection sc = new ShooterCollection
                    {
                        CollectionName = m.GroupingName,
                        ProgramNumber = int.Parse(m.SelectedParticipation.ProgramNumber)
                    };

                    shooterCollectionDataStore.Create(sc);
                    _messenger.Send(new RefreshDataFromRepositoriesMessage());
                });
        }

        private void RegisterEditGroupingDialog(IShooterCollectionDataStore shooterCollectionDataStore)
        {
            _messenger.Register<EditGroupingDialogMessage>(this,
                x =>
                {
                    var m = new EditGroupingViewModel
                    {
                        GroupingName = x.GroupingName,
                        GroupingId = x.GroupingId,
                        Title = "Gruppenname ändern"
                    };

                    IWindow w = _vs.ExecuteFunction<EditGroupingViewModel, IWindow>((IWindow) Current.MainWindow, m);
                    bool? result = w.ShowDialog();
                    if (!result.HasValue || !result.Value) return;

                    ShooterCollection sc = shooterCollectionDataStore.FindById(m.GroupingId);
                    sc.CollectionName = m.GroupingName;
                    shooterCollectionDataStore.Update(sc);

                    _messenger.Send(new RefreshDataFromRepositoriesMessage());
                });
        }

        private void RegisterDeleteGroupingDialog(IShooterCollectionDataStore shooterCollectionDataStore)
        {
            _messenger.Register<DeleteGroupingDialogMessage>(this,
                x =>
                {
                    YesNoMessageBoxViewModel vm = new YesNoMessageBoxViewModel
                    {
                        Caption = "Gruppierung löschen",
                        Message =
                            string.Format("Wollen Sie die Gruppierung '{0}' wirklich löschen?",
                                x.CollectionName)
                    };

                    IWindow w = _vs.ExecuteFunction<YesNoMessageBoxViewModel, IWindow>((IWindow) Current.MainWindow, vm);
                    bool? result = w.ShowDialog();
                    if (!result.HasValue || !result.Value) return;
                    
                    ShooterCollection sc = shooterCollectionDataStore.FindById(x.ShooterCollectionId);
                    shooterCollectionDataStore.Delete(sc);
                    _messenger.Send(new RefreshDataFromRepositoriesMessage());
                });
        }

        private void RegisterDeleteShooterDialog(IShooterDataStore shooterDataStore)
        {
            _messenger.Register<DeleteShooterDialogMessage>(this,
                x =>
                {
                    YesNoMessageBoxViewModel vm = new YesNoMessageBoxViewModel
                    {
                        DefaultYes = false,
                        Caption = "Schütze löschen?",
                        Message =
                            string.Format("Möchtest du den Schützen '{0}' wirklich löschen?",
                                x.ShooterNumber)
                    };

                    IWindow w = _vs.ExecuteFunction<YesNoMessageBoxViewModel, IWindow>((IWindow) Current.MainWindow, vm);
                    bool? result = w.ShowDialog();
                    if (!result.HasValue || !result.Value) return;

                    Shooter shooter = shooterDataStore.FindByShooterNumber(x.ShooterNumber);
                    shooterDataStore.Delete(shooter);
                    _messenger.Send(new RefreshDataFromRepositoriesMessage());
                });
        }

        private void RegisterRemoveGroupingFromShooterDialog(ICollectionShooterDataStore collectionShooterDataStore)
        {
            _messenger.Register<RemoveGroupingFromShooterDialogMessage>(this,
                x =>
                {
                    YesNoMessageBoxViewModel vm = new YesNoMessageBoxViewModel
                    {
                        DefaultYes = false,
                        Caption = "Gruppierungszugehörigkeit löschen?",
                        Message =
                            string.Format("Möchtest du die Gruppierungszugehörigkeit '{0}' wirklich löschen?",
                                x.Grouping.GroupingName)
                    };

                    IWindow w = _vs.ExecuteFunction<YesNoMessageBoxViewModel, IWindow>((IWindow) Current.MainWindow, vm);
                    bool? result = w.ShowDialog();

                    if (!result.HasValue || !result.Value) return;

                    CollectionShooter cs = collectionShooterDataStore.FindById(x.Grouping.ShooterCollectionId);
                    collectionShooterDataStore.Delete(cs);

                    _messenger.Send(new RefreshDataFromRepositoriesMessage());
                });
        }

        private void RegisterAddShooterToGroupingDialog(ICollectionShooterDataStore collectionShooterDataStore)
        {
            _messenger.Register<AddShooterToGroupingDialogMessage>(this,
                x =>
                {
                    SelectShooterViewModel vm = new SelectShooterViewModel
                    {
                        Title = "Schütze auswählen"
                    };

                    vm.Initialize(x.ProgramNumber);
                    IWindow w = _vs.ExecuteFunction<SelectShooterViewModel, IWindow>((IWindow) Current.MainWindow, vm);
                    bool? result = w.ShowDialog();
                    if (!result.HasValue || !result.Value) return;

                    if (vm.SelectedShooter == null) return;
                    
                    CollectionShooter cs = new CollectionShooter
                    {
                        ShooterId = vm.SelectedShooter.ShooterId,
                        ShooterCollectionId = x.ShooterCollectionId
                    };
                    collectionShooterDataStore.Create(cs);

                    _messenger.Send(new RefreshDataFromRepositoriesMessage());
                    _messenger.Send(new SetSelectedShooterCollectionMessage(x.ShooterCollectionId));
                });
        }

        private void RegisterAddGroupingToShooterDialog(ICollectionShooterDataStore collectionShooterDataStore)
        {
            _messenger.Register<AddGroupingToShooterDialogMessage>(this,
                x =>
                {
                    SelectGroupingViewModel vm = new SelectGroupingViewModel
                    {
                        Title = "Gruppe auswählen"
                    };

                    vm.Initialize(x.ShooterId);

                    IWindow w = _vs.ExecuteFunction<SelectGroupingViewModel, IWindow>((IWindow) Current.MainWindow, vm);
                    bool? result = w.ShowDialog();
                    if (!result.HasValue || !result.Value) return;

                    CollectionShooter cs = new CollectionShooter
                    {
                        ShooterId = x.ShooterId,
                        ShooterCollectionId = vm.SelectedGrouping.ShooterCollectionId
                    };
                    collectionShooterDataStore.Create(cs);

                    _messenger.Send(new RefreshDataFromRepositoriesMessage());
                });
        }

        private void RegisterAddParticipationToShooterDialog(IShooterParticipationDataStore shooterParticipationDataStore)
        {
            _messenger.Register<AddParticipationToShooterDialogMessage>(this,
                x =>
                {
                    SelectParticipationViewModel vm = new SelectParticipationViewModel
                    {
                        Title = "Wettkampf auswählen"
                    };

                    vm.Initialize(x.ShooterId);

                    IWindow w = _vs.ExecuteFunction<SelectParticipationViewModel, IWindow>((IWindow) Current.MainWindow, vm);
                    bool? result = w.ShowDialog();

                    if (!result.HasValue || !result.Value) return;

                    ShooterParticipation sp = new ShooterParticipation
                    {
                        ProgramNumber = int.Parse(vm.SelectedParticipationDescription.ProgramNumber),
                        ShooterId = x.ShooterId
                    };

                    shooterParticipationDataStore.Create(sp);

                    _messenger.Send(new RefreshDataFromRepositoriesMessage());
                });
        }

        private void RegisterRemoveParticipationFromShooterDialog(IShooterParticipationDataStore shooterParticipationDataStore)
        {
            _messenger.Register<RemoveParticipationFromShooterDialogMessage>(this,
                x =>
                {
                    YesNoMessageBoxViewModel vm = new YesNoMessageBoxViewModel
                    {
                        Caption = "Wettkampf löschen",
                        DefaultYes = false,
                        Message =
                            string.Format("Wettkampfzuordnung '{0}' wirklich löschen?", x.Participation.ProgramName)
                    };

                    IWindow w = _vs.ExecuteFunction<YesNoMessageBoxViewModel, IWindow>((IWindow) Current.MainWindow, vm);
                    bool? result = w.ShowDialog();

                    if (!result.HasValue || !result.Value) return;

                    ShooterParticipation shooterParticipation =
                        shooterParticipationDataStore.FindByShooterId(x.ShooterId)
                            .SingleOrDefault(sp => sp.ProgramNumber == x.Participation.ProgramNumber);
                    shooterParticipationDataStore.Delete(shooterParticipation);

                    _messenger.Send(new RefreshDataFromRepositoriesMessage());
                });
        }

        private void RegisterReassignSessionDialog(ISessionDataStore sessionDataStore)
        {
            _messenger.Register<ShowReassignSessionDialogMessage>(this,
                x =>
                {
                    ReassignSessionViewModel vm = new ReassignSessionViewModel
                    {
                        Title = "Schütze neu zuweisen"
                    };
                    vm.Initialize(x.SessionId);

                    IWindow w = _vs.ExecuteFunction<ReassignSessionViewModel, IWindow>((IWindow) Current.MainWindow, vm);
                    bool? result = w.ShowDialog();

                    if (!result.HasValue || !result.Value) return;

                    Session s = sessionDataStore.FindById(x.SessionId);
                    s.ShooterId = vm.SelectedShooter.ShooterId;
                    sessionDataStore.Update(s);

                    _messenger.Send(new RefreshDataFromRepositoriesMessage());
                });
        }

        private void RegisterReassignShooterNumberDialog(ISessionDataStore sessionDataStore)
        {
            _messenger.Register<ShowReassignShooterNumberDialogMessage>(this,
                x =>
                {
                    ReassignProgramNumberViewModel vm = new ReassignProgramNumberViewModel
                    {
                        Title = "Programm neu zuweisen"
                    };
                    vm.Initialize(x.SessionId);

                    IWindow w = _vs.ExecuteFunction<ReassignProgramNumberViewModel, IWindow>((IWindow)Current.MainWindow, vm);
                    bool? result = w.ShowDialog();

                    if (!result.HasValue || !result.Value) return;

                    Session s = sessionDataStore.FindById(x.SessionId);
                    s.ProgramNumber = vm.SelectedParticipation.ProgramNumber;
                    sessionDataStore.Update(s);

                    _messenger.Send(new RefreshDataFromRepositoriesMessage());
                });
        }

        private void RegisterMessageBoxDialog()
        {
            _messenger.Register<DialogMessage>(this,
                x =>
                {
                    MessageBoxViewModel vm = new MessageBoxViewModel
                    {
                        Caption = x.Caption,
                        Message = x.Message,
                        Icon = x.Icon
                    };

                    IWindow w = _vs.ExecuteFunction<MessageBoxViewModel, IWindow>((IWindow) Current.MainWindow, vm);
                    w.ShowDialog();
                });
        }

        private void RegisterShowShooterPageMessage(PersonsPageViewModel personsPageViewModel)
        {
            _messenger.Register<ShowPersonsPageMessage>(this,
                x =>
                {
                    IPage page = _vs.ExecuteFunction<PersonsPageViewModel, IPage>((IWindow)Current.MainWindow, personsPageViewModel);
                    _viewModel.CurrentPage = page;
                    _messenger.Send(new RefreshDataFromRepositoriesMessage());
                });        }

        private void RegisterShowGroupsPageMessage(GroupsPageViewModel groupsPageViewModel)
        {
            _messenger.Register<ShowGroupsPageMessage>(this,
                x =>
                {
                    IPage page = _vs.ExecuteFunction<GroupsPageViewModel, IPage>((IWindow) Current.MainWindow, groupsPageViewModel);
                    _viewModel.CurrentPage = page;
                    _messenger.Send(new RefreshDataFromRepositoriesMessage());
                });
        }

        private void RegisterShowResultsPageMessage(ResultsPageViewModel resultsPageViewModel)
        {
            _messenger.Register<ShowResultsPageMessage>(this,
                x =>
                {
                    IPage page = _vs.ExecuteFunction<ResultsPageViewModel, IPage>((IWindow) Current.MainWindow, resultsPageViewModel);
                    _viewModel.CurrentPage = page;
                });

            _messenger.Send(new RefreshDataFromRepositoriesMessage());
        }

        private void RegisterShowRankingsPageMessage(RankViewModel rankViewModel)
        {
            _messenger.Register<ShowRankingsPageMessage>(this,
                x =>
                {
                    IPage page = _vs.ExecuteFunction<RankViewModel, IPage>((IWindow)Current.MainWindow, rankViewModel);
                    _viewModel.CurrentPage = page;
                });

            _messenger.Send(new RefreshDataFromRepositoriesMessage());
        }
        #endregion
    }
}