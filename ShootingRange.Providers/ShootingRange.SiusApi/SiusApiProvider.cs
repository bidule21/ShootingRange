using System;
using ShootingRange.Common;
using Sius.Communication.SiusApi.ClientLib;
using Sius.Communication.SiusApi.Contracts;
using ShotEventArgs = ShootingRange.Common.ShotEventArgs;

namespace ShootingRange.SiusApi
{
    public class SiusApiProvider : IShootingRange
    {
      private readonly string _server;

      public SiusApiProvider(string server)
      {
        _server = server;
      }

      public void Initialize()
      {
        try
        {
          SiusApiClient client = new SiusApiClient(_server);
          client.RCIMessageReceived += ClientOnRciMessageReceived;

          client.RequestConnectedDevices();
          //client.Login("ShootingRange.Spectator");
        }
        catch (Exception e)
        {
          Console.WriteLine(e.ToString());
        }

      }

      private void ClientOnRciMessageReceived(object sender, RCIEventArgs rciEventArgs)
      {
        throw new NotImplementedException();
      }

      public event EventHandler<ShooterNumberEventArgs> ShooterNumber;
      public event EventHandler<ShotEventArgs> Shot;
      public event EventHandler<ProgramNumberEventArgs> ProgramNumber;
    }
}
