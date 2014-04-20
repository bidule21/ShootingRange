using ShootingRange.BusinessObjects;
using ShootingRange.Entities;

namespace ShootingRange.Repository.Mapper
{
  internal static class PersonMapper
  {
    public static t_person UpdateEntity(this t_person entity, Person person)
    {
      entity.FirstName = person.FirstName;
      entity.LastName = person.LastName;

      return entity;
    }
  }
}