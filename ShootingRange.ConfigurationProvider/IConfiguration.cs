using ShootingRange.Common;
using ShootingRange.Common.Modules;
using ShootingRange.Repository;
using ShootingRange.Service.Interface;

namespace ShootingRange.ConfigurationProvider
{
  public interface IConfiguration
  {
    IShootingRange GetShootingRange();
    ShootingRangeEvents GetEvents();
    UIEvents GetUIEvents();
    IPersonDataStore GetPersonRepository();
    IWindowService GetWindowService();
  }
}
