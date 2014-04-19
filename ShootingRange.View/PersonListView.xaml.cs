using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
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
      PersonListItem item = (PersonListItem)ListBox.SelectedItem;
      SelectedPerson = item;

      e.Handled = true;
      RoutedEventArgs args = new RoutedEventArgs(SelectedPersonChangedEvent);
      RaiseEvent(args);
    }

    public event RoutedEventHandler SelectedPersonChanged
    {
      add { AddHandler(SelectedPersonChangedEvent, value); }
      remove { RemoveHandler(SelectedPersonChangedEvent, value); }
    }

    public ObservableCollection<PersonListItem> PersonListItems
    {
      get { return (ObservableCollection<PersonListItem>)GetValue(PersonListItemsProperty); }
      set { SetValue(PersonListItemsProperty, value); }
    }

    public static readonly DependencyProperty PersonListItemsProperty = DependencyProperty.Register("PersonListItems",
      typeof(ObservableCollection<PersonListItem>),
      typeof (PersonListView));

    public PersonListItem SelectedPerson
    {
      get { return (PersonListItem) GetValue(SelectedPersonProperty); }
      set { SetValue(SelectedPersonProperty, value); }
    }

    public static readonly DependencyProperty SelectedPersonProperty = DependencyProperty.Register("SelectedPerson",
      typeof (PersonListItem), typeof (PersonListView), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true});

    public static readonly RoutedEvent SelectedPersonChangedEvent =
      EventManager.RegisterRoutedEvent("SelectedPersonChanged",
        RoutingStrategy.Bubble,
        typeof (RoutedEventHandler),
        typeof (PersonListView));
  }
}
