using System.Windows;
using ShootingRange.Service.Interface;

namespace ShootingRange.View
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window, IMainWindow
  {
    public MainWindow()
    {
      InitializeComponent();
    }
  }
}
