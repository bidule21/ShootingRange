using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShootingRange.BusinessObjects;
using ShootingRange.Entities;

namespace ShootingRange.Repository.Repositories
{
  public interface IProgramItemDataStore : IDataStore<ProgramItem, int>
  {
    ProgramItem FindByProgramNumber(int programNumber);
  }
}
