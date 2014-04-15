using ShootingRange.Common;
using ShootingRange.Common.Modules;
using ShootingRange.Repository;

namespace ShootingRange.Engine
{
  public class DummyRangeConfigurationFactory : IConfigurationFactory
  {


    public IShootingRange GetShootingRange()
    {
      throw new System.NotImplementedException();
    }

    public ShootingRangeEvents GetEvents()
    {
      throw new System.NotImplementedException();
    }

    public IPersonDataStore GetPersonRepository()
    {
      return new FakePersonDataStore();
    }
  }
}