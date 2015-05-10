using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Gui.ViewModel;
using ShootingRange.BusinessObjects;

namespace ShootingRange.ServiceDesk.ViewModel
{
  public class CreateGroupingViewModel : INotifyPropertyChanged
  {
    public CreateGroupingViewModel()
    {
      ShooterCollection = new ShooterCollection();

      ShowCreateGroupingCommand = new ViewModelCommand(x => CreateShooter((IWindow)x));
    }

    private void CreateShooter(IWindow parent)
    {
      throw new System.NotImplementedException();
    }

    public ViewModelCommand ShowCreateGroupingCommand { get; private set; }

    private ShooterCollection _shooterCollection;
    public ShooterCollection ShooterCollection
    {
      get { return _shooterCollection; }
      set
      {
        if (value != _shooterCollection)
        {
          _shooterCollection = value;
          OnPropertyChanged("ShooterCollection");
        }
      }
    }


    private Participation _selectedParticipation;
    public Participation SelectedParticipation
    {
      get { return _selectedParticipation; }
      set
      {
        if (value != _selectedParticipation)
        {
          _selectedParticipation = value;
          OnPropertyChanged("SelectedParticipation");
        }
      }
    }

    private ObservableCollection<Participation> _participations;
    public ObservableCollection<Participation> Participations
    {
      get { return _participations; }
      set
      {
        if (value != _participations)
        {
          _participations = value;
          OnPropertyChanged("Participations");
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