using System;
using System.Linq;
using ShootingRange.SiusData.Messages;

namespace ShootingRange.SiusData.Parser
{
  class ShotMessageParser : IMessageParser
  {
    private readonly MessageFactory _messageFactory;
    private const int ValueCount = 23;
    private const string Identifier = "_SHOT";

    private const int LaneId = 0;
    private const int LaneNumber = 1;
    private const int ShooterId = 2;
    private const int DateTime = 5;
    private const int ShotType = 6;
    private const int PrimaryScore = 9;
    private const int SecondaryScore = 10;
    private const int ShotNbr = 12;
    private const int X = 13;
    private const int Y = 14;
    private const int ProgramNumber = 22;

    public ShotMessageParser(MessageFactory messageFactory)
    {
      if (messageFactory == null) throw new ArgumentNullException("messageFactory");
      _messageFactory = messageFactory;
    }

    public SiusDataMessage Parse(RawMessage rawMessage)
    {
      if (!rawMessage.IsResponsible(Identifier, ValueCount)) return null;
      
      string[] valueList = rawMessage.Values.ToArray();

      SuccessHelper allOverSuccess = new SuccessHelper();
      int laneId = ValueAccessHelper.GetInt(valueList, LaneId, allOverSuccess);
      int laneNbr = ValueAccessHelper.GetInt(valueList, LaneNumber, allOverSuccess);
      int shooterId = ValueAccessHelper.GetInt(valueList, ShooterId, allOverSuccess);
      DateTime timstamp = ValueAccessHelper.GetDateTime(valueList, DateTime, allOverSuccess);
      int shotType = ValueAccessHelper.GetInt(valueList, ShotType, allOverSuccess);
      double primaryScore = ValueAccessHelper.GetDouble(valueList, PrimaryScore, allOverSuccess);
      double secondaryScore = ValueAccessHelper.GetDouble(valueList, SecondaryScore, allOverSuccess);
      int shotNbr = ValueAccessHelper.GetInt(valueList, ShotNbr, allOverSuccess);
      float x = ValueAccessHelper.GetFloat(valueList, X, allOverSuccess);
      float y = ValueAccessHelper.GetFloat(valueList, Y, allOverSuccess);
      int programNumber = ValueAccessHelper.GetInt(valueList, ProgramNumber, allOverSuccess);

      if (!allOverSuccess.Success) return null;
      SiusDataMessage message = null;
      switch (shotType)
      {
        case 2:
          message = _messageFactory.MakeBestShotMessage(shooterId, laneId, timstamp, primaryScore, secondaryScore, shotNbr, programNumber);
          break;
        case 3:
          message = _messageFactory.MakeShotMessage(shooterId, laneId, timstamp, primaryScore, secondaryScore, shotNbr, programNumber);
          break;
      }
      return message;
    }
  }
}
