using System.ComponentModel;
using System.Runtime.CompilerServices;
using ShootingRange.BusinessObjects.Properties;

namespace ShootingRange.ServiceDesk.ViewModel
{
  public class ParticipationViewModel : INotifyPropertyChanged
  {
    public ParticipationViewModel(int participationId)
    {
      ParticipationId = participationId;
    }

    public ParticipationViewModel()
    {
      
    }

    public int ParticipationId { get; private set; }

    private string _programName;
    public string ProgramName
    {
      get { return _programName; }
      set
      {
        if (value != _programName)
        {
          _programName = value;
          OnPropertyChanged("ProgramName");
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