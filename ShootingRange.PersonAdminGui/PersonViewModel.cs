using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Repository;
using ShootingRange.BusinessObjects;
using ShootingRange.PersonAdminGui.Annotations;

namespace ShootingRange.PersonAdminGui
{
  public class PersonViewModel : INotifyPropertyChanged 
  {
    private IRepository<Person> _repository;

    public PersonViewModel()
    {
      IUnityContainer container = new UnityContainer().LoadConfiguration();
      _repository = container.Resolve<IRepository<Person>>();
      People = new ObservableCollection<Person>(_repository.FindAll());
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
