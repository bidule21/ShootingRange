using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShootingRange.BusinessObjects;
using ShootingRange.Repository.Repositories;

namespace ShootingRange.Repository.FakeRepositories
{
  public class FakeGroupDetailsView : IGroupDetailsView
  {
    private HashSet<ParticipationDetails> _set;

    public FakeGroupDetailsView()
    {
      _set = new HashSet<ParticipationDetails>();
    }

    public IEnumerable<ParticipationDetails> GetAll()
    {
      return _set;
    }

    public IEnumerable<ParticipationDetails> FindByShooterId(int shooterId)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<ParticipationDetails> FindByPersonId(int personId)
    {
      throw new NotImplementedException();
    }
  }
}
