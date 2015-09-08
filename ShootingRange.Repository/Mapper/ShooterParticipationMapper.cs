using ShootingRange.BusinessObjects;
using ShootingRange.Entities;

namespace ShootingRange.Repository.Mapper
{
  internal static class ShooterParticipationMapper
  {
    public static t_shooterparticipation UpdateEntity(this t_shooterparticipation entity,
      ShooterParticipation shooterParticipation)
    {
      entity.ShooterId = shooterParticipation.ShooterId;
      entity.ProgramNumber = shooterParticipation.ProgramNumber;
      return entity;
    }
  }
}
