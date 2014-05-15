using System;
using System.Collections.Generic;
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
    private IProgramItemDataStore _programItemDataStore;
    private IShooterDataStore _shooterDataStore;
    private ShootingRangeEvents _events;
    private readonly IShootingRange _shootingRange;

    private Dictionary<int, Session> _sessionsAwaitingProgramNumber;

    public ShootingRangeEngine(IConfiguration configuration)
    {
      _sessionsAwaitingProgramNumber = new Dictionary<int, Session>();

      _sessionDataStore = configuration.GetSessionDataStore();
      _programItemDataStore = configuration.GetProgramItemDataStore();
      _shotDataStore = configuration.GetShotDataStore();
      _shooterDataStore = configuration.GetShooterDataStore();

      _shootingRange = configuration.GetShootingRange();
      _shootingRange.ShooterNumber += ShootingRangeOnShooterNumber;
      _shootingRange.Prch += ShootingRangeOnPrch;
      _shootingRange.Shot += ShootingRangeOnShot;

      _events = configuration.GetEvents();
    }

    public void ConnectToSius()
    { 
      _shootingRange.Initialize();
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
          session.ProgramItemId = programItem.ProgramItemId;
          _sessionDataStore.Create(session);
        }

        Shot shot = new Shot
        {
          PrimaryScore = e.PrimaryScore,
          SecondaryScore = e.SecondaryScore,
          LaneNumber = e.LaneNumber
        };

        _shotDataStore.Create(shot);
      }
      catch (Exception exc)
      {
        Console.WriteLine(exc.Message);
      }
    }

    private void ShootingRangeOnPrch(object sender, PrchEventArgs e)
    {
      try
      {
        Shooter shooter = _shooterDataStore.FindByShooterNumber(e.ShooterNumber);
        _sessionsAwaitingProgramNumber[e.LaneNumber] = new Session
        {
          LaneNumber = e.LaneNumber,
          ShooterId = shooter.ShooterId,
          Timestamp = e.Timestamp,
          ProgramItemId = e.ProgramNumber,
        };
      }
      catch (Exception exc)
      {
        Console.WriteLine(exc.Message);
      }
    }

    private void ShootingRangeOnShooterNumber(object sender, ShooterNumberEventArgs e)
    {
      //_events.ShooterNumber(e);
    }
  }
}
