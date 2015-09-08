using ShootingRange.BusinessObjects;
using ShootingRange.Entities;

namespace ShootingRange.Repository.Mapper
{
  internal static class SessionMapper
  {
    public static t_session UpdateEntity(this t_session entity, Session session)
    {
      entity.LaneNumber = session.LaneNumber;
      entity.ShooterId = session.ShooterId;
      entity.ProgramNumber = session.ProgramNumber;
      return entity;
    }
  }

  internal static class SessionSubtotalMapper
  {
    public static t_sessionsubtotal UpdateEntity(this t_sessionsubtotal entity, SubSession sessionSubtotal)
    {
      entity.SessionId = sessionSubtotal.SessionId;
      entity.BestShotId = sessionSubtotal.BestShotId;
      entity.SubtotalOrdinal = sessionSubtotal.Ordinal;
      return entity;
    }
  }
}
