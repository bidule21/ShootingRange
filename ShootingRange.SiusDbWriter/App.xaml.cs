using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Unity;
using ShootingRange.Configuration;
using ShootingRange.ConfigurationProvider;
using ShootingRange.SiusDbWriterView;

namespace ShootingRange.SiusDbWriter
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
      _container.RegisterType<IConfiguration, DefaultConfiguration>(new ContainerControlledLifetimeManager());
    }

    private void ComposeObjects()
    {
      ConfigurationSource.Configuration = _container.Resolve<IConfiguration>();
      Application.Current.MainWindow = _container.Resolve<MainWindow>();
      Application.Current.MainWindow.Title = "SiusDbWriter";
    }
  }
}
