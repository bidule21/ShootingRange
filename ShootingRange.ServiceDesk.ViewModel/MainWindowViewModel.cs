using Gui.ViewModel;
using ShootingRange.ServiceDesk.ViewModel.MessageTypes;

namespace ShootingRange.ServiceDesk.ViewModel
{
    public class MainWindowViewModel : Gui.ViewModel.ViewModel
    {
        public MainWindowViewModel()
        {
            Title = "Shooting Range";

            ShowPersonsCommand = new ViewModelCommand(x => MessengerInstance.Send(new ShowPersonsPageMessage()));
            ShowPersonsCommand.RaiseCanExecuteChanged();

            ShowGroupingsCommand = new ViewModelCommand(x => MessengerInstance.Send(new ShowGroupsPageMessage()));
            ShowGroupingsCommand.RaiseCanExecuteChanged();

            ShowCreatePersonDialogCommand = new ViewModelCommand(x => MessengerInstance.Send(new CreatePersonDialogMessage()));
            ShowCreatePersonDialogCommand.RaiseCanExecuteChanged();

            ShowCreateGroupingDialogCommand = new ViewModelCommand(x => MessengerInstance.Send(new CreateGroupingDialogMessage((IWindow)x)));
            ShowCreateGroupingDialogCommand.RaiseCanExecuteChanged();
        }

        #region Commands

        public ViewModelCommand ShowPersonsCommand { get; private set; }
        public ViewModelCommand ShowGroupingsCommand { get; private set; }
        public ViewModelCommand ShowCreatePersonDialogCommand { get; private set; }
        public ViewModelCommand ShowCreateGroupingDialogCommand { get; private set; }


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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
                }
            }
        }

        #endregion
    }
}