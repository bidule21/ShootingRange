using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using Gui.ViewModel;

namespace ShootingRange.ServiceDesk.ViewModel
{
  public class DialogHelper
  {
    public static bool? ShowDialog<T>(IWindow parent, T ucViewModel, string title)
    {
      UcDialogViewModel dialogViewModel = new UcDialogViewModel();
      dialogViewModel.LoadUserControl(ucViewModel, title);

      IWindow window = ViewServiceLocator.ViewService.ExecuteFunction<UcDialogViewModel, IWindow>(parent,
        dialogViewModel);

      bool? result = window.ShowDialog();
      return result;
    }

    public static void ShowErrorDialog(IWindow parent, string caption, string messageText)
    {
      ErrorDialogViewModel vm = new ErrorDialogViewModel
      {
        Caption = caption,
        MessageText = messageText
      };

      ViewServiceLocator.ViewService.ExecuteAction(parent, vm);
    }

    public static void ShowMessageDialog(IWindow parent, string caption, string messageText)
    {
      MessageDialogViewModel vm = new MessageDialogViewModel
      {
        Caption = caption,
        MessageText = messageText
      };

      ViewServiceLocator.ViewService.ExecuteAction(parent, vm);
    }
  }
}