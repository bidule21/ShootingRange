using ShootingRange.Repository.Repositories;

namespace ShootingRange.Service.Interface
{
  public interface IShooterNumberService
  {
    int GetShooterNumber();
    void Configure(IShooterDataStore shooterDataStore);
  }
}
