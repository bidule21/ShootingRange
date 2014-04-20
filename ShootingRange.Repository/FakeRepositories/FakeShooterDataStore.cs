using System.Collections.Generic;
using System.Linq;
using Repository;
using ShootingRange.BusinessObjects;
using ShootingRange.Repository.Repositories;

namespace ShootingRange.Repository.FakeRepositories
{
  public class FakeShooterDataStore : IShooterDataStore
  {
    private FakeRepository<Shooter> _repository;

    public FakeShooterDataStore()
    {
      _repository = new FakeRepository<Shooter>();

      Shooter[] shooters = new[]
      {
        new Shooter {ShooterId = 1, ShooterNumber = 123458},
        new Shooter {ShooterId = 2, ShooterNumber = 987522},
      };

      foreach (Shooter shooter in shooters)
      {
        _repository.Insert(shooter);
      }
    }

    public void Create(Shooter shooter)
    {
      _repository.Insert(shooter);
    }

    public Shooter FindById(int id)
    {
      return _repository.Find(_ => _.ShooterId == id).Single();
    }

    public IEnumerable<Shooter> GetAll()
    {
      return _repository.GetAll();
    }

    public void Update(Shooter shooter)
    {
      Shooter entity = FindById(shooter.ShooterId);
      entity.ShooterNumber = shooter.ShooterNumber;
      entity.GroupId = shooter.GroupId;
      _repository.Commit();
    }

    public void Delete(Shooter shooter)
    {
      _repository.Delete(shooter);
    }

    public IEnumerable<Shooter> FindByShooterNumber(int shooterNumber)
    {
      return _repository.Find(shooter => shooter.ShooterNumber == shooterNumber);
    }

    public IEnumerable<Shooter> FindByPersonId(int personId)
    {
      return _repository.Find(shooter => shooter.PersonId == personId);
    }
  }
}
