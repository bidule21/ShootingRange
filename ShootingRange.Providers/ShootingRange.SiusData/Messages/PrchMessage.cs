using System;
using ShootingRange.Common.Modules;

namespace ShootingRange.SiusData.Messages
{
  public class PrchMessage : SiusDataMessage
  {
    public ShootingRangeModuleDelegate<PrchMessage> ProcessDelegate { get; set; }

    public PrchMessage(int shooterNumber, int laneId, int laneNumber, DateTime timestamp)
    {
      ShooterNumber = shooterNumber;
      LaneId = laneId;
      Timestamp = timestamp;
      LaneNumber = laneNumber;
    }

    public int ShooterNumber { get; private set; }
    public int LaneId { get; private set; }
    public int LaneNumber { get; private set; }
    public DateTime Timestamp { get; private set; }

    public override void Process()
    {
      if (ProcessDelegate != null)
      {
        ProcessDelegate(this);
      }
    }
  }
}
