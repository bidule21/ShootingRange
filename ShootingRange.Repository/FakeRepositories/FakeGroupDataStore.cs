using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using ShootingRange.BusinessObjects;
using ShootingRange.Repository.Repositories;

namespace ShootingRange.Repository.FakeRepositories
{
  public class FakeGroupDataStore : IGroupDataStore
  {
    private FakeRepository<Group> _repository;

    public FakeGroupDataStore()
    {
      _repository = new FakeRepository<Group>();

      Group[] groups = new[]
      {
        new Group {GroupId = 1, GroupName = "Eichenlaub", GroupDescriptionId = 1}
      };

      foreach (var g in groups)
      {
        _repository.Insert(g);
      }
    }

    public void Create(Group group)
    {
      _repository.Insert(group);
    }

    public Group FindById(int id)
    {
      return _repository.Find(_ => _.GroupId == id).Single();
    }

    public IEnumerable<Group> GetAll()
    {
      return _repository.GetAll();
    }

    public void Update(Group group)
    {
      Group entity = FindById(group.GroupId);
      entity.GroupName = group.GroupName;
      entity.GroupDescriptionId = group.GroupDescriptionId;
      _repository.Commit();
    }

    public void Delete(Group group)
    {
      _repository.Delete(group);
    }

    public IEnumerable<Group> FindByGroupName(string groupName)
    {
      return
        _repository.Find(g => string.Compare(g.GroupName, groupName, StringComparison.OrdinalIgnoreCase) == 0);
    }

    public IEnumerable<Group> FindByGroupDescriptionId(int groupDescriptionId)
    {
      return _repository.Find(g => g.GroupDescriptionId == groupDescriptionId);
    }
  }
}
