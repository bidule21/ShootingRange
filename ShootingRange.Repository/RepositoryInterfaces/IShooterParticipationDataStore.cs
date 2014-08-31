using System.Collections.Generic;
using ShootingRange.BusinessObjects;

namespace ShootingRange.Repository.RepositoryInterfaces
{
  public interface IShooterParticipationDataStore : IDataStore<ShooterParticipation, int>
  {
    IEnumerable<ShooterParticipation> FindByShooterId(int shooterId);
    IEnumerable<ShooterParticipation> FindByParticipationId(int participationId);
  }
}
