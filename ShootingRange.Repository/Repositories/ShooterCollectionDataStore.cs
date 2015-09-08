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
  public class ShooterCollectionDataStore : IShooterCollectionDataStore
  {
    private SqlRepository<t_shootercollection> _sqlRepository;
    private Func<t_shootercollection, ShooterCollection> _selector;

    public ShooterCollectionDataStore(DbContext context)
    {
      _sqlRepository = new SqlRepository<t_shootercollection>(context);
      _selector = shooterCollection => new ShooterCollection
      {
        CollectionName = shooterCollection.CollectionName,
        ShooterCollectionId = shooterCollection.ShooterCollectionId,
        ProgramNumber = shooterCollection.ProgramNumber
      };
    }

    public void Revert()
    {
      _sqlRepository.Revert();
    }

    public void Create(ShooterCollection shooter)
    {
      t_shootercollection entity = new t_shootercollection();
      entity.UpdateEntity(shooter);
      _sqlRepository.Insert(entity);
      shooter.ShooterCollectionId = entity.ShooterCollectionId;
    }

    public ShooterCollection FindById(int id)
    {
      ShooterCollection result = _sqlRepository.Find(_ => _.ShooterCollectionId == id).Select(_selector).Single();
      return result;
    }

    public IEnumerable<ShooterCollection> GetAll()
    {
      return _sqlRepository.GetAll().ToList().Select(_selector);
    }

    public void Update(ShooterCollection shooterCollection)
    {
      t_shootercollection entity = _sqlRepository.Find(_ => _.ShooterCollectionId == shooterCollection.ShooterCollectionId).Single();
      entity.UpdateEntity(shooterCollection);
      _sqlRepository.Commit();
    }

    public void Delete(ShooterCollection shooterCollection)
    {
      t_shootercollection entity = _sqlRepository.Find(_ => _.ShooterCollectionId == shooterCollection.ShooterCollectionId).Single();
      _sqlRepository.Delete(entity);
    }

      public IEnumerable<ShooterCollection> FindByProgramNumber(int programNumber)
      {
          return _sqlRepository.Find(_ => _.ProgramNumber == programNumber).Select(_selector);
      }
  }
}
