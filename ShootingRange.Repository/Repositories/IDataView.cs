using System.Collections.Generic;

namespace ShootingRange.Repository.Repositories
{
  public interface IDataView<TEntity>
  {
    IEnumerable<TEntity> GetAll();
  }
}
