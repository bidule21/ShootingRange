using System.ComponentModel;
using System.Runtime.CompilerServices;
using ShootingRange.BusinessObjects.Properties;

namespace ShootingRange.ServiceDesk.ViewModel
{
  public class MessageDialogViewModel : INotifyPropertyChanged
  {

    private string _messageText;
    public string MessageText
    {
      get { return _messageText; }
      set
      {
        if (value != _messageText)
        {
          _messageText = value;
          OnPropertyChanged("MessageText");
        }
      }
    }

    private string _caption;
    public string Caption
    {
      get { return _caption; }
      set
      {
        if (value != _caption)
        {
          _caption = value;
          OnPropertyChanged("Caption");
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