using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShootingRange.BusinessObjects;
using ShootingRange.Entities;

namespace ShootingRange.Repository.Mapper
{
  internal static class ShooterNumberConfigMapper
  {
    public static t_shooternumberconfig UpdateEntity(this t_shooternumberconfig entity,
      ShooterNumberConfig shooterNumberConfig)
    {
      entity.ShooterNumberIncrement = shooterNumberConfig.ShooterNumberIncrement;
      entity.LastGivenShooterNumber = shooterNumberConfig.LastGivenShooterNumber;
      return entity;
    }
  }
}
