using System;
using System.Collections.Generic;
using System.IO;
using ShootingRange.Common;
using ShootingRange.SiusData.Messages;
using ShootingRange.SiusData.Parser;

namespace ShootingRange.SiusData
{
  public abstract class SiusDataProvider : IShootingRange, IMessageHandler
  {
    MessageParser _parser;

    public event EventHandler<LogEventArgs> Log;

    private string _dumpFilePath;

    protected SiusDataProvider()
    {
      var factory = new MessageFactory(this);
      _parser = new MessageParser(factory);

      _dumpFilePath = string.Format("{0}.log", DateTime.Now.ToString("yyyyMMdd_HHmmss"));
    }

    public abstract void Initialize();

    public event EventHandler<ShooterNumberEventArgs> ShooterNumber;
    public event EventHandler<ShotEventArgs> Shot;
    public event EventHandler<ShotEventArgs> BestShot;
    public event EventHandler<PrchEventArgs> Prch;
    public event EventHandler<SubtEventArgs> Subt;

    public void ProcessSiusDataMessage(string message)
    {
      using (TextWriter writer = new StreamWriter(_dumpFilePath, true))
      {
        writer.WriteLine(message);
      }

      LogMessage(message);
      IEnumerable<SiusDataMessage> messages =_parser.Parse(message);
      foreach (SiusDataMessage siusDataMessage in messages)
      {
        siusDataMessage.Process();
      }
    }

    protected virtual void LogMessage(string message)
    {
      LogEventArgs e = new LogEventArgs(message);
      OnLog(e);
    }

    public void ProcessTotalMessage(TotalMessage totalMessage)
    {
    }

    public void ProcessSubtotalMessage(SubtotalMessage subtotalMessage)
    {
      SubtEventArgs e = new SubtEventArgs
      {
        LaneId = subtotalMessage.LaneId,
        LaneNumber = subtotalMessage.LaneNumber
      };

      OnSubt(e);
    }

    public void ProcessBestShotMessage(BestShotMessage bestShotMessage)
    {
      ShotEventArgs e = new ShotEventArgs
      {
        PrimaryScore = bestShotMessage.PrimaryScore,
        SecondaryScore = bestShotMessage.SecondaryScore,
        ProgramNumber = bestShotMessage.ProgramNumber,
        LaneNumber = bestShotMessage.LaneNumber,
        LaneId = bestShotMessage.LaneId,
        Ordinal = bestShotMessage.ShotNbr,
      };

      OnBestShot(e);
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
        Ordinal = shotMessage.ShotNbr,
      };
      OnShot(e);
    }

    public void ProcessPrchMessage(PrchMessage prchMessage)
    {
      PrchEventArgs e = new PrchEventArgs
      {
        LaneNumber = prchMessage.LaneNumber,
        ShooterNumber = prchMessage.ShooterNumber,
      };

      OnPrch(e);
    }

    protected virtual void OnSubt(SubtEventArgs e)
    {
      EventHandler<SubtEventArgs> handler = Subt;
      if (handler != null) handler(this, e);
    }

    protected virtual void OnBestShot(ShotEventArgs e)
    {
      EventHandler<ShotEventArgs> handler = BestShot;
      if (handler != null) handler(this, e);
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

    protected virtual void OnLog(LogEventArgs e)
    {
      EventHandler<LogEventArgs> handler = Log;
      if (handler != null) handler(this, e);
    }
  }
}
