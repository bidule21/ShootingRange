using System.ComponentModel;
using System.Runtime.CompilerServices;
using ShootingRange.BusinessObjects.Properties;

namespace ShootingRange.ServiceDesk.ViewModel
{
  public class GroupingViewModel : INotifyPropertyChanged 
  {

    private int _collectionShooterId;
    public int CollectionShooterId
    {
      get { return _collectionShooterId; }
      set
      {
        if (value != _collectionShooterId)
        {
          _collectionShooterId = value;
          OnPropertyChanged("CollectionShooterId");
        }
      }
    }

    private int _groupingId;
    public int GroupingId
    {
      get { return _groupingId; }
      set
      {
        if (value != _groupingId)
        {
          _groupingId = value;
          OnPropertyChanged("GroupingId");
        }
      }
    }

    private string _groupingName;
    public string GroupingName
    {
      get { return _groupingName; }
      set
      {
        if (value != _groupingName)
        {
          _groupingName = value;
          OnPropertyChanged("GroupingName");
        }
      }
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