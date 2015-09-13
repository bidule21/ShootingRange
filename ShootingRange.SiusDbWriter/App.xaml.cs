using System.Windows;
using Autofac;
using ShootingRange.Common;
using ShootingRange.Persistence;
using ShootingRange.Repository.Repositories;
using ShootingRange.Repository.RepositoryInterfaces;
using ShootingRange.SiusData;
using ShootingRange.SiusDbWriterView;
using ShootingRange.SiusDbWriterViewModel;

namespace ShootingRange.SiusDbWriter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IContainer _container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureContainer();
            ComposeObjects();
        }

        private void ConfigureContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();

            ShootingRangeEntities entities = new ShootingRangeEntities();
            ISessionDataStore sessionDataStore = new SessionDataStore(entities);
            ISessionSubtotalDataStore sessionSubtotalDataStore = new SessionSubtotalDataStore(entities);
            IShotDataStore shotDataStore = new ShotDataStore(entities);
            IShooterDataStore shooterDataStore = new ShooterDataStore(entities);
            IPersonDataStore personDataStore = new PersonDataStore(entities);

            builder.RegisterInstance(sessionDataStore).As<ISessionDataStore>();
            builder.RegisterInstance(sessionSubtotalDataStore).As<ISessionSubtotalDataStore>();
            builder.RegisterInstance(shotDataStore).As<IShotDataStore>();
            builder.RegisterInstance(shooterDataStore).As<IShooterDataStore>();
            builder.RegisterInstance(personDataStore).As<IPersonDataStore>();
            builder.RegisterInstance(new SiusDataFileProvider(@"F:\work\ShootingRange\dumps\20150912_140726.log")).As<IShootingRange>();
            //builder.RegisterInstance(new SiusDataSocketProvider("127.0.0.1", 4000)).As<IShootingRange>();

            _container = builder.Build();
        }

        private void ComposeObjects()
        {
            MainViewModel mainViewModel = new MainViewModel(_container);
            Window mainWindow = new MainWindow();
            mainWindow.DataContext = mainViewModel;
            Current.MainWindow = mainWindow;
            Current.MainWindow.Show();
        }
    }
}
