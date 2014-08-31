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
  public class SessionSubtotalDataStore : ISessionSubtotalDataStore
  {
    private SqlRepository<t_sessionsubtotal> _sqlRepository;
    private Func<t_sessionsubtotal, SubSession> _selector;

    public SessionSubtotalDataStore(DbContext context)
    {
      _sqlRepository = new SqlRepository<t_sessionsubtotal>(context);
      _selector = sessionSubtotal => new SubSession
      {
        SessionSubtotalId = sessionSubtotal.SessionSubtotalId,
        SessionId = sessionSubtotal.SessionId,
        Ordinal = sessionSubtotal.SubtotalOrdinal,
        BestShotId = sessionSubtotal.BestShotId
      };
    }

    public void Revert()
    {
      _sqlRepository.Revert();
    }

    public void Create(SubSession sessionSubtotal)
    {
      t_sessionsubtotal entity = new t_sessionsubtotal();
      entity.UpdateEntity(sessionSubtotal);
      _sqlRepository.Insert(entity);
      sessionSubtotal.SessionSubtotalId = entity.SessionSubtotalId;
    }

    public SubSession FindById(int id)
    {
      return _sqlRepository.Find(_ => _.SessionSubtotalId == id).Select(_selector).Single();
    }

    public IEnumerable<SubSession> GetAll()
    {
      return _sqlRepository.GetAll().ToList().Select(_selector);
    }

    public void Update(SubSession sessionSubtotal)
    {
      t_sessionsubtotal entity =
        _sqlRepository.Find(_ => _.SessionSubtotalId == sessionSubtotal.SessionSubtotalId).Single();
      entity.UpdateEntity(sessionSubtotal);
      _sqlRepository.Commit();
    }

    public void Delete(SubSession sessionSubtotal)
    {
      t_sessionsubtotal entity =
        _sqlRepository.Find(_ => _.SessionSubtotalId == sessionSubtotal.SessionSubtotalId).Single();
      _sqlRepository.Delete(entity);
    }
  }
}
