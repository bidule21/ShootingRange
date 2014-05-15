using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DotNetToolbox.RelayCommand;
using ShootingRange.BusinessObjects;
using ShootingRange.ConfigurationProvider;
using ShootingRange.Repository.Repositories;
using ShootingRange.Service.Interface;
using ShootingRange.UiBusinessObjects.Annotations;

namespace ShootingRange.ViewModel
{
  public class ParticipationCreateViewModel : INotifyPropertyChanged
  {
    private IParticipationTypeDataStore _participationTypeDataStore;
    private IParticipationDataStore _participationDataStore;
    private IWindowService _windowService;

    public ParticipationCreateViewModel()
    {
      if (!DesignTimeHelper.IsInDesignMode)
      {
        IConfiguration config = ConfigurationSource.Configuration;
        _participationTypeDataStore = config.GetParticipationTypeDataStore();
        _participationDataStore = config.GetParticipationDataStore();
        _windowService = config.GetWindowService();
        LoadParticipationTypeList();
      }

      OkCommand = new RelayCommand<ParticipationDraft>(ExecuteCreateParticipationCommand, CanExecuteCreateParticipationCommand);
      CancelCommand = new RelayCommand<object>(ExecuteCancelCommand);
    }

    private void ExecuteCancelCommand(object obj)
    {
      _windowService.CloseCreateParticipationWindow();
    }

    private void ExecuteCreateParticipationCommand(ParticipationDraft participationDraft)
    {
      Participation participation = new Participation
      {
        ParticipationDescriptionId = participationDraft.ParticipationType.ParticipationTypeId,
        ParticipationName = participationDraft.ParticipationName
      };

      _participationDataStore.Create(participation);
      _windowService.CloseCreateParticipationWindow();
    }

    private bool CanExecuteCreateParticipationCommand(ParticipationDraft participationDraft)
    {
      return participationDraft != null && !string.IsNullOrWhiteSpace(participationDraft.ParticipationName) &&
             participationDraft.ParticipationType != null;
    }

    public ICommand OkCommand { get; private set; }
    public ICommand CancelCommand { get; private set; }

    private void LoadParticipationTypeList()
    {
      Func<ParticipationType, ParticipationTypeListItem> selector = participationType => new ParticipationTypeListItem
      {
        ParticipationTypeId = participationType.ParticipationTypeId,
        ParticipationTypeName = participationType.ParticipationTypeName
      };

      AvailableParticipationTypes = new ObservableCollection<ParticipationTypeListItem>(_participationTypeDataStore.GetAll().Select(selector));
    }

    
    private ParticipationDraft _participationDraft;
    public ParticipationDraft ParticipationDraft
    {
      get { return new ParticipationDraft
      {
        ParticipationName = ParticipationName,
        ParticipationType = SelectedAvailableParticipationType
      }; }
    }

    private string _participationName;
    public string ParticipationName
    {
      get { return _participationName; }
      set
      {
        if (value != _participationName)
        {
          _participationName = value;
          OnPropertyChanged("ParticipationName");
          OnPropertyChanged("ParticipationDraft");
        }
      }
    }

    private ObservableCollection<ParticipationTypeListItem> _availableParticipationTypes;
    public ObservableCollection<ParticipationTypeListItem> AvailableParticipationTypes
    {
      get { return _availableParticipationTypes; }
      set
      {
        if (value != _availableParticipationTypes)
        {
          _availableParticipationTypes = value;
          OnPropertyChanged("AvailableParticipationTypes");
        }
      }
    }

    private ParticipationTypeListItem _selectedAvailableParticipationType;
    public ParticipationTypeListItem SelectedAvailableParticipationType
    {
      get { return _selectedAvailableParticipationType; }
      set
      {
        if (value != _selectedAvailableParticipationType)
        {
          _selectedAvailableParticipationType = value;
          OnPropertyChanged("SelectedAvailableParticipationType"); 
          OnPropertyChanged("ParticipationDraft");
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
