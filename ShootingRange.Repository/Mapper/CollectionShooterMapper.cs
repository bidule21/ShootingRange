using ShootingRange.BusinessObjects;
using ShootingRange.Entities;

namespace ShootingRange.Repository.Mapper
{
  internal static class CollectionShooterMapper
  {
    public static t_collectionshooter UpdateEntity(this t_collectionshooter entity, CollectionShooter shooter)
    {
      entity.CollectionShooterId = shooter.CollectionShooterId;
      entity.ShooterCollectionId = shooter.ShooterCollectionId;
      return entity;
    }
  }
}
