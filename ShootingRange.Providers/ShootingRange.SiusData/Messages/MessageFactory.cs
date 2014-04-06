using System;

namespace ShootingRange.SiusData.Messages
{
  public class MessageFactory
  {
    private readonly IMessageHandler _messageHandler;

    public MessageFactory(IMessageHandler messageHandler)
    {
      if (messageHandler == null) throw new ArgumentNullException("messageHandler");
      _messageHandler = messageHandler;
    }

    public PrchMessage MakePrchMessage(int shooterNumber, int laneId, DateTime timestamp)
    {
      var message = new PrchMessage(shooterNumber, laneId, timestamp);
      message.ProcessDelegate += _messageHandler.ProcessPrchMessage;
      return message;
    }

    public ShotMessage MakeShotMessage(int shooterId, int laneId, DateTime timestamp, double primaryScore,
      double secondaryScore, int shotNbr, int programNumber)
    {
      var message = new ShotMessage(shooterId, laneId, timestamp, primaryScore, secondaryScore, shotNbr, programNumber);
      message.ProcessDelegate += _messageHandler.ProcessShotMessage;
      return message;
    }

    public BestShotMessage MakeBestShotMessage(int shooterId, int laneId, DateTime timestamp, double primaryScore,
      double secondaryScore, int shotNbr, int programNumber)
    {
      var message = new BestShotMessage(shooterId, laneId, timestamp, primaryScore, secondaryScore, shotNbr, programNumber);
      message.ProcessDelegate = _messageHandler.ProcessBestShotMessage;
      return message;
    }

    public TotalMessage MakeTotalMessage(int shooterId, int laneId, DateTime timestamp, int primaryTotal,
      int secondaryTotal)
    {
      var message = new TotalMessage(shooterId, laneId, timestamp, primaryTotal, secondaryTotal);
      message.ProcessDelegate += _messageHandler.ProcessTotalMessage;
      return message;
    }

    public SubtotalMessage MakeSubtotalMessage(int shooterId, int laneId, DateTime timestamp, int primaryTotal,
      int secondaryTotal)
    {
      var message = new SubtotalMessage(shooterId, laneId, timestamp, primaryTotal, secondaryTotal);
      message.ProcessDelegate += _messageHandler.ProcessSubtotalMessage;
      return message;
    }
  }
}
