using ShootingRange.Common;
using ShootingRange.Common.Modules;
using ShootingRange.Repository;

namespace ShootingRange.Engine
{
  public class DummyRangeConfigurationFactory : IConfigurationFactory
  {
    private ShootingRangeEvents _events;

    public DummyRangeConfigurationFactory()
    {
      _events = new ShootingRangeEvents();
    }

    public IShootingRange GetShootingRange()
    {
      throw new System.NotImplementedException();
    }

    public ShootingRangeEvents GetEvents()
    {
      return _events;
    }

    public IPersonDataStore GetPersonRepository()
    {
      return new FakePersonDataStore();
    }
  }
}