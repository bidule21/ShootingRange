using System.Collections.Generic;

namespace ShootingRange.Repository.RepositoryInterfaces
{
  public interface IDataView<TEntity>
  {
    IEnumerable<TEntity> GetAll();
  }
}
