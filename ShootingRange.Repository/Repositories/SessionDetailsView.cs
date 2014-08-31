using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShootingRange.BusinessObjects;
using ShootingRange.Entities;
using ShootingRange.Repository.RepositoryInterfaces;

namespace ShootingRange.Repository.Repositories
{
  public class SessionDetailsView : ISessionDetailsView
  {
    private DbSet<t_shooter> _shooters;
    private DbSet<t_session> _sessions;
    private DbSet<t_shot> _shots;
    private DbSet<t_sessionsubtotal> _sessionSubtotals;
    private DbSet<t_programitem> _programItems;

    public SessionDetailsView(DbContext context)
    {
      _sessions = context.Set<t_session>();
      _shooters = context.Set<t_shooter>();
      _shots = context.Set<t_shot>();
      _sessionSubtotals = context.Set<t_sessionsubtotal>();
      _programItems = context.Set<t_programitem>();
    }

    public IEnumerable<SessionDetails> GetAll()
    {
      IEnumerable<SessionDetails> view = from session in _sessions
        join shooter in _shooters on session.ShooterId equals shooter.ShooterId
        join programItem in _programItems on session.ProgramItemId equals programItem.ProgramItemId
        join sessionSubtotal in _sessionSubtotals on session.SessionId equals sessionSubtotal.SessionId into subSessions
        select new SessionDetails
        {
          SessionId = session.SessionId,
          ProgramNumber = programItem.ProgramNumber,
          ShooterNumber = shooter.ShooterNumber,
          SessionDescription = programItem.ItemName,
          SubSessions = from subSession in subSessions
            select new SubSessionDetails
            {
              Ordinal = subSession.SubtotalOrdinal,
              Shots = from shot in subSession.t_shot
                select new Shot()
                {
                  PrimaryScore = shot.PrimaryScore,
                  SecondaryScore = shot.SecondaryScore,
                  Ordinal = shot.ShotOrdinal,
                  ValueX = shot.ValueX,
                  ValueY = shot.ValueY,
                }
            }
        };

      return view;
    }

    public IEnumerable<SessionDetails> FindByShooterId(int shooterId)
    {
      IEnumerable<SessionDetails> view = from session in _sessions
        join shooter in _shooters on session.ShooterId equals shooter.ShooterId
        where shooter.ShooterId == shooterId
        join programItem in _programItems on session.ProgramItemId equals programItem.ProgramItemId
        join sessionSubtotal in _sessionSubtotals on session.SessionId equals sessionSubtotal.SessionId into subSessions
        select new SessionDetails
        {
          SessionId = session.SessionId,
          ProgramNumber = programItem.ProgramNumber,
          ShooterNumber = shooter.ShooterNumber,
          SessionDescription = programItem.ItemName,
          SubSessions = from subSession in subSessions
            select new SubSessionDetails
            {
              Ordinal = subSession.SubtotalOrdinal,
              Shots = from shot in subSession.t_shot
                      select new Shot()
                      {
                        PrimaryScore = shot.PrimaryScore,
                        SecondaryScore = shot.SecondaryScore,
                        Ordinal = shot.ShotOrdinal,
                        ValueX = shot.ValueX,
                        ValueY = shot.ValueY,
                      }
            }
        };

      return view;
    }
  }
}
