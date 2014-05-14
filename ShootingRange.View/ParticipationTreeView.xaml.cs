using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using ShootingRange.ViewModel;

namespace ShootingRange.View
{
  /// <summary>
  /// Interaction logic for ParticipationTreeView.xaml
  /// </summary>
  public partial class ParticipationTreeView : UserControl
  {
    public ParticipationTreeView()
    {
      InitializeComponent();
    }

    public ObservableCollection<ParticipationTreeItem> ParticipationTreeItems
    {
      get { return (ObservableCollection<ParticipationTreeItem>) GetValue(ParticipationTreeItemsProperty); }
      set {  SetValue(ParticipationTreeItemsProperty, value); }
    }

    public static readonly DependencyProperty ParticipationTreeItemsProperty = DependencyProperty.Register("ParticipationTreeItems",
      typeof (ObservableCollection<ParticipationTreeItem>),
      typeof (ParticipationTreeView));
  }
}
