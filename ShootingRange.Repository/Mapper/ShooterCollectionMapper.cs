using ShootingRange.BusinessObjects;
using ShootingRange.Entities;

namespace ShootingRange.Repository.Mapper
{
  internal static class ShooterCollectionMapper
  {
    public static t_shootercollection UpdateEntity(this t_shootercollection entity, ShooterCollection shooter)
    {
      entity.CollectionName = shooter.CollectionName;
      return entity;
    }
  }
}
