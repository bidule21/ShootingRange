using System.Windows;
using System.Windows.Controls;

namespace ShootingRange.View
{
    /// <summary>
    /// Interaction logic for PersonEditControl.xaml
    /// </summary>
    public partial class PersonEditControl : UserControl
    {
      public PersonEditControl()
      {
        InitializeComponent();
      }

      public string FirstName
      {
        get { return (string) GetValue(FirstNameProperty); }
        set { SetValue(FirstNameProperty, value); }
      }

      public static readonly DependencyProperty FirstNameProperty = DependencyProperty.Register("FirstName",
        typeof (string),
        typeof (PersonEditControl));

      public string LastName
      {
        get { return (string)GetValue(LastNameProperty); }
        set { SetValue(LastNameProperty, value); }
      }

      public static readonly DependencyProperty LastNameProperty = DependencyProperty.Register("LastName",
        typeof(string),
        typeof(PersonEditControl));
    }
}
