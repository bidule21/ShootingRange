using System.Windows;
using Gui.ViewModel;

namespace ShootingRange.ServiceDesk.View.Dialogs
{
    /// <summary>
    /// Interaction logic for ReassignSessionDialog.xaml
    /// </summary>
    public partial class ReassignProgramNumber : Window, IWindow
    {
        public ReassignProgramNumber()
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
