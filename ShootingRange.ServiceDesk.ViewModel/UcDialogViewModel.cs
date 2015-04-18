using System.ComponentModel;
using System.Runtime.CompilerServices;
using Gui.ViewModel;

namespace ShootingRange.ServiceDesk.ViewModel
{
  public class UcDialogViewModel : INotifyPropertyChanged
  {
    public void LoadUserControl<T>(T vm, string title)
    {
      IPage p = ViewServiceLocator.ViewService.ExecuteFunction<T, IPage>(null, vm);
      CurrentPage = p;
      Title = title;
    }

    private IPage _currentPage;
    public IPage CurrentPage
    {
      get { return _currentPage; }
      set
      {
        if (value != _currentPage)
        {
          _currentPage = value;
          OnPropertyChanged("CurrentPage");
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

    #region NotifyPropertyChanged Members

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      var handler = PropertyChanged;
      if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion

  }
}