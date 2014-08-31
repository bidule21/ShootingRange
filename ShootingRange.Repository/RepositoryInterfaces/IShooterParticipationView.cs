using System.Collections.Generic;
using ShootingRange.BusinessObjects;

namespace ShootingRange.Repository.RepositoryInterfaces
{
  public interface IShooterParticipationView : IDataView<ShooterParticipationDetails>
  {
    IEnumerable<ShooterParticipationDetails> FindByShooterId(int shooterId);
  }
}
