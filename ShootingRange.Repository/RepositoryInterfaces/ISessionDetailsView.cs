using System.Collections.Generic;
using ShootingRange.BusinessObjects;

namespace ShootingRange.Repository.RepositoryInterfaces
{
  public interface ISessionDetailsView : IDataView<SessionDetails>
  {
    IEnumerable<SessionDetails> FindByShooterId(int shooterId);
  }
}
