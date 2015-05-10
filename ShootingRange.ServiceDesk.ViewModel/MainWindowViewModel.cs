using System;
using Gui.ViewModel;
using ShootingRange.ConfigurationProvider;
using ShootingRange.Repository.RepositoryInterfaces;
using ShootingRange.Service.Interface;

namespace ShootingRange.ServiceDesk.ViewModel
{
  public class MainWindowViewModel : NotifyPropertyChanged
  {
    private GroupsPageViewModel _groupingsPageViewModel;
    private PersonsPageViewModel _personsPageViewModel;
    private IPersonDataStore _personDataStore;

    public MainWindowViewModel()
    {
      Title = "Shooting Range";

      _personDataStore = ConfigurationSource.Configuration.GetPersonDataStore();
      _personsPageViewModel = new PersonsPageViewModel();
      _groupingsPageViewModel = new GroupsPageViewModel();

      ShowPersonsCommand = new ViewModelCommand(x => ShowPage((IWindow)x, _personsPageViewModel));
      ShowPersonsCommand.RaiseCanExecuteChanged();

      ShowGroupingsCommand = new ViewModelCommand(x => ShowPage((IWindow)x, _groupingsPageViewModel));
      ShowGroupingsCommand.RaiseCanExecuteChanged();

      ShowRankingsCommand = new ViewModelCommand(x => ShowPage((IWindow)x, new RankingsPageViewModel()));
      ShowRankingsCommand.RaiseCanExecuteChanged();

      ShowCreatePersonCommand = new ViewModelCommand(x => ShowCreatePerson((IWindow)x));
      ShowCreatePersonCommand.RaiseCanExecuteChanged();

      ShowCreateGroupingCommand = new ViewModelCommand(x => ShowCreateGrouping((IWindow)x));
      ShowCreateGroupingCommand.RaiseCanExecuteChanged();

      ShowConfigurationCommand = new ViewModelCommand(x => ShowConfiguration((IWindow)x));

      ShowPersonsCommand.Execute(null);
    }

    private void ShowCreatePerson(IWindow parent)
    {
      _personsPageViewModel.ShowCreatePerson(parent);
      RadrawCurrentPage();
    }

    private void ShowCreateGrouping(IWindow parent)
    {
      GroupingHelper.ShowCreateGrouping(parent);
      RadrawCurrentPage();
    }

    private void RadrawCurrentPage()
    {
      IPage currentPageBkp = CurrentPage;
      CurrentPage = null;
      CurrentPage = currentPageBkp;
    }

    private void ShowConfiguration(IWindow parent)
    {
      throw new NotImplementedException();
    }

    private void ShowPage<T>(IWindow parent, T vm)
    {
      IPage p = ViewServiceLocator.ViewService.ExecuteFunction<T, IPage>(parent, vm);
      CurrentPage = p;
    }

    #region Commands

    public ViewModelCommand ShowPersonsCommand { get; private set; }
    public ViewModelCommand ShowGroupingsCommand { get; private set; }
    public ViewModelCommand ShowRankingsCommand { get; private set; }
    public ViewModelCommand ShowCreatePersonCommand { get; private set; }
    public ViewModelCommand ShowCreateGroupingCommand { get; private set; }
    public ViewModelCommand ShowConfigurationCommand { get; private set; }

    #endregion

    #region INotifyPropertyChanged Members

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

    #endregion
  }
}