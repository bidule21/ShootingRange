using System;
using ShootingRange.Common.Modules;

namespace ShootingRange.SiusData.Messages
{
  public class SubtotalMessage : SiusDataMessage
  {
    public ShootingRangeModuleDelegate<SubtotalMessage> ProcessDelegate { get; set; }

    public SubtotalMessage(int shooterId, int laneId, int laneNumber, DateTime timestamp, double primaryTotal, double secondaryTotal)
    {
      ShooterId = shooterId;
      LaneId = laneId;
      LaneNumber = laneNumber;
      Timestamp = timestamp;
      PrimaryTotal = primaryTotal;
      SecondaryTotal = secondaryTotal;
      ProcessDelegate = delegate { };
    }

    public int ShooterId { get; private set; }
    public int LaneId { get; private set; }
    public int LaneNumber { get; private set; }
    public DateTime Timestamp { get; private set; }
    public double PrimaryTotal { get; private set; }
    public double SecondaryTotal { get; private set; }

    public override void Process()
    {
      if (ProcessDelegate != null)
      {
        ProcessDelegate(this);
      }
    }
  }
}
