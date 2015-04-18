using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Gui.ViewModel;
using Gui.Wpf;

namespace ShootingRange.ServiceDesk.View.Dialogs
{
  /// <summary>
  /// Interaction logic for UcDialog.xaml
  /// </summary>
  public partial class UcDialog : CustomWindow, IWindow
  {
    public UcDialog()
    {
      InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = true;
    }
  }
}
