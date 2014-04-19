using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ShootingRange.BusinessObjects;
using ShootingRange.PersonAdminGui.Properties;
using ShootingRange.Repository;

namespace ShootingRange.PersonAdminGui
{
  public class PersonViewModel : INotifyPropertyChanged 
  {
    private IPersonDataStore _repository;

    public PersonViewModel()
    {
      //IConfiguration configuration = ConfigurationSource.Configuration;
      //_repository = configuration.GetPersonRepository();
      //People = new ObservableCollection<Person>(_repository.GetAll());
      People = new ObservableCollection<Person>(new List<Person>());
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

    private ObservableCollection<Person> _people;
    public ObservableCollection<Person> People
    {
      get { return _people; }
      set
      {
        if (value != _people)
        {
          _people = value;
          OnPropertyChanged("People");
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
