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
  public class CollectionShooterDataStore : ICollectionShooterDataStore
  {
    private SqlRepository<t_collectionshooter> _sqlRepository;
    private Func<t_collectionshooter, CollectionShooter> _selector;

    public CollectionShooterDataStore(DbContext context)
    {
      _sqlRepository = new SqlRepository<t_collectionshooter>(context);
      _selector = colletionShooter => new CollectionShooter
      {
        ShooterId = colletionShooter.ShooterId,
        CollectionShooterId = colletionShooter.CollectionShooterId,
        ShooterCollectionId = colletionShooter.ShooterCollectionId
      };
    }

    public void Revert()
    {
      _sqlRepository.Revert();
    }

    public void Create(CollectionShooter shooter)
    {
      t_collectionshooter entity = new t_collectionshooter();
      entity.UpdateEntity(shooter);
      _sqlRepository.Insert(entity);
      shooter.CollectionShooterId = entity.CollectionShooterId;
    }

    public CollectionShooter FindById(int id)
    {
      return _sqlRepository.Find(_ => _.CollectionShooterId == id).Select(_selector).Single();
    }

    public IEnumerable<CollectionShooter> GetAll()
    {
      return _sqlRepository.GetAll().ToList().Select(_selector);
    }

    public void Update(CollectionShooter collectionShooter)
    {
      t_collectionshooter entity = _sqlRepository.Find(_ => _.CollectionShooterId == collectionShooter.CollectionShooterId).Single();
      entity.UpdateEntity(collectionShooter);
      _sqlRepository.Commit();
    }

    public void Delete(CollectionShooter collectionShooter)
    {
      t_collectionshooter entity = _sqlRepository.Find(_ => _.CollectionShooterId == collectionShooter.CollectionShooterId).Single();
      _sqlRepository.Delete(entity);
    }

    public IEnumerable<CollectionShooter> FindByShooterCollectionId(int shooterCollectionId)
    {
      return _sqlRepository.Find(_ => _.ShooterCollectionId == shooterCollectionId).Select(_selector);
    }
  }
}
