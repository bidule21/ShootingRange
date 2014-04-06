using System.Collections.Generic;
using System.Linq;
using ShootingRange.Common;
using ShootingRange.Common.BusinessObjects;
using ShootingRange.Persistence;

namespace ShootingRange.Repository
{
  public class ShootingRangeRepository : IShootingRangeRepository
  {
    public Shooter GetShooterByShooterNumber(int shooterNumber)
    {
      Shooter shooter = null;
      using (ShootingRangeEntities context = new ShootingRangeEntities())
      {
        t_shooter dbShooter = context.t_shooter.FirstOrDefault(s => s.ShooterNumber == shooterNumber);
        if (dbShooter != null)
        {
          shooter = new Shooter
          {
            ShooterNumber = dbShooter.ShooterNumber
          };
        }
      }

      return shooter;
    }

    public void AddShot(Shot shot)
    {
      
    }
  }
}
