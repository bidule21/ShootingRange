using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Repository;
using ShootingRange.BusinessObjects;
using ShootingRange.Entities;
using ShootingRange.Repository.Mapper;

namespace ShootingRange.Repository
{
  public class PersonDataStore : IPersonDataStore
  {
    private SqlRepository<t_person> _sqlRepository;

    public PersonDataStore(DbContext context)
    {
      _sqlRepository = new SqlRepository<t_person>(context);
    }

    public IEnumerable<Person> GetAll()
    {
      return _sqlRepository.GetAll().ToList().Select(_ => _.MapToPerson());
    }

    public Person FindById(int id)
    {
      return _sqlRepository.Find(_ => _.PersonId == id).Single().MapToPerson();
    }

    public void Create(Person person)
    {
      t_person entity = new t_person();
      entity.UpdateEntity(person);
      _sqlRepository.Insert(entity);
    }

    public void Update(Person person)
    {
      t_person entity = _sqlRepository.Find(_ => _.PersonId == person.PersonId).Single();
      entity.UpdateEntity(person);
      _sqlRepository.Update(entity);
    }

    public void Delete(Person person)
    {
      t_person entity = _sqlRepository.Find(_ => _.PersonId == person.PersonId).Single();
      _sqlRepository.Delete(entity);
    }

    public IEnumerable<Person> FindByLastName(string partialName)
    {
      return _sqlRepository.Find(person => person.LastName.Contains(partialName)).ToList().Select(_ => _.MapToPerson());
    }

    public IEnumerable<Person> FindByFirstName(string partialName)
    {
      return _sqlRepository.Find(person => person.FirstName.Contains(partialName)).ToList().Select(_ => _.MapToPerson());
    }
  }
}
