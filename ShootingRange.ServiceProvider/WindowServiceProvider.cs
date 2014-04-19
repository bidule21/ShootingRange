using ShootingRange.Service;
using ShootingRange.Service.Interface;
using ShootingRange.ServiceProvider.Interface;

namespace ShootingRange.ServiceProvider
{
    public class WindowServiceProvider : IWindowServiceProvider
    {
      public IWindowService GetWindowService()
      {
        return new WindowService();
      }
    }
}
