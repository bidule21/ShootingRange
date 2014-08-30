using System.Collections.Generic;
using ShootingRange.BusinessObjects;

namespace ShootingRange.Repository.Repositories
{
  public interface ISessionDataStore : IDataStore<Session, int>
  {
    IEnumerable<Session> FindByShooterId(int shooterId);
  }
}
