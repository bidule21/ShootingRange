using ShootingRange.Common;
using ShootingRange.Common.Modules;

namespace ShootingRange.Engine
{
  public interface IConfigurationFactory
  {
    IShootingRange GetShootingRange();
    ShootingRangeEvents GetEvents();
  }
}
