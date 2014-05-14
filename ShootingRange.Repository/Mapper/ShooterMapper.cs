using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShootingRange.BusinessObjects;
using ShootingRange.Entities;

namespace ShootingRange.Repository.Mapper
{
  internal static class ShooterMapper
  {
    public static t_shooter UpdateEntity(this t_shooter entity, Shooter shooter)
    {
      entity.ShooterNumber = shooter.ShooterNumber;
      entity.PersonId = shooter.PersonId;
      return entity;
    }
  }
}
