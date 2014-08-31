using System.Collections.Generic;
using ShootingRange.BusinessObjects;

namespace ShootingRange.Repository.RepositoryInterfaces
{
  public interface IGroupDetailsView : IDataView<ParticipationDetails>
  {
    IEnumerable<ParticipationDetails> FindByShooterId(int shooterId);
    IEnumerable<ParticipationDetails> FindByPersonId(int personId);
  }
}
