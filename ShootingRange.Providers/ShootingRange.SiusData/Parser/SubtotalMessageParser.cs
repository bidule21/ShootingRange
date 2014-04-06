using System;
using System.Linq;
using ShootingRange.SiusData.Messages;

namespace ShootingRange.SiusData.Parser
{
  class SubtotalMessageParser : IMessageParser
  {
    private readonly MessageFactory _messageFactory;
    private const int ValueCount = 11;
    private const string Identifier = "_SUBT";

    private const int LaneId = 0;
    private const int ShooterId = 2;
    private const int Timestamp = 5;
    private const int PrimaryScore = 8;
    private const int SecondaryScore = 9;

    public SubtotalMessageParser(MessageFactory messageFactory)
    {
      _messageFactory = messageFactory;
    }

    public SiusDataMessage Parse(RawMessage rawMessage)
    {
      if (!rawMessage.IsResponsible(Identifier, ValueCount)) return null;
      string[] valueList = rawMessage.Values.ToArray();

      SuccessHelper allOverSuccess = new SuccessHelper();
      int laneId = ValueAccessHelper.GetInt(valueList, LaneId, allOverSuccess);
      int shooterId = ValueAccessHelper.GetInt(valueList, ShooterId, allOverSuccess);
      int primaryScore = ValueAccessHelper.GetInt(valueList, PrimaryScore, allOverSuccess);
      int secondaryScore = ValueAccessHelper.GetInt(valueList, SecondaryScore, allOverSuccess);
      DateTime timestamp = ValueAccessHelper.GetDateTime(valueList, Timestamp, allOverSuccess);

      if (!allOverSuccess.Success) return null;
      return _messageFactory.MakeSubtotalMessage(shooterId, laneId, timestamp, primaryScore, secondaryScore);
    }
  }
}
