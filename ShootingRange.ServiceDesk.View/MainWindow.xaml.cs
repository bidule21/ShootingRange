using System.Windows.Controls;
using Gui.Wpf;

namespace ShootingRange.ServiceDesk.View
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : CustomWindow
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    public void SetPage(UserControl page)
    {
      Content = page;
    }
  }
}
