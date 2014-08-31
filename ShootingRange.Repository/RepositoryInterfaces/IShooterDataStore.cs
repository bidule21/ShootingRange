using System.Collections.Generic;
using ShootingRange.BusinessObjects;

namespace ShootingRange.Repository.RepositoryInterfaces
{
  public interface IShooterDataStore : IDataStore<Shooter, int>
  {
    Shooter FindByShooterNumber(int shooterNumber);
    IEnumerable<Shooter> FindByPersonId(int personId);
  }
}
