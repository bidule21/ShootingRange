using ShootingRange.Common;
using ShootingRange.Common.Modules;
using ShootingRange.ConfigurationProvider;
using ShootingRange.Persistence;
using ShootingRange.Repository;
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
    private IWindowService _windowService;

    public DefaultConfiguration()
    {
      //_shootingRange = new SiusDataFileProvider(@"20130914_132912.log");
      //_shootingRange = new SiusApiProvider("http://192.168.1.4");
      _events = new ShootingRangeEvents();
      _uiEvents = new UIEvents();
      //_personRepository = new FakePersonRepository();
      _personRepository = new PersonDataStore(new ShootingRangeEntities());
      _windowService = new WindowService();
    }

    public UIEvents GetUIEvents()
    {
      return _uiEvents;
    }

    public IPersonDataStore GetPersonRepository()
    {
      return _personRepository;
    }

    public IWindowService GetWindowService()
    {
      return _windowService;
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
