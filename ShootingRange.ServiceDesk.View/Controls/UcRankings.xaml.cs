using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Gui.ViewModel;
using ShootingRange.Ranking;

namespace ShootingRange.ServiceDesk.View.Controls
{
  /// <summary>
  /// Interaction logic for UcRankings.xaml
  /// </summary>
  public partial class UcRankings : UserControl, IPage
  {
    public UcRankings()
    {
      InitializeComponent();

      //CommandBinding cb = new CommandBinding(ApplicationCommands.Copy, CopyCmdExecuted, CopyCmdCanExecute);

      //this._RankListView.CommandBindings.Add(cb);  
    }

    void CopyCmdExecuted(object target, ExecutedRoutedEventArgs e)
    {
        string separator = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator;
        ListBox lb = e.OriginalSource as ListBox;
        string copyContent = String.Empty;
        // The SelectedItems could be ListBoxItem instances or data bound objects depending on how you populate the ListBox.  
        foreach (RankItem item in lb.SelectedItems)
        {
            copyContent += item.Rank + separator;
            copyContent += item.Label + separator;
            copyContent += item.Score + separator;
            // Add a NewLine for carriage return  
            copyContent += Environment.NewLine;
        }

        Clipboard.SetText(copyContent);
    }

    void CopyCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        ListBox lb = e.OriginalSource as ListBox;
        // CanExecute only if there is one or more selected Item.  
        if (lb.SelectedItems.Count > 0)
            e.CanExecute = true;
        else
            e.CanExecute = false;
    }  
  }
}
