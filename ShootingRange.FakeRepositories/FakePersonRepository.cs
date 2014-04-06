using ShootingRange.BusinessObjects;

namespace ShootingRange.FakeRepositories
{
  public class FakePersonRepository : FakeRepository<Person>
  {
    public FakePersonRepository()
    {
      Add(new Person {Id = 0, FirstName = "Roger", LastName = "Waltenspül"});
      Add(new Person {Id = 1, FirstName = "Daniel", LastName = "Eberli"});
    }
  }
}