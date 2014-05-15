using System;
using System.Collections.Generic;
using ShootingRange.Common;
using ShootingRange.SiusData.Messages;
using ShootingRange.SiusData.Parser;

namespace ShootingRange.SiusData
{
  public abstract class SiusDataProvider : IShootingRange, IMessageHandler
  {
    MessageParser _parser;

    protected SiusDataProvider()
    {
      var factory = new MessageFactory(this);
      _parser = new MessageParser(factory);
    }

    public abstract void Initialize();

    public event EventHandler<ShooterNumberEventArgs> ShooterNumber;
    public event EventHandler<ShotEventArgs> Shot;
    public event EventHandler<PrchEventArgs> Prch;

    public void ProcessSiusDataMessage(string message)
    {
      IEnumerable<SiusDataMessage> messages =_parser.Parse(message);
      foreach (SiusDataMessage siusDataMessage in messages)
      {
        siusDataMessage.Process();
      }
    }

    public void ProcessTotalMessage(TotalMessage totalMessage)
    {
    }

    public void ProcessSubtotalMessage(SubtotalMessage subtotalMessage)
    {
    }

    public void ProcessBestShotMessage(BestShotMessage bestShotMessage)
    {
    }

    public void ProcessShotMessage(ShotMessage shotMessage)
    {
      ShotEventArgs e = new ShotEventArgs
      {
        PrimaryScore = shotMessage.PrimaryScore,
        SecondaryScore = shotMessage.SecondaryScore,
        ProgramNumber = shotMessage.ProgramNumber,
        LaneNumber = shotMessage.LaneNumber,
        LaneId =  shotMessage.LaneId,
      };
      OnShot(e);
    }

    public void ProcessPrchMessage(PrchMessage prchMessage)
    {
      PrchEventArgs e = new PrchEventArgs
      {
        LaneNumber = prchMessage.LaneNumber,
        ShooterNumber = prchMessage.ShooterNumber,
        Timestamp = prchMessage.Timestamp,
      };

      OnPrch(e);
    }

    protected virtual void OnShot(ShotEventArgs e)
    {
      EventHandler<ShotEventArgs> handler = Shot;
      if (handler != null) handler(this, e);
    }

    protected virtual void OnPrch(PrchEventArgs e)
    {
      EventHandler<PrchEventArgs> handler = Prch;
      if (handler != null) handler(this, e);
    }
  }
}
