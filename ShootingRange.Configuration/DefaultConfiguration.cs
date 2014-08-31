using ShootingRange.BarcodePrinter;
using ShootingRange.Common;
using ShootingRange.Common.Modules;
using ShootingRange.ConfigurationProvider;
using ShootingRange.Persistence;
using ShootingRange.Repository.Repositories;
using ShootingRange.Repository.RepositoryInterfaces;
using ShootingRange.Service;
using ShootingRange.Service.Interface;
using ShootingRange.SiusData;
using SiusUtils;

namespace ShootingRange.Configuration
{
  public class DefaultConfiguration : IConfiguration
  {
    private IShootingRange _shootingRange;
    private ShootingRangeEvents _events;
    private UIEvents _uiEvents;
    private IPersonDataStore _personRepository;
    private ISessionDataStore _sessionDataStore;
    private IShooterDataStore _shooterRepository;
    private IParticipationDataStore _participationDataStore;
    private IShooterNumberConfigDataStore _shooterNumberConfigDataStore;
    private ISessionSubtotalDataStore _sessionSubtotalDataStore;
    private IProgramItemDataStore _programItemDataStore;
    private IShotDataStore _shotDataStore;
    private IWindowService _windowService;
    private IBarcodePrintService _barcodePrintService;
    private GroupMemberDetailsView _groupMemberDetailsView;
    private GroupDetailsView _groupDetailsView;
    private ShooterNumberService _shooterNumberService;
    private ShooterParticipationDataStore _shooterParticipationDataStore;
    private IShooterParticipationView _shooterParticipationView;
    private ISessionDetailsView  _sessionDetailsView;
    private IBarcodeBuilderService _barcodeBuilderService;
    private ISsvShooterDataWriterService _ssvShooterDataWriterService;
    private IShooterCollectionDataStore _shooterCollectionDataStore;
    private IShooterCollectionParticipationDataStore _shooterCollectionParticipationDataStore;
    private ICollectionShooterDataStore _collectionShooterDataStore;

    public DefaultConfiguration()
    {
      //_shootingRange = new SiusDataFileProvider(@"C:\Users\eberlid\Dropbox\SSC\2014\Herbstschiessen\ShootingRange\20140516_164043.log");
      _shootingRange = new SiusDataFileProvider(@"C:\Users\eberlid\Dropbox\SSC\2014\Herbstschiessen\ShootingRange\20130914_132912.log");
      //_shootingRange = new SiusApiProvider("http://192.168.1.4");
      //_shootingRange = new SiusDataSocketProvider("192.168.1.4", 4000);
      _events = new ShootingRangeEvents();
      _uiEvents = new UIEvents();
      ShootingRangeEntities entities = new ShootingRangeEntities();
      _personRepository = new PersonDataStore(entities);
      _shooterRepository = new ShooterDataStore(entities);
      _participationDataStore = new ParticipationDataStore(entities);
      _sessionDataStore = new SessionDataStore(entities);
      _shotDataStore = new ShotDataStore(entities);
      _sessionDetailsView = new SessionDetailsView(entities);
      _shooterNumberConfigDataStore = new ShooterNumberConfigDataStore(entities);
      _shooterParticipationDataStore = new ShooterParticipationDataStore(entities);
      _sessionSubtotalDataStore = new SessionSubtotalDataStore(entities);
      _programItemDataStore = new ProgramItemDataStore(entities);
      _groupMemberDetailsView = new GroupMemberDetailsView(entities);
      _groupDetailsView = new GroupDetailsView(entities);
      _shooterParticipationView = new ShooterParticipationView(entities);
      _windowService = new WindowService();
      _barcodePrintService = new PtouchBarcodePrinter();
      _barcodeBuilderService = new Barcode2Of5InterleavedService();
      _shooterNumberService = new ShooterNumberService(_shooterNumberConfigDataStore);
      _shooterCollectionParticipationDataStore = new ShooterCollectionParticipationDataStore(entities);
      _shooterCollectionDataStore = new ShooterCollectionDataStore(entities);
      _collectionShooterDataStore = new CollectionShooterDataStore(entities);
      _ssvShooterDataWriterService = new SsvFileWriter(@"C:\Sius\SiusData\SSVDaten\SSV_schuetzen.txt");
    }

    public UIEvents GetUIEvents()
    {
      return _uiEvents;
    }

    public IPersonDataStore GetPersonDataStore()
    {
      return _personRepository;
    }

    public IShooterDataStore GetShooterDataStore()
    {
      return _shooterRepository;
    }

    public IParticipationDataStore GetParticipationDataStore()
    {
      return _participationDataStore;
    }

    public ISessionSubtotalDataStore GetSessionSubtotalDataStore()
    {
      return _sessionSubtotalDataStore;
    }

    public IGroupMemberDetailsView GetGroupMemberDetailsView()
    {
      return _groupMemberDetailsView;
    }

    public IGroupDetailsView GetGroupDetailsView()
    {
      return _groupDetailsView;
    }

    public ISessionDetailsView GetSessionDetailsView()
    {
      return _sessionDetailsView;
    }

    public IWindowService GetWindowService()
    {
      return _windowService;
    }

    public IShooterNumberService GetShooterNumberService()
    {
      return _shooterNumberService;
    }

    public IShooterParticipationDataStore GetShooterParticipationDataStore()
    {
      return _shooterParticipationDataStore;
    }

    public IShooterParticipationView GetShooterParticipationView()
    {
      return _shooterParticipationView;
    }

    public IBarcodePrintService GetBarcodePrintService()
    {
      return _barcodePrintService;
    }

    public IBarcodeBuilderService GetBarcodeBuilderService()
    {
      return _barcodeBuilderService;
    }

    public ISessionDataStore GetSessionDataStore()
    {
      return _sessionDataStore;
    }

    public IProgramItemDataStore GetProgramItemDataStore()
    {
      return _programItemDataStore;
    }

    public IShotDataStore GetShotDataStore()
    {
      return _shotDataStore;
    }

    public IShooterCollectionDataStore GetShooterCollectionDataStore()
    {
      return _shooterCollectionDataStore;
    }

    public IShooterCollectionParticipationDataStore GetShooterCollectionParticipationDataStore()
    {
      return _shooterCollectionParticipationDataStore;
    }

    public ICollectionShooterDataStore GetCollectionShooterDataStore()
    {
      return _collectionShooterDataStore;
    }

    public ISsvShooterDataWriterService GetSsvShooterDataWriterService()
    {
      return _ssvShooterDataWriterService;
    }

    public IShootingRange GetShootingRange()
    {
      return _shootingRange;
    }

    public ShootingRangeEvents GetEvents()
    {
      return _events;
    }
  }
}
