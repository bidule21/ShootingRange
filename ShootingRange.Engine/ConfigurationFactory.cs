using ShootingRange.Common;
using ShootingRange.Common.Modules;
using ShootingRange.SiusData;

namespace ShootingRange.Engine
{
  public class ConfigurationFactory : IConfigurationFactory
  {
    private IShootingRange _shootingRange;
    private ShootingRangeEvents _events;

    public ConfigurationFactory()
    {
      _shootingRange = new SiusDataFileProvider(@"20130914_132912.log");
      //_shootingRange = new SiusApiProvider("http://192.168.1.4");
      _events = new ShootingRangeEvents();
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
