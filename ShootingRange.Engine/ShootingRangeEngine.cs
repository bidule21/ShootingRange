using System;
using ShootingRange.Common;
using ShootingRange.Common.BusinessObjects;
using ShootingRange.Common.Modules;
using ShootingRange.ConfigurationProvider;

namespace ShootingRange.Engine
{
  public class ShootingRangeEngine
  {
    private IShootingRangeRepository _repository;
    private ShootingRangeEvents _events;
    private SessionManager _sessionManager;

    public ShootingRangeEngine(IConfiguration configuration)
    {
      IShootingRange shootingRange = configuration.GetShootingRange();
      shootingRange.ShooterNumber += ShootingRangeOnShooterNumber;
      shootingRange.ProgramNumber += ShootingRangeOnProgramNumber;
      shootingRange.Shot += ShootingRangeOnShot;

      _events = configuration.GetEvents();
    }

    /// <summary>Stores the shot data to the repository and invokes module extension points.</summary>
    private void ShootingRangeOnShot(object sender, ShotEventArgs e)
    {
      Shot shot = new Shot {PrimaryScore = e.PrimaryScore, SecondaryScore = e.SecondaryScore, LaneNumber = e.LaneNumber};
      _repository.AddShot(shot);

      _events.Shot(e);
    }

    private void ShootingRangeOnProgramNumber(object sender, ProgramNumberEventArgs e)
    {
      _events.ProgramNumber(e);
    }

    private void ShootingRangeOnShooterNumber(object sender, ShooterNumberEventArgs e)
    {
      _events.ShooterNumber(e);
    }
  }
}
