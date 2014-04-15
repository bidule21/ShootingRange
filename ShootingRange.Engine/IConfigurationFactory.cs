using ShootingRange.Common;
using ShootingRange.Common.Modules;
using ShootingRange.Repository;

namespace ShootingRange.Engine
{
  public interface IConfigurationFactory
  {
    IShootingRange GetShootingRange();
    ShootingRangeEvents GetEvents();
    IPersonDataStore GetPersonRepository();
  }
}
