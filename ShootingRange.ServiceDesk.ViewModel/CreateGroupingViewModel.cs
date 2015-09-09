using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Gui.ViewModel;
using ShootingRange.ConfigurationProvider;

namespace ShootingRange.ServiceDesk.ViewModel
{
    public class CreateGroupingViewModel : INotifyPropertyChanged
    {
        public CreateGroupingViewModel(ServiceDeskConfiguration serviceDeskConfiguration)
        {
            Participations =
                new ObservableCollection<ParticipationDescription>(
                    serviceDeskConfiguration.ParticipationDescriptions.GetAll()
                        .Where(x => x.AllowShooterCollectionParticipation));

            OkCommand = new ViewModelCommand(x =>
            {
                ((IWindow)x).Close();
            });
            OkCommand.AddGuard(x => !string.IsNullOrWhiteSpace(GroupingName) && SelectedParticipation != null);
        }

        public ViewModelCommand OkCommand { get; private set; }

        private string _groupingName;

        public string GroupingName
        {
            get { return _groupingName; }
            set
            {
                if (value != _groupingName)
                {
                    _groupingName = value;
                    OnPropertyChanged();
                    OkCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private ParticipationDescription _selectedParticipation;

        public ParticipationDescription SelectedParticipation
        {
            get { return _selectedParticipation; }
            set
            {
                if (value != _selectedParticipation)
                {
                    _selectedParticipation = value;
                    OnPropertyChanged();
                    OkCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private ObservableCollection<ParticipationDescription> _participations;

        public ObservableCollection<ParticipationDescription> Participations
        {
            get { return _participations; }
            set
            {
                if (value != _participations)
                {
                    _participations = value;
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}