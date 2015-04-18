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

    public IPage CreatePersonPage(IWindow owner, CreatePersonViewModel dataContext)
    {
      return GetUserControl<UcCreatePerson>((Window)owner, dataContext);
    }
  }
}