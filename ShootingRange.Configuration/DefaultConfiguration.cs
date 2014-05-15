using ShootingRange.BarcodePrinter;
using ShootingRange.Common;
using ShootingRange.Common.Modules;
using ShootingRange.ConfigurationProvider;
using ShootingRange.Persistence;
using ShootingRange.Repository;
using ShootingRange.Repository.FakeRepositories;
using ShootingRange.Repository.Repositories;
using ShootingRange.Service;
using ShootingRange.Service.Interface;

namespace ShootingRange.Configuration
{
  public class DefaultConfiguration : IConfiguration
  {
    private IShootingRange _shootingRange;
    private ShootingRangeEvents _events;
    private UIEvents _uiEvents;
    private IPersonDataStore _personRepository;
    private IShooterDataStore _shooterRepository;
    private IParticipationDataStore _participationDataStore;
    private IShooterNumberConfigDataStore _shooterNumberConfigDataStore;
    private IWindowService _windowService;
    private IBarcodePrintService _barcodePrintService;
    private GroupMemberDetailsView _groupMemberDetailsView;
    private GroupDetailsView _groupDetailsView;
    private ShooterNumberService _shooterNumberService;
    private ShooterParticipationDataStore _shooterParticipationDataStore;
    private IShooterParticipationView _shooterParticipationView;
    private IBarcodeBuilderService _barcodeBuilderService;

    public DefaultConfiguration()
    {
      //_shootingRange = new SiusDataFileProvider(@"20130914_132912.log");
      //_shootingRange = new SiusApiProvider("http://192.168.1.4");
      _events = new ShootingRangeEvents();
      _uiEvents = new UIEvents();
      ShootingRangeEntities entites = new ShootingRangeEntities();
      _personRepository = new PersonDataStore(entites);
      _shooterRepository = new ShooterDataStore(entites);
      _participationDataStore = new ParticipationDataStore(entites);
      _shooterNumberConfigDataStore = new ShooterNumberConfigDataStore(entites);
      _shooterParticipationDataStore = new ShooterParticipationDataStore(entites);
      _groupMemberDetailsView = new GroupMemberDetailsView(entites);
      _groupDetailsView = new GroupDetailsView(entites);
      _shooterParticipationView = new ShooterParticipationView(entites);
      _windowService = new WindowService();
      _barcodePrintService = new PtouchBarcodePrinter();
      _barcodeBuilderService = new Barcode2Of5InterleavedService();
      _shooterNumberService = new ShooterNumberService(_shooterNumberConfigDataStore);
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

    public IGroupMemberDetailsView GetGroupMemberDetailsView()
    {
      return _groupMemberDetailsView;
    }

    public IGroupDetailsView GetGroupDetailsView()
    {
      return _groupDetailsView;
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
