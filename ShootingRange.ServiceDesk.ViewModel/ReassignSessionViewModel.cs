using System.Collections.ObjectModel;
using System.Linq;
using Gui.ViewModel;
using Microsoft.Practices.ServiceLocation;
using ShootingRange.Repository.RepositoryInterfaces;

namespace ShootingRange.ServiceDesk.ViewModel
{
    public class ReassignSessionViewModel : Gui.ViewModel.ViewModel
    {
        public ReassignSessionViewModel()
        {
            OkCommand = new ViewModelCommand(x =>
            {
            });
            OkCommand.AddGuard(x => SelectedShooter != null);
            OkCommand.RaiseCanExecuteChanged();
        }

        public ViewModelCommand OkCommand { get; private set; }

        public void Initialize(int sessionId)
        {
            IPersonDataStore personDataStore = ServiceLocator.Current.GetInstance<IPersonDataStore>();
            IShooterDataStore shooterDataStore = ServiceLocator.Current.GetInstance<IShooterDataStore>();

            Shooters = new ObservableCollection<PersonShooterViewModel>(
                from p in personDataStore.GetAll()
                join s in shooterDataStore.GetAll() on p.PersonId equals s.PersonId orderby p.FirstName orderby p.LastName
                select new PersonShooterViewModel(p, s)
                );
        }

        private ObservableCollection<PersonShooterViewModel> _shooters;
        public ObservableCollection<PersonShooterViewModel> Shooters
        {
            get { return _shooters; }
            set
            {
                if (value != _shooters)
                {
                    _shooters = value;
                    OnPropertyChanged();
                }
            }
        }

        private PersonShooterViewModel _selectedShooter;
        public PersonShooterViewModel SelectedShooter
        {
            get { return _selectedShooter; }
            set
            {
                if (value != _selectedShooter)
                {
                    _selectedShooter = value;
                    OnPropertyChanged();

                    OkCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private string _titel;
        public string Title
        {
            get { return _titel; }
            set
            {
                if (value != _titel)
                {
                    _titel = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}