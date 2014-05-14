using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
  }
}
