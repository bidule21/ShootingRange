using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Repository;
using ShootingRange.BusinessObjects;
using ShootingRange.Entities;
using ShootingRange.Repository.Mapper;
using ShootingRange.Repository.RepositoryInterfaces;

namespace ShootingRange.Repository.Repositories
{
  public class PersonDataStore : IPersonDataStore
  {
    private SqlRepository<t_person> _sqlRepository;
    private Func<t_person, Person> _selector;

    public PersonDataStore(DbContext context)
    {
      _sqlRepository = new SqlRepository<t_person>(context);
      _selector = person => new Person
      {
        PersonId = person.PersonId,
        FirstName = person.FirstName,
        LastName = person.LastName,
        Address = person.Street,
        ZipCode = person.Zip,
        City = person.City,
        Email =  person.Email,
        Phone = person.Phone,
        DateOfBirth = person.BirthDate,
      };
    }

    public IEnumerable<Person> GetAll()
    {
      return _sqlRepository.GetAll().ToList().Select(_selector);
    }

    public Person FindById(int id)
    {
      return _sqlRepository.Find(_ => _.PersonId == id).Select(_selector).Single();
    }

    public void Revert()
    {
      _sqlRepository.Revert();
    }

    public void Create(Person person)
    {
      t_person entity = new t_person();
      entity.UpdateEntity(person);
      _sqlRepository.Insert(entity);
      person.PersonId = entity.PersonId;
    }

    public void Update(Person person)
    {
      t_person entity = _sqlRepository.Find(_ => _.PersonId == person.PersonId).Single();
      entity.UpdateEntity(person);
      _sqlRepository.Commit();
    }

    public void Delete(Person person)
    {
      t_person entity = _sqlRepository.Find(_ => _.PersonId == person.PersonId).Single();
      _sqlRepository.Delete(entity);
    }

    public IEnumerable<Person> FindByLastName(string partialName)
    {
      return _sqlRepository.Find(person => person.LastName.Contains(partialName)).Select(_selector);
    }

    public IEnumerable<Person> FindByFirstName(string partialName)
    {
      return _sqlRepository.Find(person => person.FirstName.Contains(partialName)).Select(_selector);
    }
  }
}
