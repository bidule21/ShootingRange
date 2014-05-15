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
    IParticipationDataStore GetParticipationDataStore();
    IGroupMemberDetailsView GetGroupMemberDetailsView();
    IGroupDetailsView GetGroupDetailsView();
    IWindowService GetWindowService();
    IShooterNumberService GetShooterNumberService();
    IShooterParticipationDataStore GetShooterParticipationDataStore();
    IShooterParticipationView GetShooterParticipationView();
    IBarcodePrintService GetBarcodePrintService();
    IBarcodeBuilderService GetBarcodeBuilderService();
  }
}
