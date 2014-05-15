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

namespace ShootingRange.Repository.Repositories
{
  public class ShooterDataStore : IShooterDataStore
  {
    private SqlRepository<t_shooter> _sqlRepository;
    private Func<t_shooter, Shooter> _selector;

    public ShooterDataStore(DbContext context)
    {
      _sqlRepository = new SqlRepository<t_shooter>(context);
      _selector = shooter => new Shooter
      {
        ShooterId = shooter.ShooterId,
        ShooterNumber = shooter.ShooterNumber,
        PersonId = shooter.PersonId,
      };
    }

    public void Revert()
    {
      _sqlRepository.Revert();
    }
    
    public void Create(Shooter shooter)
    {
      t_shooter entity = new t_shooter();
      entity.UpdateEntity(shooter);
      _sqlRepository.Insert(entity);
      shooter.ShooterId = entity.ShooterId;
    }

    public Shooter FindById(int id)
    {
      return _sqlRepository.Find(_ => _.ShooterId == id).Select(_selector).Single();
    }

    public IEnumerable<Shooter> GetAll()
    {
      return _sqlRepository.GetAll().ToList().Select(_selector);
    }

    public void Update(Shooter shooter)
    {
      t_shooter entity = _sqlRepository.Find(_ => _.ShooterId == shooter.ShooterId).Single();
      entity.UpdateEntity(shooter);
      _sqlRepository.Commit();
    }

    public void Delete(Shooter shooter)
    {
      t_shooter entity = _sqlRepository.Find(_ => _.ShooterId == shooter.ShooterId).Single();
      _sqlRepository.Delete(entity);
    }

    public Shooter FindByShooterNumber(int shooterNumber)
    {
      return
        _sqlRepository.Find(shooter => shooter.ShooterNumber == shooterNumber).Select(_selector).First();
    }

    public IEnumerable<Shooter> FindByPersonId(int personId)
    {
      return
        _sqlRepository.Find(shooter => shooter.PersonId == personId).Select(_selector);
    }
  }
}
