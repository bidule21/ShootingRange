using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ShootingRange.BusinessObjects.Properties;

namespace ShootingRange.ServiceDesk.ViewModel
{
  public class ShooterCollectionViewModel : INotifyPropertyChanged
  {

    private ObservableCollection<PersonShooterViewModel> _shooters;
    public ObservableCollection<PersonShooterViewModel> Shooters
    {
      get { return _shooters; }
      set
      {
        if (value != _shooters)
        {
          _shooters = value;
          OnPropertyChanged("Shooters");
        }
      }
    }

    private int _shooterCollectionId;
    public int ShooterCollectionId
    {
      get { return _shooterCollectionId; }
      set
      {
        if (value != _shooterCollectionId)
        {
          _shooterCollectionId = value;
          OnPropertyChanged("ShooterCollectionId");
        }
      }
    }

    private string _collectionName;
    public string CollectionName
    {
      get { return _collectionName; }
      set
      {
        if (value != _collectionName)
        {
          _collectionName = value;
          OnPropertyChanged("CollectionName");
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