using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using ShootingRange.UiBusinessObjects;
using ShootingRange.ViewModel;

namespace ShootingRange.View
{
  /// <summary>
  /// Interaction logic for PersonListView.xaml
  /// </summary>
  public partial class PersonListView : UserControl
  {
    public PersonListView()
    {
      InitializeComponent();
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      UiPerson item = (UiPerson)ListBox.SelectedItem;
      SelectedUiPerson = item;

      e.Handled = true;
      RoutedEventArgs args = new RoutedEventArgs(SelectedUiPersonChangedEvent);
      RaiseEvent(args);
    }

    public event RoutedEventHandler SelectedUiPersonChanged
    {
      add { AddHandler(SelectedUiPersonChangedEvent, value); }
      remove { RemoveHandler(SelectedUiPersonChangedEvent, value); }
    }

    public ObservableCollection<UiPerson> UiPeople
    {
      get { return (ObservableCollection<UiPerson>)GetValue(UiPeopleProperty); }
      set { SetValue(UiPeopleProperty, value); }
    }

    public static readonly DependencyProperty UiPeopleProperty = DependencyProperty.Register("UiPeople",
      typeof(ObservableCollection<UiPerson>),
      typeof (PersonListView));

    public UiPerson SelectedUiPerson
    {
      get { return (UiPerson) GetValue(SelectedUiPersonProperty); }
      set { SetValue(SelectedUiPersonProperty, value); }
    }

    public static readonly DependencyProperty SelectedUiPersonProperty = DependencyProperty.Register("SelectedUiPerson",
      typeof (UiPerson), typeof (PersonListView), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true});

    public static readonly RoutedEvent SelectedUiPersonChangedEvent =
      EventManager.RegisterRoutedEvent("SelectedUiPersonChanged",
        RoutingStrategy.Bubble,
        typeof (RoutedEventHandler),
        typeof (PersonListView));
  }
}
