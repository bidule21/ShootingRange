using System.Collections.Generic;

namespace ShootingRange.Repository.Repositories
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
