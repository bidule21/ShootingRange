using System.Collections.Generic;
using ShootingRange.Common;
using ShootingRange.Common.Modules;
using ShootingRange.Engine;

namespace ShootingRange.DummyRange
{
  class DummyRangeConfigurationFactory : IConfigurationFactory
  {
    private ShootingRangeEvents _events;
    private IShootingRange _shootingRange;

    public DummyRangeConfigurationFactory()
    {
      _shootingRange = new DummyRange();

      _events = new ShootingRangeEvents();
      var modules = new List<IShootingRangeModule>();
      foreach (IShootingRangeModule module in modules)
      {
        module.Initialize(_events);
      }
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
