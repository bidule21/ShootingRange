using System.Collections.ObjectModel;
using System.Linq;
using Gui.ViewModel;
using Microsoft.Practices.ServiceLocation;
using ShootingRange.ConfigurationProvider;

namespace ShootingRange.ServiceDesk.ViewModel
{
    public class ReassignProgramNumberViewModel : Gui.ViewModel.ViewModel
    {
        public ReassignProgramNumberViewModel()
        {
            OkCommand = new ViewModelCommand(x =>
            {
            });
            OkCommand.AddGuard(x => SelectedParticipation != null);
            OkCommand.RaiseCanExecuteChanged();
        }

        public ViewModelCommand OkCommand { get; private set; }

        public void Initialize(int sessionId)
        {
            ServiceDeskConfiguration sdk = ServiceLocator.Current.GetInstance<ServiceDeskConfiguration>();

            Participations = new ObservableCollection<ParticipationViewModel>(
                from p in sdk.ParticipationDescriptions.GetAll() select new ParticipationViewModel
                {
                    ProgramNumber = int.Parse(p.ProgramNumber),
                    ProgramName = p.ProgramName
                });
        }

        private ObservableCollection<ParticipationViewModel> _participations;
        public ObservableCollection<ParticipationViewModel> Participations
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

        private ParticipationViewModel _selectedParticipation;
        public ParticipationViewModel SelectedParticipation
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