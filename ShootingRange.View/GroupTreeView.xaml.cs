using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ShootingRange.ViewModel;

namespace ShootingRange.View
{
  /// <summary>
  /// Interaction logic for GroupTreeView.xaml
  /// </summary>
  public partial class GroupTreeView : UserControl
  {
    public GroupTreeView()
    {
      InitializeComponent();
    }

    public ObservableCollection<GroupTreeItem> GroupTreeItems
    {
      get { return (ObservableCollection<GroupTreeItem>) GetValue(GroupTreeItemsProperty); }
      set {  SetValue(GroupTreeItemsProperty, value); }
    }

    public static readonly DependencyProperty GroupTreeItemsProperty = DependencyProperty.Register("GroupTreeItems",
      typeof (ObservableCollection<GroupTreeItem>),
      typeof (GroupTreeView));
  }
}
