using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using ShootingRange.BusinessObjects.Properties;
using ShootingRange.ConfigurationProvider;
using ShootingRange.Repository.RepositoryInterfaces;

namespace ShootingRange.ServiceDesk.ViewModel
{
  public class SelectShooterViewModel : INotifyPropertyChanged
  {

    public void Initialize()
    {
      IPersonDataStore personDataStore = ConfigurationSource.Configuration.GetPersonDataStore();
      IShooterDataStore shooterDataStore = ConfigurationSource.Configuration.GetShooterDataStore();

      IEnumerable<PersonShooterViewModel> shooters = from shooter in shooterDataStore.GetAll()
        join person in personDataStore.GetAll() on shooter.PersonId equals person.PersonId
        orderby person.FirstName
        orderby person.LastName
        select new PersonShooterViewModel
        {
          FirstName = person.FirstName,
          LastName = person.LastName,
          PersonId = person.PersonId,
          ShooterId = shooter.ShooterId,
          ShooterNumber = shooter.ShooterNumber
        };

      Shooters = new ObservableCollection<PersonShooterViewModel>(shooters);
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

    private PersonShooterViewModel _selectedShooter;
    public PersonShooterViewModel SelectedShooter
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

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      var handler = PropertyChanged;
      if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}