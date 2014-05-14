using System.Collections.Generic;
using ShootingRange.BusinessObjects;

namespace ShootingRange.Repository.Repositories
{
  public interface IParticipationDataStore : IDataStore<Participation, int>
  {
    IEnumerable<Participation> FindByParticipationName(string participationName);
    IEnumerable<Participation> FindByParticipationDescriptionId(int participationDescriptionId);
  }
}
