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

    public void Initialize(int currentGroupTypeId)
    {
      IPersonDataStore personDataStore = ConfigurationSource.Configuration.GetPersonDataStore();
      IShooterDataStore shooterDataStore = ConfigurationSource.Configuration.GetShooterDataStore();
      ICollectionShooterDataStore collectionShooterDataStore = ConfigurationSource.Configuration.GetCollectionShooterDataStore();
      IShooterCollectionParticipationDataStore shooterCollectionParticipationDataStore = ConfigurationSource.Configuration.GetShooterCollectionParticipationDataStore();

      var personToParticipationTypes = from shooter in shooterDataStore.GetAll()
        join person in personDataStore.GetAll() on shooter.PersonId equals person.PersonId
        join cs in collectionShooterDataStore.GetAll() on shooter.ShooterId equals cs.ShooterId into gj
        select new
        {
          Person = person,
          Shooter = shooter,
          ParticipationTypes = from cs in gj
            join scp in shooterCollectionParticipationDataStore.GetAll() on cs.ShooterCollectionId equals
              scp.ShooterCollectionId
            select scp.ParticipationId
        }
        ;

      IEnumerable<PersonShooterViewModel> shooters = (from grouped in personToParticipationTypes
        where grouped.ParticipationTypes.All(_ => _ != currentGroupTypeId)
        orderby grouped.Shooter.ShooterNumber
        orderby grouped.Person.FirstName
        orderby grouped.Person.LastName
        select
          new PersonShooterViewModel
          {
            FirstName = grouped.Person.FirstName,
            LastName = grouped.Person.LastName,
            PersonId = grouped.Person.PersonId,
            ShooterId = grouped.Shooter.ShooterId,
            ShooterNumber = grouped.Shooter.ShooterNumber,
            DateOfBirth = grouped.Person.DateOfBirth

          });

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