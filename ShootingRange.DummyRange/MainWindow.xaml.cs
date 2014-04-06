using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ShootingRange.Common;
using ShootingRange.Engine;

namespace ShootingRange.DummyRange
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    ShootingRangeEngine _engine;
    private DummyRange _range;

    public MainWindow()
    {
      InitializeComponent();

      IConfigurationFactory config = new DummyRangeConfigurationFactory();
      _engine = new ShootingRangeEngine(config);
       _range = (DummyRange)config.GetShootingRange();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      _range.ProcessShooterNumber(54);
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
      _range.ProcessProgramNumber(99);
    }

    private void Button_Click_2(object sender, RoutedEventArgs e)
    {
      _range.ProcessShot(101);
    }
  }
}
