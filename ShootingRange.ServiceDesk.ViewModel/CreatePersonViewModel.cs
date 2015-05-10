using System.ComponentModel;
using System.Runtime.CompilerServices;
using ShootingRange.BusinessObjects;

namespace ShootingRange.ServiceDesk.ViewModel
{
  public class CreatePersonViewModel : INotifyPropertyChanged
  {
    public CreatePersonViewModel()
    {
      Person = new Person();
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

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      var handler = PropertyChanged;
      if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}