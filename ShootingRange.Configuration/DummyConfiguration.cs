using ShootingRange.Common;
using ShootingRange.Common.Modules;
using ShootingRange.ConfigurationProvider;
using ShootingRange.Repository;
using ShootingRange.Service;
using ShootingRange.Service.Interface;

namespace ShootingRange.Configuration
{
  public class DummyConfiguration : IConfiguration
  {
    private ShootingRangeEvents _events;
    private UIEvents _uiEvents;
    private IWindowService _windowService;

    public DummyConfiguration()
    {
      _events = new ShootingRangeEvents();
      _uiEvents = new UIEvents();
      _windowService = new WindowService();
    }

    public IShootingRange GetShootingRange()
    {
      throw new System.NotImplementedException();
    }

    public ShootingRangeEvents GetEvents()
    {
      return _events;
    }

    public UIEvents GetUIEvents()
    {
      return _uiEvents;
    }

    public IPersonDataStore GetPersonRepository()
    {
      return new FakePersonDataStore();
    }

    public IWindowService GetWindowService()
    {
      return _windowService;
    }
  }
}