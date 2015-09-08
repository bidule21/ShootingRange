using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Gui.ViewModel;
using Microsoft.Practices.ServiceLocation;
using ShootingRange.ConfigurationProvider;
using ShootingRange.Repository.RepositoryInterfaces;
using ShootingRange.ServiceDesk.ViewModel.Annotations;

namespace ShootingRange.ServiceDesk.ViewModel
{
    public class SelectParticipationViewModel : INotifyPropertyChanged 
    {
        public SelectParticipationViewModel()
        {
            OkCommand = new ViewModelCommand(x => { });
            OkCommand.AddGuard(x => SelectedParticipationDescription != null);
        }

        public ViewModelCommand OkCommand { get; private set; }

        public void Initialize(int shooterId)
        {
            ServiceDeskConfiguration sdk = ServiceLocator.Current.GetInstance<ServiceDeskConfiguration>();
            IShooterParticipationDataStore shooterParticipationDataStore = ServiceLocator.Current.GetInstance<IShooterParticipationDataStore>();

            var programNumberToShooterParticipations = from sp in shooterParticipationDataStore.GetAll()
                join p in sdk.ParticipationDescriptions.GetAll() on sp.ProgramNumber.ToString() equals
                    p.ProgramNumber
                where sp.ShooterId == shooterId
                group sp by p.ProgramNumber into gj select new
                {
                    ProgramNumber = gj.Key,
                    ShooterParticipations = gj.Select(x => x)
                };

            var selectableParticipations = from p in sdk.ParticipationDescriptions.GetAll()
                where
                    (!programNumberToShooterParticipations.Any(x => x.ProgramNumber == p.ProgramNumber) ||
                     !programNumberToShooterParticipations.Single(x => x.ProgramNumber == p.ProgramNumber)
                         .ShooterParticipations.Any()) select p;

            ParticipationDescriptions = new ObservableCollection<ParticipationDescription>(selectableParticipations);
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

        private ObservableCollection<ParticipationDescription> _participationDescriptions;

        public ObservableCollection<ParticipationDescription> ParticipationDescriptions
        {
            get { return _participationDescriptions; }
            set
            {
                if (value != _participationDescriptions)
                {
                    _participationDescriptions = value;
                    OnPropertyChanged();
                }
            }
        }

        private ParticipationDescription _selectedParticipationDescription;

        public ParticipationDescription SelectedParticipationDescription
        {
            get { return _selectedParticipationDescription; }
            set
            {
                if (value != _selectedParticipationDescription)
                {
                    _selectedParticipationDescription = value;
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