using System;

namespace ShootingRange.Common
{
  public class PrchEventArgs : EventArgs
  {
    public int ProgramNumber { get; set; }
    public int ShooterNumber { get; set; }
    public DateTime Timestamp { get; set; }
    public int LaneNumber { get; set; }
  }
}