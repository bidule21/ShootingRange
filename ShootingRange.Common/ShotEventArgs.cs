using System;

namespace ShootingRange.Common
{
  public class ShotEventArgs : EventArgs
  {
    public int LaneId { get; set; }
    public int ShooterNumber { get; set; }
    public decimal PrimaryScore { get; set; }
    public decimal? SecondaryScore { get; set; }
    public int LaneNumber { get; set; }
    public int ProgramNumber { get; set; }
    public int Ordinal { get; set; }
  }
}
