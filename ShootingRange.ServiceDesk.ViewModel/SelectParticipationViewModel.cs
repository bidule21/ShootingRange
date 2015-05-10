using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using ShootingRange.BusinessObjects.Properties;
using ShootingRange.ConfigurationProvider;
using ShootingRange.Repository.RepositoryInterfaces;

namespace ShootingRange.ServiceDesk.ViewModel
{
  public class SelectParticipationViewModel : INotifyPropertyChanged
  {
    private IParticipationDataStore _participationDataStore;

    public SelectParticipationViewModel()
    {
      _participationDataStore = ConfigurationSource.Configuration.GetParticipationDataStore();
    }

    public void Initialize()
    {
      Participations =
        new ObservableCollection<ParticipationViewModel>(
          _participationDataStore.GetAll()
            .Select(x => new ParticipationViewModel(x.ParticipationId) {ProgramName = x.ParticipationName}));
    }

    private string _title;

    public string Title
    {
      get { return _title; }
      set
      {
        if (value != _title)
        {
          _title = value;
          OnPropertyChanged("Title");
        }
      }
    }

    private ObservableCollection<ParticipationViewModel> _participations;

    public ObservableCollection<ParticipationViewModel> Participations
    {
      get { return _participations; }
      set
      {
        if (value != _participations)
        {
          _participations = value;
          OnPropertyChanged("Participations");
        }
      }
    }

    private ParticipationViewModel _selectedParticipation;

    public ParticipationViewModel SelectedParticipation
    {
      get { return _selectedParticipation; }
      set
      {
        if (value != _selectedParticipation)
        {
          _selectedParticipation = value;
          OnPropertyChanged("SelectedParticipation");
        }
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      var handler = PropertyChanged;
      if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}