using ShootingRange.Service.Interface;
using ShootingRange.View;

namespace ShootingRange.Service
{
  public class WindowService : IWindowService
  {
    public void ShowPersonEditWindow()
    {
      PersonEditWindow window = new PersonEditWindow();
      window.Show();
    }
  }
}