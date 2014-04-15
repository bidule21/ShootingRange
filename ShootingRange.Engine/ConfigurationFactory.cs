using ShootingRange.Common;
using ShootingRange.Common.Modules;
using ShootingRange.Persistence;
using ShootingRange.Repository;

namespace ShootingRange.Engine
{
  public class ConfigurationFactory : IConfigurationFactory
  {
    private IShootingRange _shootingRange;
    private ShootingRangeEvents _events;
    private IPersonDataStore _personRepository;

    public ConfigurationFactory()
    {
      //_shootingRange = new SiusDataFileProvider(@"20130914_132912.log");
      //_shootingRange = new SiusApiProvider("http://192.168.1.4");
      _events = new ShootingRangeEvents();
      //_personRepository = new FakePersonRepository();
      _personRepository = new PersonDataStore(new ShootingRangeEntities());
    }

    public IPersonDataStore GetPersonRepository()
    {
      return _personRepository;
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
