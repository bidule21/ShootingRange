using System.Collections.Generic;
using System.Linq;
using Repository;
using ShootingRange.BusinessObjects;
using ShootingRange.Repository.Repositories;

namespace ShootingRange.Repository.FakeRepositories
{
  public class FakePersonDataStore : IPersonDataStore
  {
    private FakeRepository<Person> _repository;

    public FakePersonDataStore()
    {
      _repository = new FakeRepository<Person>();

      Person[] people = new[]
      {
        new Person() {PersonId = 1, FirstName = "Roger", LastName = "Waltenspül"},
        new Person() {PersonId = 2, FirstName = "Daniel", LastName = "Eberli"},
        new Person() {PersonId = 3, FirstName = "Hans", LastName = "Hugentobler"},
      };

      foreach (Person person in people)
      {
        _repository.Insert(person);
      }
    }

    public IEnumerable<Person> GetAll()
    {
      return _repository.GetAll();
    }

    public Person FindById(int id)
    {
      return _repository.Find(_ => _.PersonId == id).Single();
    }

    public void Revert()
    {
      throw new System.NotImplementedException();
    }

    public void Create(Person person)
    {
      _repository.Insert(person);
    }

    public void Update(Person person)
    {
      Person entity = FindById(person.PersonId);
      entity.FirstName = person.FirstName;
      entity.LastName = person.LastName;
      _repository.Commit();
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
