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
      entity.Street = person.Address;
      entity.Zip = person.ZipCode;
      entity.City = person.City;
      entity.Email = person.Email;
      entity.Phone = person.Phone;
      entity.BirthDate = person.DateOfBirth;
      return entity;
    }
  }
}