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
  public class ShooterCollectionParticipationDataStore : IShooterCollectionParticipationDataStore
  {
    private SqlRepository<t_shootercollectionparticipation> _sqlRepository;
    private Func<t_shootercollectionparticipation, ShooterCollectionParticipation> _selector;

    public ShooterCollectionParticipationDataStore(DbContext context)
    {
      _sqlRepository = new SqlRepository<t_shootercollectionparticipation>(context);
      _selector = colletionShooter => new ShooterCollectionParticipation
      {
        ShooterCollectionParticipationId = colletionShooter.ShooterCollectionParticipationId,
        ParticipationId = colletionShooter.ParticipationId,
        ShooterCollectionId = colletionShooter.ShooterCollectionId
      };
    }

    public void Revert()
    {
      _sqlRepository.Revert();
    }

    public void Create(ShooterCollectionParticipation shooter)
    {
      t_shootercollectionparticipation entity = new t_shootercollectionparticipation();
      entity.UpdateEntity(shooter);
      _sqlRepository.Insert(entity);
      shooter.ShooterCollectionParticipationId = entity.ShooterCollectionParticipationId;
    }

    public ShooterCollectionParticipation FindById(int id)
    {
      return _sqlRepository.Find(_ => _.ShooterCollectionParticipationId == id).Select(_selector).Single();
    }

    public IEnumerable<ShooterCollectionParticipation> GetAll()
    {
      return _sqlRepository.GetAll().ToList().Select(_selector);
    }

    public void Update(ShooterCollectionParticipation collectionShooter)
    {
      t_shootercollectionparticipation entity = _sqlRepository.Find(_ => _.ShooterCollectionParticipationId == collectionShooter.ShooterCollectionParticipationId).Single();
      entity.UpdateEntity(collectionShooter);
      _sqlRepository.Commit();
    }

    public void Delete(ShooterCollectionParticipation collectionShooter)
    {
      t_shootercollectionparticipation entity = _sqlRepository.Find(_ => _.ShooterCollectionParticipationId == collectionShooter.ShooterCollectionParticipationId).Single();
      _sqlRepository.Delete(entity);
    }
  }
}
