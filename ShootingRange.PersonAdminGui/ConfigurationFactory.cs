using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using ShootingRange.BusinessObjects;

namespace ShootingRange.PersonAdminGui
{
  class ConfigurationFactory : IConfigurationFactory
  {
    public IRepository<Person> GetPersonRepository()
    {
      return null;
    }
  }

  internal interface IConfigurationFactory
  {
  }
}
