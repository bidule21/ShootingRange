using ShootingRange.Repository.Repositories;
using ShootingRange.Repository.RepositoryInterfaces;

namespace ShootingRange.Service.Interface
{
  public interface IShooterNumberService
  {
    int GetShooterNumber();
    void Configure(IShooterDataStore shooterDataStore);
  }
}
