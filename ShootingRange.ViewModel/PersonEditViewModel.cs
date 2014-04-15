using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ShootingRange.BusinessObjects;
using ShootingRange.BusinessObjects.Annotations;
using ShootingRange.Common.Modules;
using ShootingRange.Engine;
using ShootingRange.Repository;

namespace ShootingRange.ViewModel
{
  public class PersonEditViewModel : INotifyPropertyChanged
  {
    private IPersonDataStore _repositoy;
    private ShootingRangeEvents _events;

    public PersonEditViewModel()
    {
      //IConfigurationFactory configFactory = ConfigurationFactoryProvider.GetConfigurationFactory();
      //_repositoy = configFactory.GetPersonRepository();
      //_events = configFactory.GetEvents();
      //_events.SelectedPersonChanged += SelectedPersonChanged;

      //Person = new Person();
    }

    //private void SelectedPersonChanged(int selectedPersonId)
    //{
    //  //Person = _repositoy.FindById(selectedPersonId);
    //}

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
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
