using System.Runtime.InteropServices.ComTypes;

namespace ShootingRange.Common
{
  public delegate void UIEventsDelegate<T>(T e);

  public delegate void UIEventsDelegate();

  public delegate T UiEventsDelegate<T>();
}
