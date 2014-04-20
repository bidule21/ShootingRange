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
    private HashSet<GroupDetails> _set;

    public FakeGroupDetailsView()
    {
      _set = new HashSet<GroupDetails>();
    }

    public IEnumerable<GroupDetails> GetAll()
    {
      return _set;
    }

    public IEnumerable<GroupDetails> FindByShooterId(int shooterId)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<GroupDetails> FindByPersonId(int personId)
    {
      throw new NotImplementedException();
    }
  }
}
