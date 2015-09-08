using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Gui.ViewModel;
using Microsoft.Practices.ServiceLocation;
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
            ServiceDeskConfiguration serviceDeskConfiguration = ServiceLocator.Current.GetInstance<ServiceDeskConfiguration>();
            IShooterDataStore shooterDataStore = ServiceLocator.Current.GetInstance<IShooterDataStore>();
            IPersonDataStore personDataStore = ServiceLocator.Current.GetInstance<IPersonDataStore>();
            ICollectionShooterDataStore collectionShooterDataStore = ServiceLocator.Current.GetInstance<ICollectionShooterDataStore>();
            IShooterCollectionDataStore shooterCollectionDataStore = ServiceLocator.Current.GetInstance<IShooterCollectionDataStore>();

            var shooterCollectionToProgramNumber = from shooterCollection in shooterCollectionDataStore.GetAll()
                join collectionShooter in collectionShooterDataStore.GetAll() on shooterCollection.ShooterCollectionId
                    equals collectionShooter.ShooterCollectionId
                join shooter in shooterDataStore.GetAll() on collectionShooter.ShooterId equals shooter.ShooterId group shooter by shooterCollection.ProgramNumber into
                    gj
                select new
                {
                    ProgramNumber = gj.Key.ToString(),
                    Shooters = gj.Select(x => x)
                };

            var selectableCollections = from grouping in shooterCollectionToProgramNumber
                where grouping.Shooters.All(x => x.ShooterId != shooterId)
                join participation in serviceDeskConfiguration.ParticipationDescriptions.GetAll() on
                    grouping.ProgramNumber equals participation.ProgramNumber
                join shooterCollection in shooterCollectionDataStore.GetAll() on grouping.ProgramNumber equals
                    shooterCollection.ProgramNumber.ToString()
                select new GroupingViewModel
                {
                    ParticipationName = participation.ProgramName,
                    ShooterCollectionId = shooterCollection.ShooterCollectionId,
                    GroupingName = shooterCollection.CollectionName,
                    Shooters = new ObservableCollection<PersonShooterViewModel>(from cs in collectionShooterDataStore.GetAll()
                        join s in shooterDataStore.GetAll() on cs.ShooterId equals s.ShooterId join p in personDataStore.GetAll() on s.PersonId equals p.PersonId
                        where cs.ShooterCollectionId == shooterCollection.ShooterCollectionId
                        select new PersonShooterViewModel(p, s))
                };

            Groupings = new ObservableCollection<GroupingViewModel>(selectableCollections);
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

        private ObservableCollection<GroupingViewModel> _groupings;

        public ObservableCollection<GroupingViewModel> Groupings
        {
            get { return _groupings; }
            set
            {
                if (value != _groupings)
                {
                    _groupings = value;
                    OnPropertyChanged("Groupings");
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
                    OnPropertyChanged("SelectedGrouping");

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