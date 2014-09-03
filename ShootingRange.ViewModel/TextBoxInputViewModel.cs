using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DotNetToolbox.RelayCommand;
using ShootingRange.BusinessObjects;
using ShootingRange.Common;
using ShootingRange.ConfigurationProvider;
using ShootingRange.Repository.RepositoryInterfaces;
using ShootingRange.Service.Interface;
using ShootingRange.UiBusinessObjects;
using ShootingRange.UiBusinessObjects.Annotations;

namespace ShootingRange.ViewModel
{
  public class TextBoxInputViewModel : INotifyPropertyChanged
  {
    private IShooterCollectionDataStore _shooterCollectionDataStore;
    private IShooterCollectionParticipationDataStore _shooterCollectionParticipationDataStore;

    public TextBoxInputViewModel()
    {
      if (!DesignTimeHelper.IsInDesignMode)
      {
        IConfiguration config = ConfigurationSource.Configuration;
        _shooterCollectionDataStore = config.GetShooterCollectionDataStore();
        _shooterCollectionParticipationDataStore = config.GetShooterCollectionParticipationDataStore();
        _uiEvents = config.GetUIEvents();
        _windowService = config.GetWindowService();
      }

      AddCommand = new RelayCommand<string>(ExecuteAddCommand, CanExecuteAddCommand);
      CancelCommand = new RelayCommand<object>((_) => _windowService.CloseTextBoxInputDialog());
    }

    private bool CanExecuteAddCommand(string obj)
    {
      return !string.IsNullOrWhiteSpace(obj);
    }

    private void ExecuteAddCommand(string obj)
    {
      ShooterCollection sc = new ShooterCollection
      {
        CollectionName = obj
      };
      _shooterCollectionDataStore.Create(sc);

      UiEventsDelegate<UiParticipation> handler = _uiEvents.FetchSelectedParticipation;
      if (handler != null)
      {
        UiParticipation p = handler();
        if (p != null)
        {
          ShooterCollectionParticipation scp = new ShooterCollectionParticipation
          {
            ParticipationId = p.ParticipationId,
            ShooterCollectionId = sc.ShooterCollectionId
          };
          _shooterCollectionParticipationDataStore.Create(scp);
        }
      }

      _windowService.CloseTextBoxInputDialog();
    }


    public ICommand AddCommand { get; set; }
    public ICommand CancelCommand { get; set; }

    private string _input;
    private UIEvents _uiEvents;
    private IWindowService _windowService;

    public string Input
    {
      get { return _input; }
      set
      {
        if (value != _input)
        {
          _input = value;
          OnPropertyChanged("Input");
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
