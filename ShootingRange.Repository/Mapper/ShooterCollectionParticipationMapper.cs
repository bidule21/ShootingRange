using ShootingRange.BusinessObjects;
using ShootingRange.Entities;

namespace ShootingRange.Repository.Mapper
{
  internal static class ShooterCollectionParticipationMapper
  {
    public static t_shootercollectionparticipation UpdateEntity(this t_shootercollectionparticipation entity, ShooterCollectionParticipation shooter)
    {
      entity.ParticipationId = shooter.ParticipationId;
      entity.ShooterCollectionId = shooter.ShooterCollectionId;
      return entity;
    }
  }
}
