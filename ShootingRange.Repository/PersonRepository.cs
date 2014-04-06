using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using Repository;
using ShootingRange.BusinessObjects;

namespace ShootingRange.Repository
{
  class PersonRepository : SqlRepository<Person>
  {
    public PersonRepository() : this(null)
    {
      
    }
    public PersonRepository(ObjectContext context) : base(context)
    {
    }
  }
}
