using System.Collections.Generic;
using ShootingRange.BusinessObjects;

namespace ShootingRange.Repository.RepositoryInterfaces
{
  public interface IShotDataStore : IDataStore<Shot, int>
  {
    IEnumerable<Shot> FindBySubSessionId(int subSessionId);
  }
}
