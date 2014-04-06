using System;
using System.Linq;
using ShootingRange.SiusData.Messages;

namespace ShootingRange.SiusData.Parser
{
  class PrchMessageParser : IMessageParser
  {
    private readonly MessageFactory _messageFactory;
    private const int ValueCount = 25;
    private const string Identifier = "_PRCH";

    private const int LaneId = 0;
    private const int ShooterNumber = 2;
    private const int Timestamp = 5;

    public PrchMessageParser(MessageFactory messageFactory)
    {
      _messageFactory = messageFactory;
    }

    public SiusDataMessage Parse(RawMessage rawMessage)
    {
      if (!rawMessage.IsResponsible(Identifier, ValueCount)) return null;

      string[] valueList = rawMessage.Values.ToArray();

      SuccessHelper allOverSuccess = new SuccessHelper();
      int shooterNumber = ValueAccessHelper.GetInt(valueList, ShooterNumber, allOverSuccess);
      int laneId = ValueAccessHelper.GetInt(valueList, LaneId, allOverSuccess);
      DateTime timestamp = ValueAccessHelper.GetDateTime(valueList, Timestamp, allOverSuccess);

      if (!allOverSuccess.Success) return null;
      return _messageFactory.MakePrchMessage(shooterNumber, laneId, timestamp);
    }
  }
}
