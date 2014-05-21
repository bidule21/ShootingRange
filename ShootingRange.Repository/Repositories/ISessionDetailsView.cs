using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShootingRange.BusinessObjects;

namespace ShootingRange.Repository.Repositories
{
  public interface ISessionDetailsView : IDataView<SessionDetails>
  {
    IEnumerable<SessionDetails> FindByShooterId(int shooterId);
  }
}
