using System.Linq.Expressions;
using System.Windows;
using Gui.ViewModel;
using Gui.Wpf;
using ShootingRange.ServiceDesk.View.Controls;
using ShootingRange.ServiceDesk.View.Dialogs;
using ShootingRange.ServiceDesk.ViewModel;

namespace ShootingRange.ServiceDesk.View
{
  public class ServiceDeskViewServiceHandler : ViewServiceHandler
  {
    public MainWindow CreateMainWindow(IWindow owner, MainWindowViewModel dataContext)
    {
      return GetOwnedWindow<MainWindow>((Window) owner, dataContext);
    }

    public UcDialog CreateUserControlDialog(IWindow owner, UcDialogViewModel dataContext)
    {
      return GetOwnedWindow<UcDialog>((Window)owner, dataContext);
    }

    public UcPersons CreatePersonsPage(IWindow owner, PersonsPageViewModel dataContext)
    {
      return GetUserControl<UcPersons>((Window)owner, dataContext);
    }

    public IPage CreateShootersPage(IWindow owner, ShootersPageViewModel dataContext)
    {
      return GetUserControl<UcShooters>((Window) owner, dataContext);
    }

    public IPage CreateGroupsPage(IWindow owner, GroupsPageViewModel dataContext)
    {
      return GetUserControl<UcGroups>((Window)owner, dataContext);
    }

    public IPage CreateRankingsPage(IWindow owner, RankingsPageViewModel dataContext)
    {
      return GetUserControl<UcRankings>((Window)owner, dataContext);
    }

    public IPage CreateResultsPage(IWindow owner, ResultsPageViewModel dataContext)
    {
      return GetUserControl<UcResults>((Window)owner, dataContext);
    }

    public IPage CreatePersonPage(IWindow owner, CreatePersonViewModel dataContext)
    {
      return GetUserControl<UcCreatePerson>((Window)owner, dataContext);
    }

    public IPage CreateGroupingPage(IWindow owner, CreateGroupingViewModel dataContext)
    {
      return GetUserControl<UcCreateGrouping>((Window) owner, dataContext);
    }

    public IPage CreateSelectParticipationPage(IWindow owner, SelectParticipationViewModel dataContext)
    {
      return GetUserControl<UcSelectParticipation>((Window) owner, dataContext);
    }

    public IPage CreateSelectGroupingPage(IWindow owner, SelectGroupingViewModel dataContext)
    {
      return GetUserControl<UcSelectGrouping>((Window)owner, dataContext);
    }

    public IPage CreateSelectShooterPage(IWindow owner, SelectShooterViewModel dataContext)
    {
      return GetUserControl<UcSelectShooter>((Window) owner, dataContext);
    }

    public bool? ShowNativeDialog(IWindow owner, YesNoMessageBoxViewModel dataContext)
    {
      MessageBoxResult result = MessageBox.Show((Window) owner,
        dataContext.MessageBoxText,
        dataContext.Caption,
        MessageBoxButton.YesNo,
        MessageBoxImage.Question,
        dataContext.DefaultYes ? MessageBoxResult.Yes : MessageBoxResult.No);

      if (result == MessageBoxResult.Yes)
        return true;
      if (result == MessageBoxResult.No)
        return false;

      return null;
    }

    public void ShowErrorDialog(IWindow owner, ErrorDialogViewModel vm)
    {
      MessageBox.Show((Window)owner, vm.MessageText, vm.Caption, MessageBoxButton.OK, MessageBoxImage.Error);
    }

    public void ShowMessageDialog(IWindow owner, MessageDialogViewModel vm)
    {
      MessageBox.Show((Window)owner, vm.MessageText, vm.Caption, MessageBoxButton.OK, MessageBoxImage.Information);
    }
  }
}