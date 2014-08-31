using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Repository;
using ShootingRange.BusinessObjects;
using ShootingRange.Entities;
using ShootingRange.Repository.Mapper;
using ShootingRange.Repository.RepositoryInterfaces;

namespace ShootingRange.Repository.Repositories
{
  public class ParticipationDataStore : IParticipationDataStore
  {
    private SqlRepository<t_participation> _sqlRepository;
    private Func<t_participation, Participation> selector;

    public ParticipationDataStore(DbContext context)
    {
      _sqlRepository = new SqlRepository<t_participation>(context);
      selector = g => new Participation
      {
        ParticipationId = g.ParticipationId,
        ParticipationName = g.Description,
      };
    }
    
    public IEnumerable<Participation> FindByParticipationName(string participationName)
    {
      return _sqlRepository.Find(g => g.Description == participationName).ToList().Select(selector);
    }

    public void Revert()
    {
      _sqlRepository.Revert();
    }

    public void Create(Participation participation)
    {
      t_participation entity = new t_participation();
      entity.UpdateEntity(participation);
      _sqlRepository.Insert(entity);
      participation.ParticipationId = entity.ParticipationId;
    }

    public Participation FindById(int id)
    {
      return _sqlRepository.Find(_ => _.ParticipationId == id).Select(selector).Single();
    }

    public IEnumerable<Participation> GetAll()
    {
      return _sqlRepository.GetAll().ToList().Select(selector);
    }

    public void Update(Participation participation)
    {
      t_participation entity = _sqlRepository.Find(_ => _.ParticipationId == participation.ParticipationId).Single();
      entity.UpdateEntity(participation);
      _sqlRepository.Commit();
    }

    public void Delete(Participation participation)
    {
      t_participation entity = _sqlRepository.Find(_ => _.ParticipationId == participation.ParticipationId).Single();
      _sqlRepository.Delete(entity);
    }
  }
}
