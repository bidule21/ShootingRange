using System.ComponentModel;
using System.Windows;

namespace ShootingRange.ViewModel
{
  public static class DesignTimeHelper
  {
    public static bool IsInDesignMode
    {
      get
      {
        return DesignerProperties.GetIsInDesignMode(new DependencyObject());
      }
    }
  }
}
