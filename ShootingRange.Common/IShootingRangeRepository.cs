﻿using ShootingRange.BusinessObjects;

namespace ShootingRange.Common
{
  public interface IShootingRangeRepository
  {
    void AddShot(Shot shot);
  }
}