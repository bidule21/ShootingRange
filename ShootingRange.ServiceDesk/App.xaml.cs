﻿using System.Windows;
using Gui.ViewModel;
using Gui.Wpf;
using ShootingRange.ServiceDesk.View;
using ShootingRange.ServiceDesk.ViewModel;

namespace ShootingRange.ServiceDesk
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      ConfigureContainer();
      ComposeObjects();
      Current.MainWindow.Show();
    }

    private void ComposeObjects()
    {
      IViewService viewService = ViewServiceLocator.ViewService;
      MainWindowViewModel viewModel = new MainWindowViewModel();
      Window mainWindow = (Window) viewService.ExecuteFunction<MainWindowViewModel, IWindow>(null, viewModel);
      Current.MainWindow = mainWindow;
    }

    public static void ConfigureContainer()
    {
      ViewService vs = new ViewService();
      ViewServiceLocator.ViewService = vs;
      ServiceDeskViewServiceHandler vsh = new ServiceDeskViewServiceHandler();

      #region Windows and Dialogs
      vs.RegisterFunction<MainWindowViewModel, IWindow>(vsh.CreateMainWindow);
      vs.RegisterFunction<UcDialogViewModel, IWindow>(vsh.CreateUserControlDialog);

      #endregion

      #region Primary Pages

      vs.RegisterFunction<PersonsPageViewModel, IPage>(vsh.CreatePersonsPage);
      vs.RegisterFunction<ShootersPageViewModel, IPage>(vsh.CreateShootersPage);
      vs.RegisterFunction<GroupsPageViewModel, IPage>(vsh.CreateGroupsPage);
      vs.RegisterFunction<RankingsPageViewModel, IPage>(vsh.CreateRankingsPage);

      #endregion

      #region UserControl Pages

      vs.RegisterFunction<CreatePersonViewModel, IPage>(vsh.CreatePersonPage);

      #endregion
    }
  }
}
