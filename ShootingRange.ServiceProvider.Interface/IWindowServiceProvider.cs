using ShootingRange.Service.Interface;

namespace ShootingRange.ServiceProvider.Interface
{
  public interface IWindowServiceProvider
  {
    IWindowService GetWindowService();
  }
}
