using System;

namespace ShootingRange.SiusData.Messages
{
  public class TotalMessage : SiusDataMessage
  {
    public SiusDataMessageProcessDelegate<TotalMessage> ProcessDelegate { get; set; }

    public TotalMessage(int shooterId, int laneId, DateTime timestamp, double primaryTotal, double secondaryTotal)
    {
      ShooterId = shooterId;
      LaneId = laneId;
      Timestamp = timestamp;
      PrimaryTotal = primaryTotal;
      SecondaryTotal = secondaryTotal;
    }

    public int ShooterId { get; private set; }
    public int LaneId { get; private set; }
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
