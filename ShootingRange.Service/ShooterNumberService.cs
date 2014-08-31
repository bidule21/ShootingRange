using System;
using System.Collections.Generic;
using System.Linq;
using ShootingRange.BusinessObjects;
using ShootingRange.Repository.Repositories;
using ShootingRange.Repository.RepositoryInterfaces;
using ShootingRange.Service.Interface;

namespace ShootingRange.Service
{
  public class ShooterNumberService : IShooterNumberService
  {
    private readonly IShooterNumberConfigDataStore _shooterNumberConfigDataStore;
    private const int ConfigId = 1;

    public ShooterNumberService(IShooterNumberConfigDataStore shooterNumberConfigDataStore)
    {
      _shooterNumberConfigDataStore = shooterNumberConfigDataStore;
    }

    public int GetShooterNumber()
    {
      ShooterNumberConfig config = _shooterNumberConfigDataStore.FindById(ConfigId);
      int nextShooterNumber = config.LastGivenShooterNumber + config.ShooterNumberIncrement;
      config.LastGivenShooterNumber = nextShooterNumber;
      _shooterNumberConfigDataStore.Update(config);
      return nextShooterNumber;
    }

    public void Configure(IShooterDataStore shooterDataStore)
    {
      IEnumerable<Shooter> shooters = shooterDataStore.GetAll().ToArray();
      if (shooters.Any())
      {
        int highestShooterNumber = shooters.Max(_ => _.ShooterNumber);
        ShooterNumberConfig config = _shooterNumberConfigDataStore.FindById(ConfigId);
        config.LastGivenShooterNumber = highestShooterNumber;
        _shooterNumberConfigDataStore.Update(config);
      }
    }
  }
}
