using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShootingRange.BusinessObjects;
using ShootingRange.Repository.Repositories;
using ShootingRange.Repository.RepositoryInterfaces;

namespace ShootingRange.Repository.FakeRepositories
{
  public class FakeGroupMemberDetailsView : IGroupMemberDetailsView
  {
    private HashSet<GroupMemberDetails> _set;

    public FakeGroupMemberDetailsView()
    {
      _set = new HashSet<GroupMemberDetails>();
    }

    public IEnumerable<GroupMemberDetails> GetAll()
    {
      return _set;
    }

    public IEnumerable<GroupMemberDetails> FindByGroupId(int groupId)
    {
      return _set.Where(_ => _.GroupId == groupId);
    }

    public IEnumerable<GroupMemberDetails> FindByShooterId(int shooterId)
    {
      return _set.Where(_ => _.ShooterId == shooterId);
    }

    public IEnumerable<GroupMemberDetails> FindByPersonId(int personId)
    {
      return _set.Where(_ => _.PersonId == personId);
    }
  }
}
