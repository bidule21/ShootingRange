using System.Collections.Generic;
using System.Linq;

namespace ShootingRange.Repository
{
  public interface IDataStore<TEntity, in TKey>
  {
    void Create(TEntity entity);
    TEntity FindById(TKey key);
    IEnumerable<TEntity> GetAll();
    void Update(TEntity entity);
    void Delete(TEntity entity);
  }
}
