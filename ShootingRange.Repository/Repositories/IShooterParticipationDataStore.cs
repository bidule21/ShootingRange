using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShootingRange.BusinessObjects;

namespace ShootingRange.Repository.Repositories
{
  public interface IShooterParticipationDataStore : IDataStore<ShooterParticipation, int>
  {
    IEnumerable<ShooterParticipation> FindByShooterId(int shooterId);
  }
}
