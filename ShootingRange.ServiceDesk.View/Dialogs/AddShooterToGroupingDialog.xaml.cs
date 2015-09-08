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

namespace ShootingRange.ServiceDesk.View.Dialogs
{
    /// <summary>
    /// Interaction logic for AddShooterToGroupingDialog.xaml
    /// </summary>
    public partial class AddShooterToGroupingDialog : Window, IWindow
    {
        public AddShooterToGroupingDialog()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            IconHelper.RemoveIcon(this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
