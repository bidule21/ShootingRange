namespace ShootingRange.Common.Modules
{
  public class ShootingRangeEvents
  {
    public ShootingRangeEvents()
    {
      Shot = delegate { };
      ProgramNumber = delegate { };
      ShooterNumber = delegate { };
    }

    public ShootingRangeModuleDelegate<ShotEventArgs> Shot { get; set; }

    public ShootingRangeModuleDelegate<ProgramNumberEventArgs> ProgramNumber { get; set; }

    public ShootingRangeModuleDelegate<ShooterNumberEventArgs> ShooterNumber { get; set; }
  }
}
