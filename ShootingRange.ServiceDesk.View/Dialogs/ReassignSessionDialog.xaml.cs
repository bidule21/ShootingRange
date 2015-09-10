using System.Windows;
using Gui.ViewModel;

namespace ShootingRange.ServiceDesk.View.Dialogs
{
    /// <summary>
    /// Interaction logic for ReassignSessionDialog.xaml
    /// </summary>
    public partial class ReassignSessionDialog : Window, IWindow
    {
        public ReassignSessionDialog()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
