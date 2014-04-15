using ShootingRange.BusinessObjects;
using ShootingRange.Entities;

namespace ShootingRange.Repository.Mapper
{
  internal static class PersonMapper
  {
    public static Person MapToPerson(this t_person entity)
    {
      return new Person
      {
        PersonId = entity.PersonId,
        FirstName = entity.FirstName,
        LastName = entity.LastName
      };
    }

    public static t_person UpdateEntity(this t_person entity, Person person)
    {
      entity.FirstName = person.FirstName;
      entity.LastName = person.LastName;

      return entity;
    }
  }
}