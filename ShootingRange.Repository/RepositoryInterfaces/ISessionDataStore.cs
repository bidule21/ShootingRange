using System.Collections.Generic;
using ShootingRange.BusinessObjects;

namespace ShootingRange.Repository.RepositoryInterfaces
{
  public interface ISessionDataStore : IDataStore<Session, int>
  {
    IEnumerable<Session> FindByShooterId(int shooterId);
  }
}
