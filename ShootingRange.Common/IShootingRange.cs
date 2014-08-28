using System;

namespace ShootingRange.Common
{
  /// <summary>A shooting range which will fire events on specified situations. </summary>
  public interface IShootingRange
  {
    void Initialize();

    /// <summary>Log a message. </summary>
    event EventHandler<LogEventArgs> Log;

    /// <summary>A shooter number information is received. </summary>
    event EventHandler<ShooterNumberEventArgs> ShooterNumber;

    /// <summary>A shot information is received. </summary>
    event EventHandler<ShotEventArgs> Shot;

    /// <summary>A best shot of a series information is received. </summary>
    event EventHandler<ShotEventArgs> BestShot;

    /// <summary>A program number information is received. </summary>
    event EventHandler<PrchEventArgs> Prch;

    /// <summary>A subtotal is received. </summary>
    event EventHandler<SubtEventArgs> Subt;
  }

  public class LogEventArgs : EventArgs
  {
    public string Message { get; private set; }

    public LogEventArgs(string message)
    {
      Message = message;
    }
  }
}
