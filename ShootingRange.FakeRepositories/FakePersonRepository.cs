using Repository;
using ShootingRange.BusinessObjectContracts;
using ShootingRange.BusinessObjects;
using ShootingRange.Entities;

namespace ShootingRange.FakeRepositories
{
  public class FakePersonRepository : FakeRepository<Person>
  {
    public FakePersonRepository()
    {
      Insert(new Person {Id = 0, FirstName = "Roger", LastName = "Waltenspül"});
      Insert(new Person { Id = 1, FirstName = "Daniel", LastName = "Eberli" });
    }
  }
}