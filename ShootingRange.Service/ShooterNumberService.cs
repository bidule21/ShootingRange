using ShootingRange.BusinessObjects;
using ShootingRange.Repository.Repositories;
using ShootingRange.Service.Interface;

namespace ShootingRange.Service
{
  public class ShooterNumberService : IShooterNumberService
  {
    private readonly IShooterNumberConfigDataStore _shooterNumberConfigDataStore;
    private const int ConfigId = 1;

    public ShooterNumberService(IShooterNumberConfigDataStore shooterNumberConfigDataStore)
    {
      _shooterNumberConfigDataStore = shooterNumberConfigDataStore;
    }

    public int GetShooterNumber()
    {
      ShooterNumberConfig config = _shooterNumberConfigDataStore.FindById(ConfigId);
      int nextShooterNumber = config.LastGivenShooterNumber + config.ShooterNumberIncrement;
      config.LastGivenShooterNumber = nextShooterNumber;
      _shooterNumberConfigDataStore.Update(config);
      return nextShooterNumber;
    }
  }
}
