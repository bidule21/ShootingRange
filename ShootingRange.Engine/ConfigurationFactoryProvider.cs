using System.ComponentModel;
using System.Windows;

namespace ShootingRange.Engine
{
  public static class ConfigurationFactoryProvider
  {
    public static IConfigurationFactory GetConfigurationFactory()
    {
      if (IsInDesignMode)
        return new DummyRangeConfigurationFactory();
      else
        return new ConfigurationFactory();
    }

    public static bool IsInDesignMode
    {
      get
      {
        return DesignerProperties.GetIsInDesignMode(new DependencyObject());
      }
    }
  }
}
