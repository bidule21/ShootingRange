using System;
using ShootingRange.Repository.RepositoryInterfaces;
using ShootingRange.Service.Interface;

namespace ShootingRange.Service
{
  public class FakeShooterNumberService : IShooterNumberService
  {
    private static int _shooterNumber = 1;
    public int GetShooterNumber()
    {
      return _shooterNumber++;
    }

    public void Configure(IShooterDataStore shooterDataStore)
    {
      throw new NotImplementedException();
    }
  }
}
