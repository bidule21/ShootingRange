using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ShootingRange.UiBusinessObjects;
using ShootingRange.UiBusinessObjects.Annotations;

namespace ShootingRange.ViewModel
{
  public class EditGroupViewModel : INotifyPropertyChanged
  {
    public EditGroupViewModel()
    {
      if (!DesignTimeHelper.IsInDesignMode)
      {
        
      }
      else
      {
        UiShooterCollection = new UiShooterCollection
        {
          CollectionName = "fooCollection",
          Shooters = new List<UiShooter>
          {
            new UiShooter
            {
              FirstName = "alfred",
              LastName = "neumann"
            }
          }
        };
      }
    }

    private ObservableCollection<UiShooter> _uiShooters;
    public ObservableCollection<UiShooter> UiShooters
    {
      get { return _uiShooters; }
      set
      {
        if (value != _uiShooters)
        {
          _uiShooters = value;
          OnPropertyChanged("UiShooters");
        }
      }
    }


    private UiShooterCollection _uiShooterCollection;
    public UiShooterCollection UiShooterCollection
    {
      get { return _uiShooterCollection; }
      set
      {
        if (value != _uiShooterCollection)
        {
          _uiShooterCollection = value;
          OnPropertyChanged("UiShooterCollection");
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
