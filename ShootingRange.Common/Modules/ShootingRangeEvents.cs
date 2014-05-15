namespace ShootingRange.Common.Modules
{
  public class ShootingRangeEvents
  {
    public ShootingRangeEvents()
    {
      Shot = delegate { };
      ProgramNumber = delegate { };
      ShooterNumber = delegate { };
      SelectedPersonChanged = delegate { };
    }

    public ShootingRangeModuleDelegate<ShotEventArgs> Shot { get; set; }

    public ShootingRangeModuleDelegate<PrchEventArgs> ProgramNumber { get; set; }

    public ShootingRangeModuleDelegate<ShooterNumberEventArgs> ShooterNumber { get; set; }

    public ShootingRangeModuleDelegate<int> SelectedPersonChanged { get; set; } 
  }
}
