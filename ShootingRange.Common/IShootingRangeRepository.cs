using ShootingRange.Common.BusinessObjects;

namespace ShootingRange.Common
{
  public interface IShootingRangeRepository
  {
    void AddShot(Shot shot);
  }
}