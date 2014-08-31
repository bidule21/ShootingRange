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
  public class ShooterNumberConfigDataStore : IShooterNumberConfigDataStore
  {
    private SqlRepository<t_shooternumberconfig> _sqlRepository;
    private Func<t_shooternumberconfig, ShooterNumberConfig> _selector;

    public ShooterNumberConfigDataStore(DbContext context)
    {
      _sqlRepository = new SqlRepository<t_shooternumberconfig>(context);
      _selector = shooterNumberConfig => new ShooterNumberConfig()
      {
        ShooterNumberConfigId =  shooterNumberConfig.ShooterNumberConfigId,
        LastGivenShooterNumber = shooterNumberConfig.LastGivenShooterNumber,
        ShooterNumberIncrement = shooterNumberConfig.ShooterNumberIncrement
      };
    }

    public void Revert()
    {
      _sqlRepository.Revert();
    }

    public void Create(ShooterNumberConfig entity)
    {
      throw new NotImplementedException();
    }

    public ShooterNumberConfig FindById(int id)
    {
      return _sqlRepository.Find(_ => _.ShooterNumberConfigId == id).Select(_selector).Single();
    }

    public IEnumerable<ShooterNumberConfig> GetAll()
    {
      throw new NotImplementedException();
    }

    public void Update(ShooterNumberConfig shooterNumberConfig)
    {
      t_shooternumberconfig entity =
        _sqlRepository.Find(_ => _.ShooterNumberConfigId == shooterNumberConfig.ShooterNumberConfigId).Single();
      entity.UpdateEntity(shooterNumberConfig);
      _sqlRepository.Commit();
    }

    public void Delete(ShooterNumberConfig entity)
    {
      throw new NotImplementedException();
    }
  }
}
