using ShootingRange.Common;
using ShootingRange.Common.Modules;
using ShootingRange.Repository;
using ShootingRange.Repository.Repositories;
using ShootingRange.Service.Interface;

namespace ShootingRange.ConfigurationProvider
{
  public interface IConfiguration
  {
    IShootingRange GetShootingRange();
    ShootingRangeEvents GetEvents();
    UIEvents GetUIEvents();
    IPersonDataStore GetPersonDataStore();
    IShooterDataStore GetShooterDataStore();
    IGroupDataStore GetGroupDataStore();
    IGroupMemberDetailsView GetGroupMemberDetailsView();
    IGroupDetailsView GetGroupDetailsView();
    IWindowService GetWindowService();
    IShooterNumberService GetShooterNumberService();
  }
}
