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
    private IGroupDataStore _groupDataStore;
    private IWindowService _windowService;
    private GroupMemberDetailsView _groupMemberDetailsView;
    private GroupDetailsView _groupDetailsView;

    public DefaultConfiguration()
    {
      //_shootingRange = new SiusDataFileProvider(@"20130914_132912.log");
      //_shootingRange = new SiusApiProvider("http://192.168.1.4");
      _events = new ShootingRangeEvents();
      _uiEvents = new UIEvents();
      ShootingRangeEntities entites = new ShootingRangeEntities();
      _personRepository = new PersonDataStore(entites);
      _shooterRepository = new ShooterDataStore(entites);
      _groupDataStore = new GroupDataStore(entites);
      _groupMemberDetailsView = new GroupMemberDetailsView(entites);
      _groupDetailsView = new GroupDetailsView(entites);
      _windowService = new WindowService();
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

    public IGroupDataStore GetGroupDataStore()
    {
      return _groupDataStore;
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
      return new FakeShooterNumberService();
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
