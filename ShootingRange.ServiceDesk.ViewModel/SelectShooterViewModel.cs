using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Gui.ViewModel;
using Microsoft.Practices.ServiceLocation;
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

            var personToParticipationTypes = from s in shooterDataStore.GetAll()
                join p in personDataStore.GetAll() on s.PersonId equals p.PersonId
                join cs in collectionShooterDataStore.GetAll() on s.ShooterId equals cs.ShooterId into gj
                select new
                {
                    Person = p,
                    Shooter = s,
                    ParticipationTypes = from cs in gj
                        join sc in shooterCollectionDataStore.GetAll() on cs.ShooterCollectionId equals
                            sc.ShooterCollectionId
                        select sc.ProgramNumber
                };

            IEnumerable<PersonShooterViewModel> shooters = (from grouped in personToParticipationTypes
                                                            where grouped.ParticipationTypes.All(_ => _ != programNumber)
                                                            orderby grouped.Shooter.ShooterNumber
                                                            orderby grouped.Person.FirstName
                                                            orderby grouped.Person.LastName
                                                            select
                                                              new PersonShooterViewModel
                                                              {
                                                                  PersonId = grouped.Person.PersonId,
                                                                  FirstName = grouped.Person.FirstName,
                                                                  LastName = grouped.Person.LastName,
                                                                  DateOfBirth = grouped.Person.DateOfBirth,
                                                                  ShooterId = grouped.Shooter.ShooterId,
                                                                  ShooterNumber = grouped.Shooter.ShooterNumber
                                                              });

            Shooters = new ObservableCollection<PersonShooterViewModel>(shooters);
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