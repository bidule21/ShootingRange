using ShootingRange.Common;
using ShootingRange.Common.Modules;
using ShootingRange.ConfigurationProvider;
using ShootingRange.Repository;
using ShootingRange.Repository.FakeRepositories;
using ShootingRange.Repository.Repositories;
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

    public IPersonDataStore GetPersonDataStore()
    {
      return new FakePersonDataStore();
    }

    public IShooterDataStore GetShooterDataStore()
    {
      return new FakeShooterDataStore();
    }

    public IParticipationDataStore GetParticipationDataStore()
    {
      return new FakeParticipationDataStore();
    }

    public IGroupMemberDetailsView GetGroupMemberDetailsView()
    {
      return new FakeGroupMemberDetailsView();
    }

    public IGroupDetailsView GetGroupDetailsView()
    {
      return new FakeGroupDetailsView();
    }

    public IWindowService GetWindowService()
    {
      return _windowService;
    }

    public IShooterNumberService GetShooterNumberService()
    {
      return new FakeShooterNumberService();
    }

    public IShooterParticipationDataStore GetShooterParticipationDataStore()
    {
      throw new System.NotImplementedException();
    }

    public IShooterParticipationView GetShooterParticipationView()
    {
      throw new System.NotImplementedException();
    }

    public IBarcodePrintService GetBarcodePrintService()
    {
      throw new System.NotImplementedException();
    }

    public IBarcodeBuilderService GetBarcodeBuilderService()
    {
      throw new System.NotImplementedException();
    }
  }
}