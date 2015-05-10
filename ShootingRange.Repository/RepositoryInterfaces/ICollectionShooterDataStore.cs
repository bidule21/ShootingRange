using System.Collections.Generic;
using ShootingRange.BusinessObjects;

namespace ShootingRange.Repository.RepositoryInterfaces
{
  public interface ICollectionShooterDataStore : IDataStore<CollectionShooter, int>
  {
    IEnumerable<CollectionShooter> FindByShooterCollectionId(int shooterCollectionId);
    IEnumerable<CollectionShooter> FindByShooterId(int shooterCollectionId);
  }
}
