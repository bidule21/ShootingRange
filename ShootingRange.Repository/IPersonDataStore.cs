using System.Collections.Generic;
using System.Linq;
using ShootingRange.BusinessObjects;

namespace ShootingRange.Repository
{
  public interface IPersonDataStore : IDataStore<Person, int>
  {
    IEnumerable<Person> FindByLastName(string partialName);

    IEnumerable<Person> FindByFirstName(string partialName);
  }
}
