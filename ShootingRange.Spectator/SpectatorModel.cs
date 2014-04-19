using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ShootingRange.Common;
using ShootingRange.Configuration;
using ShootingRange.ConfigurationProvider;
using ShootingRange.Engine;
using ShootingRange.Spectator.Properties;

namespace ShootingRange.Spectator
{
  class SpectatorModel : INotifyPropertyChanged
  {
    public SpectatorModel()
    {
      IConfiguration config = new DefaultConfiguration();
      ShootingRangeEngine engine = new ShootingRangeEngine(config);
      IShootingRange shootingRange = config.GetShootingRange();

      shootingRange.Shot += ShootingRangeOnShot;
      shootingRange.Initialize();
    }

    
    private string _text;
    public string Text
    {
      get { return _text; }
      set
      {
        if (value != _text)
        {
          _text = value;
          OnPropertyChanged("Text");
        }
      }
    }

    private void ShootingRangeOnShot(object sender, ShotEventArgs e)
    {
      Text += string.Format("PrimaryScore {0}", e.PrimaryScore) + Environment.NewLine;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
