using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Gui.ViewModel;
using Microsoft.Practices.ServiceLocation;
using ShootingRange.BusinessObjects;
using ShootingRange.BusinessObjects.Properties;
using ShootingRange.Repository.RepositoryInterfaces;

namespace ShootingRange.ServiceDesk.ViewModel
{
    public class SelectShooterViewModel : INotifyPropertyChanged
    {
        public SelectShooterViewModel()
        {
            OkCommand = new ViewModelCommand(x => { });
            OkCommand.AddGuard(x => SelectedShooter != null);
        }

        public void Initialize(int programNumber)
        {
            IPersonDataStore personDataStore = ServiceLocator.Current.GetInstance<IPersonDataStore>();
            IShooterDataStore shooterDataStore = ServiceLocator.Current.GetInstance<IShooterDataStore>();
            ICollectionShooterDataStore collectionShooterDataStore = ServiceLocator.Current.GetInstance<ICollectionShooterDataStore>();
            IShooterCollectionDataStore shooterCollectionDataStore = ServiceLocator.Current.GetInstance<IShooterCollectionDataStore>();
            IShooterParticipationDataStore shooterParticipationDataStore =
                ServiceLocator.Current.GetInstance<IShooterParticipationDataStore>();

            // ShooterIds enroled in ProgramNumber
            IEnumerable<int> shooters = from sp in shooterParticipationDataStore.FindByProgramNumber(programNumber)
                select sp.ShooterId;

            // ShooterIds with a ShooterCollection participating in ProgramNumber
            IEnumerable<int> shootersWithCollectionInProgramNumber = from cs in collectionShooterDataStore.GetAll()
                join sc in shooterCollectionDataStore.GetAll() on cs.ShooterCollectionId equals sc.ShooterCollectionId
                where sc.ProgramNumber == programNumber
                select cs.ShooterId;

            IEnumerable<int> shootersFinal = shooters.Except(shootersWithCollectionInProgramNumber);

            IEnumerable<PersonShooterViewModel> result = from shooterId in shootersFinal
                join s in shooterDataStore.GetAll() on shooterId equals s.ShooterId
                join p in personDataStore.GetAll() on s.PersonId equals p.PersonId
                select
                    new PersonShooterViewModel
                    {
                        PersonId = p.PersonId,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        DateOfBirth = p.DateOfBirth,
                        ShooterId = s.ShooterId,
                        ShooterNumber = s.ShooterNumber
                    };

            Shooters = new ObservableCollection<PersonShooterViewModel>(result);
        }


        public ViewModelCommand OkCommand { get; private set; }

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

        private PersonShooterViewModel _selectedShooter;

        public PersonShooterViewModel SelectedShooter
        {
            get { return _selectedShooter; }
            set
            {
                if (value != _selectedShooter)
                {
                    _selectedShooter = value;
                    OnPropertyChanged("SelectedShooter");

                    OkCommand.RaiseCanExecuteChanged();
                }
            }
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
                    OnPropertyChanged("Shooters");
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