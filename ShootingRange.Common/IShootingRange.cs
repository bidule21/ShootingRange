using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShootingRange.Common
{
  /// <summary>A shooting range which will fire events on specified situations. </summary>
  public interface IShootingRange
  {
    void Initialize();

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
}
