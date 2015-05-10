using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ShootingRange.BusinessObjects;
using ShootingRange.BusinessObjects.Properties;

namespace ShootingRange.ServiceDesk.ViewModel
{
  public class PersonViewModel : INotifyPropertyChanged
  {


    private Shooter _selectedShooter;
    public Shooter SelectedShooter
    {
      get { return _selectedShooter; }
      set
      {
        if (value != _selectedShooter)
        {
          _selectedShooter = value;
          OnPropertyChanged("SelectedShooter");
        }
      }
    }

    private ObservableCollection<Shooter> _shooters;
    public ObservableCollection<Shooter> Shooters
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

    private Person _person;
    public Person Person
    {
      get { return _person; }
      set
      {
        if (value != _person)
        {
          _person = value;
          OnPropertyChanged("Person");
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