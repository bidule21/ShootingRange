using System.Collections.Generic;
using ShootingRange.BusinessObjects;

namespace ShootingRange.Repository.RepositoryInterfaces
{
  public interface IShooterCollectionParticipationDataStore : IDataStore<ShooterCollectionParticipation, int>
  {
    IEnumerable<ShooterCollectionParticipation> FindByIdParticipationId(int participationId);
  }
}
