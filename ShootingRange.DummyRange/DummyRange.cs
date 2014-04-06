using System;
using ShootingRange.Common;

namespace ShootingRange.DummyRange
{
  class DummyRange : IShootingRange
  {
    public void ProcessShooterNumber(int shooterNumber)
    {
      OnShooterNumber(new ShooterNumberEventArgs());
    }

    public void ProcessShot(int primaryScore)
    {
      OnShot(new ShotEventArgs());
    }

    public void ProcessProgramNumber(int programNumber)
    {
      OnProgramNumber(new ProgramNumberEventArgs());
    }

    protected virtual void OnShooterNumber(ShooterNumberEventArgs e)
    {
      EventHandler<ShooterNumberEventArgs> handler = ShooterNumber;
      if (handler != null) handler(this, e);
    }

    protected virtual void OnShot(ShotEventArgs e)
    {
      EventHandler<ShotEventArgs> handler = Shot;
      if (handler != null) handler(this, e);
    }
    protected virtual void OnProgramNumber(ProgramNumberEventArgs e)
    {
      EventHandler<ProgramNumberEventArgs> handler = ProgramNumber;
      if (handler != null) handler(this, e);
    }

    public void Initialize()
    {
      throw new NotImplementedException();
    }

    public event EventHandler<ShooterNumberEventArgs> ShooterNumber;
    public event EventHandler<ShotEventArgs> Shot;
    public event EventHandler<ProgramNumberEventArgs> ProgramNumber;
  }
}
