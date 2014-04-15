using System.Collections.Generic;
using System.Linq;
using Repository;
using ShootingRange.BusinessObjects;

namespace ShootingRange.Repository
{
  public class FakePersonDataStore : IPersonDataStore
  {
    private FakeRepository<Person> _repository;

    public FakePersonDataStore()
    {
      _repository = new FakeRepository<Person>();
      _repository.Insert(new Person() {FirstName = "Waltenspül", LastName = "Roger"});
    }

    public IEnumerable<Person> GetAll()
    {
      return _repository.GetAll();
    }

    public Person FindById(int id)
    {
      return _repository.Find(_ => _.PersonId == id).Single();
    }

    public void Create(Person person)
    {
      _repository.Insert(person);
    }

    public void Update(Person person)
    {
      _repository.Update(person);
    }

    public void Delete(Person person)
    {
      _repository.Delete(person);
    }

    public IEnumerable<Person> FindByLastName(string partialName)
    {
      return _repository.Find(person => person.LastName.Contains(partialName));
    }

    public IEnumerable<Person> FindByFirstName(string partialName)
    {
      return _repository.Find(person => person.FirstName.Contains(partialName));
    }
  }
}
