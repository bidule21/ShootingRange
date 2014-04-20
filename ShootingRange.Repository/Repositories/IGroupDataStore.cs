using System.Collections.Generic;
using ShootingRange.BusinessObjects;

namespace ShootingRange.Repository.Repositories
{
  public interface IGroupDataStore : IDataStore<Group, int>
  {
    IEnumerable<Group> FindByGroupName(string groupName);
    IEnumerable<Group> FindByGroupDescriptionId(int groupDescriptionId);
  }
}
