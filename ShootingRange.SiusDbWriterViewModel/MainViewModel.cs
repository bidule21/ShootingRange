using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using DotNetToolbox.RelayCommand;
using ShootingRange.Common;
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
      LastSiusMessage = "Welcome!";
      LogCollection = new ObservableCollection<string>();

      Host = "localhost";
      Port = 4000;
      Server = "localhost";
      User = "root";
      Password = "";
      Database = "shootingrange";

      if (!DesignTimeHelper.IsInDesignMode)
      {

      }

      StartProcessingOnConnect = false;
      ConnectSiusCommand = new RelayCommand<object>(ExecuteConnectSiusCommand, CanExecuteConnectSiusCommand);
      StartProcessingCommand = new RelayCommand<object>(ExecuteStartProcessingCommand, CanExecuteStartProcessingCommand);
    }

    private bool CanExecuteConnectSiusCommand(object obj)
    {
      return _engine == null;
    }

    private void ExecuteStartProcessingCommand(object obj)
    {
      _engine.StartProcessing();
    }

    private bool CanExecuteStartProcessingCommand(object obj)
    {
      return _engine != null && !_engine.IsProcessing;
    }

    private void EngineOnLog(object sender, LogEventArgs e)
    {
      LastSiusMessage = e.Message;
      LogCollection.Add(e.Message);

      while (LogCollection.Count > 20)
        LogCollection.RemoveAt(0);
    }

    private void ExecuteConnectSiusCommand(object obj)
    {
      try
      {
        IConfiguration config = ConfigurationSource.Configuration;
        _engine = new ShootingRangeEngine(config);
        _engine.Log += EngineOnLog;

        if (StartProcessingOnConnect)
          ExecuteStartProcessingCommand(null);

        _engine.ConnectToSius();
      }
      catch (Exception e)
      {
        LastSiusMessage = e.Message;
      }
    }

    public ICommand ConnectSiusCommand { get; private set; }

    public ICommand StartProcessingCommand { get; private set; }

    private string _lastSiusMessage;

    
    private bool _startProcessingOnConnect;
    public bool StartProcessingOnConnect
    {
      get { return _startProcessingOnConnect; }
      set
      {
        if (value != _startProcessingOnConnect)
        {
          _startProcessingOnConnect = value;
          OnPropertyChanged("StartProcessingOnConnect");
        }
      }
    }
    
    private ObservableCollection<string> _logCollection;
    public ObservableCollection<string> LogCollection
    {
      get { return _logCollection; }
      set
      {
        if (value != _logCollection)
        {
          _logCollection = value;
          OnPropertyChanged("LogCollection");
        }
      }
    }

    public string LastSiusMessage
    {
      get { return _lastSiusMessage; }
      set
      {
        if (value != _lastSiusMessage)
        {
          _lastSiusMessage = value;
          OnPropertyChanged("LastSiusMessage");
        }
      }
    }

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