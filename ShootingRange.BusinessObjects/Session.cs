using System;

namespace ShootingRange.BusinessObjects
{
  public class Session
  {
    public int SessionId { get; set; }
    public int ShooterId { get; set; }
    public int LaneNumber { get; set; }
    public int ProgramItemId { get; set; }
    public DateTime Timestamp { get; set; }
  }
}
