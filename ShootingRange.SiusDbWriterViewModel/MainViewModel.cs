using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using DotNetToolbox.RelayCommand;
using ShootingRange.ConfigurationProvider;
using ShootingRange.Engine;
using ShootingRange.Repository.Repositories;
using ShootingRange.SiusDbWriterViewModel.Properties;
using ShootingRange.ViewModel;

namespace ShootingRange.SiusDbWriterViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
      private IShotDataStore _shotDataStore;
      private ShootingRangeEngine _engine;

      public MainViewModel()
      {
        Host = "localhost";
        Port = 4000;
        Server = "localhost";
        User = "root";
        Password = "";
        Database = "shootingrange";

        if (!DesignTimeHelper.IsInDesignMode)
        {
          IConfiguration config = ConfigurationSource.Configuration;
          _engine = new ShootingRangeEngine(config);
        }

        ConnectSiusCommand = new RelayCommand<object>(ExecuteConnectSiusCommand);
        ConnectDatabaseCommand = new RelayCommand<object>(ExecuteConnectDatabaseCommand);
      }

      private void ExecuteConnectDatabaseCommand(object obj)
      {
        throw new System.NotImplementedException();
      }

      private void ExecuteConnectSiusCommand(object obj)
      {
        _engine.ConnectToSius();
      }

      public ICommand ConnectSiusCommand { get; private set; }
      public ICommand ConnectDatabaseCommand { get; private set; }
      
      private string _database;
      public string Database
      {
        get { return _database; }
        set
        {
          if (value != _database)
          {
            _database = value;
            OnPropertyChanged("Database");
          }
        }
      }
      
      private string _password;
      public string Password
      {
        get { return _password; }
        set
        {
          if (value != _password)
          {
            _password = value;
            OnPropertyChanged("Password");
          }
        }
      }

      private string _user;
      public string User
      {
        get { return _user; }
        set
        {
          if (value != _user)
          {
            _user = value;
            OnPropertyChanged("User");
          }
        }
      }

      private string _server;
      public string Server
      {
        get { return _server; }
        set
        {
          if (value != _server)
          {
            _server = value;
            OnPropertyChanged("Server");
          }
        }
      }
      
      private int _port;
      public int Port
      {
        get { return _port; }
        set
        {
          if (value != _port)
          {
            _port = value;
            OnPropertyChanged("Port");
          }
        }
      }

      private string _host;
      public string Host
      {
        get { return _host; }
        set
        {
          if (value != _host)
          {
            _host = value;
            OnPropertyChanged("Host");
          }
        }
      }

      public event PropertyChangedEventHandler PropertyChanged;

      [NotifyPropertyChangedInvocator]
      protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
      {
        PropertyChangedEventHandler handler = PropertyChanged;
        if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
      }
    }
}
