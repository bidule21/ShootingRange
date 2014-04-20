using ShootingRange.BusinessObjects;
using ShootingRange.Entities;

namespace ShootingRange.Repository.Mapper
{
  internal static class GroupMapper
  {
    public static t_group UpdateEntity(this t_group entity, Group person)
    {
      entity.GroupName = person.GroupName;
      entity.GroupDescriptionId = person.GroupDescriptionId;

      return entity;
    }
  }
}
