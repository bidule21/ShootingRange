using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using ShootingRange.BusinessObjects;
using ShootingRange.Entities;
using ShootingRange.Repository.Mapper;

namespace ShootingRange.Repository.Repositories
{
  public class GroupDataStore : IGroupDataStore
  {
    private SqlRepository<t_group> _sqlRepository;
    private Func<t_group, Group> selector;

    public GroupDataStore(DbContext context)
    {
      _sqlRepository = new SqlRepository<t_group>(context);
      selector = g => new Group
      {
        GroupId = g.GroupId,
        GroupName = g.GroupName,
        GroupDescriptionId = g.GroupDescriptionId
      };
    }
    
    public IEnumerable<Group> FindByGroupName(string groupName)
    {
      return _sqlRepository.Find(g => g.GroupName == groupName).ToList().Select(selector);
    }

    public IEnumerable<Group> FindByGroupDescriptionId(int groupDescriptionId)
    {
      return _sqlRepository.Find(g => g.GroupDescriptionId == groupDescriptionId).ToList().Select(selector);
    }

    public void Create(Group group)
    {
      t_group entity = new t_group();
      entity.UpdateEntity(group);
      _sqlRepository.Insert(entity);
    }

    public Group FindById(int id)
    {
      return _sqlRepository.Find(_ => _.GroupId == id).Select(selector).Single();
    }

    public IEnumerable<Group> GetAll()
    {
      return _sqlRepository.GetAll().ToList().Select(selector);
    }

    public void Update(Group group)
    {
      t_group entity = _sqlRepository.Find(_ => _.GroupId == group.GroupId).Single();
      entity.UpdateEntity(group);
      _sqlRepository.Commit();
    }

    public void Delete(Group group)
    {
      t_group entity = _sqlRepository.Find(_ => _.GroupId == group.GroupId).Single();
      _sqlRepository.Delete(entity);
    }
  }
}
