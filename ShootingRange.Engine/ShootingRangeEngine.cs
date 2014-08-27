using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Timers;
using ShootingRange.BusinessObjects;
using ShootingRange.Common;
using ShootingRange.Common.Modules;
using ShootingRange.ConfigurationProvider;
using ShootingRange.Repository.Repositories;

namespace ShootingRange.Engine
{
  public class ShootingRangeEngine
  {
    private IShotDataStore _shotDataStore;
    private ISessionDataStore _sessionDataStore;
    private ISessionSubtotalDataStore _sessionSubtotalDataStore;
    private IProgramItemDataStore _programItemDataStore;
    private IShooterDataStore _shooterDataStore;
    private IPersonDataStore _personDataStore;
    private ShootingRangeEvents _events;
    private readonly IShootingRange _shootingRange;

    private Dictionary<int, Session> _sessionsAwaitingProgramNumber;
    private Dictionary<int, Session> _sessionsOngoing;

    public ShootingRangeEngine(IConfiguration configuration)
    {
      _sessionsAwaitingProgramNumber = new Dictionary<int, Session>();
      _sessionsOngoing = new Dictionary<int, Session>();

      _sessionDataStore = configuration.GetSessionDataStore();
      _sessionSubtotalDataStore = configuration.GetSessionSubtotalDataStore();
      _programItemDataStore = configuration.GetProgramItemDataStore();
      _shotDataStore = configuration.GetShotDataStore();
      _shooterDataStore = configuration.GetShooterDataStore();
      _personDataStore = configuration.GetPersonDataStore();

      _shootingRange = configuration.GetShootingRange();
      _shootingRange.ShooterNumber += ShootingRangeOnShooterNumber;
      _shootingRange.Prch += ShootingRangeOnPrch;
      _shootingRange.Shot += ShootingRangeOnShot;
      _shootingRange.BestShot += ShootingRangeOnBestShot;
      _shootingRange.Subt += ShootingRangeOnSubt;

      _events = configuration.GetEvents();
    }

    public void ConnectToSius()
    { 
      _shootingRange.Initialize();
    }

    private void ShootingRangeOnSubt(object sender, SubtEventArgs e)
    {
      if (_sessionsOngoing.ContainsKey(e.LaneNumber))
      {
        Session session = _sessionsOngoing[e.LaneNumber];
        session.CreateSubSession();
      }
    }

    private void ShootingRangeOnBestShot(object sender, ShotEventArgs e)
    {
      if (_sessionsOngoing.ContainsKey(e.LaneNumber))
      {
        SubSession currentSubSession = _sessionsOngoing[e.LaneNumber].CurrentSubsession();
        Shot shot =
          _shotDataStore.FindBySubSessionId(currentSubSession.SessionSubtotalId).Single(_ => _.Ordinal == e.Ordinal);
        currentSubSession.BestShotId = shot.ShotId;
        _sessionSubtotalDataStore.Update(currentSubSession);
      }
    }

    /// <summary>Stores the shot data to the repository and invokes module extension points.</summary>
    private void ShootingRangeOnShot(object sender, ShotEventArgs e)
    {
      try
      {
        if (_sessionsAwaitingProgramNumber.ContainsKey(e.LaneNumber))
        {
          ProgramItem programItem = _programItemDataStore.FindByProgramNumber(e.ProgramNumber);
          if (programItem == null)
          {
            programItem = new ProgramItem
            {
              ProgramName = "unknown",
              ProgramNumber = e.ProgramNumber
            };
            _programItemDataStore.Create(programItem);
          }

          Session session = _sessionsAwaitingProgramNumber[e.LaneNumber];
          _sessionsAwaitingProgramNumber.Remove(e.LaneNumber);
          _sessionsOngoing.Add(e.LaneNumber, session);
          session.ProgramItemId = programItem.ProgramItemId;
          _sessionDataStore.Create(session);

          SubSession subSession = session.CreateSubSession();
          _sessionSubtotalDataStore.Create(subSession);

          AddShotToSubsession(e, subSession);
        }
        else if (_sessionsOngoing.ContainsKey(e.LaneNumber))
        {
          Session session = _sessionsOngoing[e.LaneNumber];
          SubSession subSession = session.CurrentSubsession();
          if (subSession.SessionSubtotalId == 0)
            _sessionSubtotalDataStore.Create(subSession);
          AddShotToSubsession(e, subSession);
        }
        else
        {
          throw new InvalidOperationException("Session is not available.");          
        }
      }
      catch (Exception exc)
      {
        Console.WriteLine(exc.Message);
      }
    }

    private void AddShotToSubsession(ShotEventArgs e, SubSession subSession)
    {
      Shot shot = new Shot
      {
        PrimaryScore = e.PrimaryScore,
        SecondaryScore = e.SecondaryScore,
        LaneNumber = e.LaneNumber,
        SubtotalId = subSession.SessionSubtotalId,
        Ordinal = e.Ordinal,
      };
      _shotDataStore.Create(shot);
    }

    private void ShootingRangeOnPrch(object sender, PrchEventArgs e)
    {
      try
      {
        Shooter shooter = _shooterDataStore.FindByShooterNumber(e.ShooterNumber);
        if (shooter == null)
        {
          shooter = CreateUnknownShooter(e.ShooterNumber);
        }

        if (_sessionsOngoing.ContainsKey(e.LaneNumber))
        {
          _sessionsOngoing.Remove(e.LaneNumber);
        }

        _sessionsAwaitingProgramNumber.Add(e.LaneNumber, new Session
        {
          LaneNumber = e.LaneNumber,
          ShooterId = shooter.ShooterId,
        });
      }
      catch (Exception exc)
      {
        Console.WriteLine(exc.Message);
      }
    }

    private Shooter CreateUnknownShooter(int shooterNumber)
    {
      Person person = _personDataStore.FindByLastName("unknown").First();
      Shooter shooter = new Shooter();
      shooter.PersonId = person.PersonId;
      shooter.ShooterNumber = shooterNumber;
      _shooterDataStore.Create(shooter);
      return shooter;
    }

    private void ShootingRangeOnShooterNumber(object sender, ShooterNumberEventArgs e)
    {
     // Shooter shooter = _shooterDataStore.FindByShooterNumber(e.);
    }
  }
}
