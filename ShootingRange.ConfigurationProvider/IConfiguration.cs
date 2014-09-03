using ShootingRange.Common;
using ShootingRange.Common.Modules;
using ShootingRange.Repository.RepositoryInterfaces;
using ShootingRange.Service.Interface;

namespace ShootingRange.ConfigurationProvider
{
  public interface IConfiguration
  {
    ISsvShooterDataWriterService GetSsvShooterDataWriterService();
    IShootingRange GetShootingRange();
    ShootingRangeEvents GetEvents();
    UIEvents GetUIEvents();
    IPersonDataStore GetPersonDataStore();
    IShooterDataStore GetShooterDataStore();
    IParticipationDataStore GetParticipationDataStore();
    ISessionSubtotalDataStore GetSessionSubtotalDataStore();
    IGroupMemberDetailsView GetGroupMemberDetailsView();
    IGroupDetailsView GetGroupDetailsView();
    ISessionDetailsView GetSessionDetailsView();
    IWindowService GetWindowService();
    IShooterNumberService GetShooterNumberService();
    IShooterParticipationDataStore GetShooterParticipationDataStore();
    IShooterParticipationView GetShooterParticipationView();
    IBarcodePrintService GetBarcodePrintService();
    IBarcodeBuilderService GetBarcodeBuilderService();
    ISessionDataStore GetSessionDataStore();
    IProgramItemDataStore GetProgramItemDataStore();
    IShotDataStore GetShotDataStore();
    IShooterCollectionDataStore GetShooterCollectionDataStore();
    IShooterCollectionParticipationDataStore GetShooterCollectionParticipationDataStore();
    ICollectionShooterDataStore GetCollectionShooterDataStore();
  }
}
