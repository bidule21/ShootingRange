using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShootingRange.BusinessObjects;

namespace ShootingRange.Repository.Repositories
{
  public interface IShooterNumberConfigDataStore : IDataStore<ShooterNumberConfig, int>
  {
  }
}
