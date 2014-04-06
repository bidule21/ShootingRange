using ShootingRange.Common.BusinessObjects;

namespace ShootingRange.Common
{
  public interface IShootingRangeRepository
  {
    Shooter GetShooterByShooterNumber(int shooterNumber);
    void AddShot(Shot shot);
  }
}