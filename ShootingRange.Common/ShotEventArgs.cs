using System;

namespace ShootingRange.Common
{
  public class ShotEventArgs : EventArgs
  {
    public int ShooterNumber { get; set; }
    public double PrimaryScore { get; set; }
    public double SecondaryScore { get; set; }
    public int LaneNumber { get; set; }
  }
}
