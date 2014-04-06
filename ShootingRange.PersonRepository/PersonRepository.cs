using System.Data.Objects;
using Repository;
using ShootingRange.BusinessObjects;

namespace ShootingRange.PersonRepository
{
  public class PersonRepository : SqlRepository<Person>
  {
    public PersonRepository(ObjectContext context) : base(context)
    {
    }
  }
}
