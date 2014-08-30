using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Repository;
using ShootingRange.BusinessObjects;
using ShootingRange.Entities;
using ShootingRange.Repository.Mapper;

namespace ShootingRange.Repository.Repositories
{
  public class SessionDataStore : ISessionDataStore
  {
    private SqlRepository<t_session> _sqlRepository;
    private Func<t_session, Session> _selector;

    public SessionDataStore(DbContext context)
    {
      _sqlRepository = new SqlRepository<t_session>(context);
      _selector = session => new Session
      {
        SessionId = session.SessionId,
        ProgramItemId = session.ProgramItemId,
        LaneNumber = session.LaneNumber,
        ShooterId = session.ShooterId,
      };
    }

    public void Revert()
    {
      _sqlRepository.Revert();
    }

    public void Create(Session session)
    {
      t_session entity = new t_session();
      entity.UpdateEntity(session);
      _sqlRepository.Insert(entity);
      session.SessionId = entity.SessionId;
    }

    public Session FindById(int id)
    {
      return _sqlRepository.Find(_ => _.ShooterId == id).Select(_selector).Single();
    }

    public IEnumerable<Session> GetAll()
    {
      return _sqlRepository.GetAll().ToList().Select(_selector);
    }

    public void Update(Session session)
    {
      t_session entity = _sqlRepository.Find(_ => _.SessionId == session.SessionId).Single();
      entity.UpdateEntity(session);
      _sqlRepository.Commit();
    }

    public void Delete(Session session)
    {
      t_session entity = _sqlRepository.Find(_ => _.SessionId == session.SessionId).Single();
      _sqlRepository.Delete(entity);
    }

    public IEnumerable<Session> FindByShooterId(int shooterId)
    {
      return _sqlRepository.Find(_ => _.ShooterId == shooterId).Select(_selector);
    }
  }
}
