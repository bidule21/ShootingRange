using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ShootingRange.BusinessObjects;

namespace ShootingRange.ServiceDesk.View.Controls
{
  /// <summary>
  /// Interaction logic for UcEditPerson.xaml
  /// </summary>
  public partial class UcEditPerson : UserControl
  {

    private static FrameworkPropertyMetadata propertymetadata =
      new FrameworkPropertyMetadata(null,
        FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
        FrameworkPropertyMetadataOptions.Journal,
        null,
        null,
        false,
        UpdateSourceTrigger.PropertyChanged);
    public UcEditPerson()
    {
      InitializeComponent();

    }

    public static readonly DependencyProperty PersonProperty =
      DependencyProperty.Register("Person", typeof(Person), typeof(UcEditPerson), propertymetadata);

    public Person Person
    {
      get { return GetValue(PersonProperty) as Person; }
      set { SetValue(PersonProperty, value); }
    }
  }
}
