using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using ShootingRange.BusinessObjects;
using ShootingRange.Entities;
using ShootingRange.Repository.Mapper;
using ShootingRange.Repository.RepositoryInterfaces;

namespace ShootingRange.Repository.Repositories
{
  public class ShotDataStore : IShotDataStore
  {
    private SqlRepository<t_shot> _sqlRepository;
    private Func<t_shot, Shot> _selector;
    public ShotDataStore(DbContext context)
    {
      _sqlRepository = new SqlRepository<t_shot>(context);
      _selector = shot => new Shot
    {
      ShotId = shot.ShotId,
      PrimaryScore = shot.PrimaryScore,
      SecondaryScore = shot.SecondaryScore,
      Ordinal = shot.ShotOrdinal,
      ValueX = shot.ValueX,
      ValueY = shot.ValueY,
    };
    }
   
    public void Revert()
    {
      _sqlRepository.Revert();
    }

    public void Create(Shot shot)
    {
      t_shot entity = new t_shot();
      entity.UpdateEntity(shot);
      _sqlRepository.Insert(entity);
      shot.ShotId = entity.ShotId;
    }

    public Shot FindById(int id)
    {
      return _sqlRepository.Find(_ => _.ShotId == id).Select(_selector).Single();
    }

    public IEnumerable<Shot> GetAll()
    {
      return _sqlRepository.GetAll().ToList().Select(_selector);
    }

    public void Update(Shot shot)
    {
      t_shot entity = _sqlRepository.Find(_ => _.ShotId == shot.ShotId).Single();
      entity.UpdateEntity(shot);
      _sqlRepository.Commit();
    }

    public void Delete(Shot shot)
    {
      t_shot entity = _sqlRepository.Find(_ => _.ShotId == shot.ShotId).Single();
      _sqlRepository.Delete(entity);
    }

    public IEnumerable<Shot> FindBySubSessionId(int subSessionId)
    {
      return _sqlRepository.Find(shot => shot.SubtotalId == subSessionId).Select(_selector);
    }
  }
}
