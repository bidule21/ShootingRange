using System.Collections.Generic;
using ShootingRange.BusinessObjects;

namespace ShootingRange.Repository.RepositoryInterfaces
{
  public interface IParticipationDataStore : IDataStore<Participation, int>
  {
    IEnumerable<Participation> FindByParticipationName(string participationName);
  }
}
