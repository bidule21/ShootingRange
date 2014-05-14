using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShootingRange.BusinessObjects;

namespace ShootingRange.Repository.Repositories
{
  public interface IGroupDetailsView : IDataView<ParticipationDetails>
  {
    IEnumerable<ParticipationDetails> FindByShooterId(int shooterId);
    IEnumerable<ParticipationDetails> FindByPersonId(int personId);
  }
}
