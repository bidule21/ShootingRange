namespace ShootingRange.SiusData.Messages
{
  public interface IMessageHandler
  {
    void ProcessTotalMessage(TotalMessage e);
    void ProcessSubtotalMessage(SubtotalMessage e);
    void ProcessBestShotMessage(BestShotMessage e);
    void ProcessShotMessage(ShotMessage e);
    void ProcessPrchMessage(PrchMessage e);
  }
}