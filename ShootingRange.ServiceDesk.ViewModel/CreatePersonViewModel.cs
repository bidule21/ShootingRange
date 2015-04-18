using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ShootingRange.ServiceDesk.ViewModel
{
  public class CreatePersonViewModel :INotifyPropertyChanged
  {

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