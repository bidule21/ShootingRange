using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using ShootingRange.BusinessObjects.Properties;
using ShootingRange.ConfigurationProvider;
using ShootingRange.Repository.RepositoryInterfaces;

namespace ShootingRange.ServiceDesk.ViewModel
{
  public class SelectGroupingViewModel : INotifyPropertyChanged
  {
    private IParticipationDataStore _participationDataStore;
    private IShooterCollectionParticipationDataStore _shooterCollectionParticipationDataStore;
    private IShooterCollectionDataStore _shooterCollectionDataStore;

    public SelectGroupingViewModel()
    {
      _participationDataStore = ConfigurationSource.Configuration.GetParticipationDataStore();
      _shooterCollectionParticipationDataStore = ConfigurationSource.Configuration.GetShooterCollectionParticipationDataStore();
      _shooterCollectionDataStore = ConfigurationSource.Configuration.GetShooterCollectionDataStore();
    }

    public void Initialize()
    {
      Groupings = new ObservableCollection<GroupingViewModel>(
        from participation in _participationDataStore.GetAll().Where(p => p.AllowCollectionParticipation)
        join shooterCollectionParticipation in _shooterCollectionParticipationDataStore.GetAll() on
          participation.ParticipationId equals shooterCollectionParticipation.ParticipationId
        join shooterCollection in _shooterCollectionDataStore.GetAll() on
          shooterCollectionParticipation.ShooterCollectionId equals shooterCollection.ShooterCollectionId
        orderby shooterCollection.CollectionName
        orderby participation.ParticipationName
        select new GroupingViewModel
        {
          GroupingId = shooterCollection.ShooterCollectionId,
          GroupingName = shooterCollection.CollectionName,
          ParticipationName = participation.ParticipationName
        }
        );
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

    private ObservableCollection<GroupingViewModel> _groupings;

    public ObservableCollection<GroupingViewModel> Groupings
    {
      get { return _groupings; }
      set
      {
        if (value != _groupings)
        {
          _groupings = value;
          OnPropertyChanged("Groupings");
        }
      }
    }

    private GroupingViewModel _selectedGrouping;

    public GroupingViewModel SelectedGrouping
    {
      get { return _selectedGrouping; }
      set
      {
        if (value != _selectedGrouping)
        {
          _selectedGrouping = value;
          OnPropertyChanged("SelectedGrouping");
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