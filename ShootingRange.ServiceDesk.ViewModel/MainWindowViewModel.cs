using System.Linq.Expressions;
using Gui.ViewModel;

namespace ShootingRange.ServiceDesk.ViewModel
{
  public class MainWindowViewModel : NotifyPropertyChanged
  {
    public MainWindowViewModel()
    {
      Title = "Shooting Range";

      ShowPersonsCommand = new ViewModelCommand(x => ShowPage<PersonsPageViewModel>());
      ShowPersonsCommand.RaiseCanExecuteChanged();

      ShowShootersCommand = new ViewModelCommand(x=> ShowPage<ShootersPageViewModel>());
      ShowShootersCommand.RaiseCanExecuteChanged();

      ShowGroupsCommand = new ViewModelCommand(x => ShowPage<GroupsPageViewModel>());
      ShowGroupsCommand.RaiseCanExecuteChanged();

      ShowRankingsCommand = new ViewModelCommand(x => ShowPage<RankingsPageViewModel>());
      ShowRankingsCommand.RaiseCanExecuteChanged();

      ShowCreatePersonCommand = new ViewModelCommand(x => ShowCreatePerson((IWindow)x));
      ShowCreatePersonCommand.RaiseCanExecuteChanged();

      ShowCreateShooterCommand = new ViewModelCommand(x => ShowCreateShooter((IWindow)x));
      ShowCreateShooterCommand.RaiseCanExecuteChanged();

      ShowCreateGroupCommand = new ViewModelCommand(x => ShowCreateGroup((IWindow)x));
      ShowCreateGroupCommand.RaiseCanExecuteChanged();

      ShowConfigurationCommand = new ViewModelCommand(x => ShowConfiguration((IWindow)x));

      ShowPersonsCommand.Execute(null);
    }

    private void ShowConfiguration(IWindow parent)
    {
      CreatePersonViewModel vm = new CreatePersonViewModel
      {
        Title = "Konfiguration anpassen"
      };
      bool? dialogResult = ShowDialog(parent, vm, vm.Title);

      if (dialogResult.HasValue && dialogResult == true)
      {
        // TODO call model to insert person in to database.
      }
    }

    private void ShowCreatePerson(IWindow parent)
    {
      CreatePersonViewModel vm = new CreatePersonViewModel
      {
        Title = "Person erfassen"
      };
      bool? dialogResult = ShowDialog(parent, vm, vm.Title);

      if (dialogResult.HasValue && dialogResult == true)
      {
        // TODO call model to insert person in to database.
      }
    }

    private void ShowCreateShooter(IWindow parent)
    {
      CreatePersonViewModel vm = new CreatePersonViewModel
      {
        Title = "Schütze erfassen"
      };
      bool? dialogResult = ShowDialog(parent, vm, vm.Title);

      if (dialogResult.HasValue && dialogResult == true)
      {
        // TODO call model to insert person in to database.
      }
    }

    private void ShowCreateGroup(IWindow parent)
    {
      CreatePersonViewModel vm = new CreatePersonViewModel
      {
        Title = "Gruppe erfassen"
      };
      bool? dialogResult = ShowDialog(parent, vm, vm.Title);

      if (dialogResult.HasValue && dialogResult == true)
      {
        // TODO call model to insert person in to database.
      }
    }

    private bool? ShowDialog<T>(IWindow parent, T ucViewModel, string title)
    {
      UcDialogViewModel dialogViewModel = new UcDialogViewModel();
      dialogViewModel.LoadUserControl(ucViewModel, title);

      IWindow window = ViewServiceLocator.ViewService.ExecuteFunction<UcDialogViewModel, IWindow>(parent, dialogViewModel);
      return window.ShowDialog();
    }

    private void ShowPage<T>() where T : new()
    {
      T vm = new T();
      IPage p = ViewServiceLocator.ViewService.ExecuteFunction<T, IPage>(null, vm);
      CurrentPage = p;
    }

    #region Commands

    public ViewModelCommand ShowPersonsCommand { get; private set; }
    public ViewModelCommand ShowShootersCommand { get; private set; }
    public ViewModelCommand ShowGroupsCommand { get; private set; }
    public ViewModelCommand ShowRankingsCommand { get; private set; }
    public ViewModelCommand ShowCreatePersonCommand { get; private set; }
    public ViewModelCommand ShowCreateShooterCommand { get; private set; }
    public ViewModelCommand ShowCreateGroupCommand { get; private set; }
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