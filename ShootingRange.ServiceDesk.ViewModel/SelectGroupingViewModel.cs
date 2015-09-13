using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Gui.ViewModel;
using Microsoft.Practices.ServiceLocation;
using ShootingRange.BusinessObjects;
using ShootingRange.BusinessObjects.Properties;
using ShootingRange.ConfigurationProvider;
using ShootingRange.Repository.RepositoryInterfaces;

namespace ShootingRange.ServiceDesk.ViewModel
{
    public class SelectGroupingViewModel : INotifyPropertyChanged
    {
        public SelectGroupingViewModel()
        {
            OkCommand = new ViewModelCommand(x => { });
            OkCommand.AddGuard(x => SelectedGrouping != null);
        }

        public ViewModelCommand OkCommand { get; private set; }

        public void Initialize(int shooterId)
        {
            ServiceDeskConfiguration serviceDeskConfiguration =
                ServiceLocator.Current.GetInstance<ServiceDeskConfiguration>();
            IShooterDataStore shooterDataStore = ServiceLocator.Current.GetInstance<IShooterDataStore>();
            IPersonDataStore personDataStore = ServiceLocator.Current.GetInstance<IPersonDataStore>();
            ICollectionShooterDataStore collectionShooterDataStore =
                ServiceLocator.Current.GetInstance<ICollectionShooterDataStore>();
            IShooterCollectionDataStore shooterCollectionDataStore =
                ServiceLocator.Current.GetInstance<IShooterCollectionDataStore>();
            IShooterParticipationDataStore shooterParticipationDataStore =
                ServiceLocator.Current.GetInstance<IShooterParticipationDataStore>();

            // Get PrgramNumbers in which the shooter is enroled.
            List<int> programNumbers = (from sp in shooterParticipationDataStore.FindByShooterId(shooterId)
                select sp.ProgramNumber).ToList();

            // Get all CollectionShooters grouped by their ShooterCollections Participation ProgramNumber
            IEnumerable<IGrouping<int, CollectionShooter>>collectionShootersGroupedByProgramNumber =
                from p in programNumbers
                join sc in shooterCollectionDataStore.GetAll() on p equals sc.ProgramNumber
                join
                    cs in collectionShooterDataStore.GetAll() on sc.ShooterCollectionId equals cs.ShooterCollectionId
                group cs by p;

            // Program Numbers with which the current shooter is not yet enroled as a CollectionShooter
            IEnumerable<int> programNumbersAlreadyEnroled = from scg in collectionShootersGroupedByProgramNumber
                where scg.Any(x => x.ShooterId == shooterId)
                select scg.Key;

            // Final list of ShooterCollections which are relevant for the current shooter
            IEnumerable<ShooterCollection> shooterCollectionsFinal = from p in programNumbers
                join sc in shooterCollectionDataStore.GetAll() on p equals sc.ProgramNumber
                where !programNumbersAlreadyEnroled.Contains(p)
                select sc;

            IEnumerable<GroupingViewModel> groupingViewModels = from sc in shooterCollectionsFinal
                join p in serviceDeskConfiguration.ParticipationDescriptions.GetAll() on sc.ProgramNumber.ToString()
                    equals
                    p.ProgramNumber
                select new GroupingViewModel
                {
                    ShooterCollectionId = sc.ShooterCollectionId,
                    GroupingName = sc.CollectionName,
                    ParticipationName = p.ProgramName,
                    Shooters =
                        new ObservableCollection<PersonShooterViewModel>(from cs in collectionShooterDataStore.GetAll()
                            join s in shooterDataStore.GetAll() on cs.ShooterId equals s.ShooterId
                            join person in personDataStore.GetAll() on s.PersonId equals person.PersonId
                            where cs.ShooterCollectionId == sc.ShooterCollectionId
                            select new PersonShooterViewModel(person, s))
                };

            Groupings = new ObservableCollection<GroupingViewModel>(groupingViewModels);
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

        private ObservableCollection<GroupingViewModel> _groupings;

        public ObservableCollection<GroupingViewModel> Groupings
        {
            get { return _groupings; }
            set
            {
                if (value != _groupings)
                {
                    _groupings = value;
                    OnPropertyChanged();
                }
            }
        }

        private GroupingViewModel _selectedGrouping;

        public GroupingViewModel SelectedGrouping
        {
            get { return _selectedGrouping; }
            set
            {
                if (value != _selectedGrouping)
                {
                    _selectedGrouping = value;
                    OnPropertyChanged();

                    OkCommand.RaiseCanExecuteChanged();
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