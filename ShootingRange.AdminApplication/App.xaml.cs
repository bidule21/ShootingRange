using System.Windows;
using Microsoft.Practices.Unity;
using ShootingRange.Configuration;
using ShootingRange.ConfigurationProvider;
using ShootingRange.Repository;
using ShootingRange.Service;
using ShootingRange.Service.Interface;
using ShootingRange.View;

namespace ShootingRange.AdminApplication
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    private IUnityContainer _container;

    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      ConfigureContainer();
      ComposeObjects();
      Application.Current.MainWindow.Show();
    }

    private void ConfigureContainer()
    {
      _container = new UnityContainer();
      _container.RegisterType<IWindowService, WindowService>();
      _container.RegisterType<IPersonDataStore, FakePersonDataStore>(new ContainerControlledLifetimeManager());
      _container.RegisterType<IConfiguration, DummyConfiguration>(new ContainerControlledLifetimeManager());
    }

    private void ComposeObjects()
    {
      ConfigurationSource.Configuration = _container.Resolve<IConfiguration>();
      Application.Current.MainWindow = _container.Resolve<MainWindow>();
      Application.Current.MainWindow.Title = "Shooting Range Admin";
    }
  }
}
