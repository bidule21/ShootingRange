using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ShootingRange.BusinessObjects;
using ShootingRange.BusinessObjects.Properties;

namespace ShootingRange.ServiceDesk.ViewModel
{
    public class SessionViewModel : INotifyPropertyChanged
    {
        public int SessionId { get; set; }


        private Shot _selectedShot;

        public Shot SelectedShot
        {
            get { return _selectedShot; }
            set
            {
                if (value != _selectedShot)
                {
                    _selectedShot = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<Shot> _shots;

        public ObservableCollection<Shot> Shots
        {
            get { return _shots; }
            set
            {
                if (value != _shots)
                {
                    _shots = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _laneNumber;

        public int LaneNumber
        {
            get { return _laneNumber; }
            set
            {
                if (value != _laneNumber)
                {
                    _laneNumber = value;
                    OnPropertyChanged();
                }
            }
        }


        private decimal _total;

        public decimal Total
        {
            get { return _total; }
            set
            {
                if (value != _total)
                {
                    _total = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _programName;

        public string ProgramName
        {
            get { return _programName; }
            set
            {
                if (value != _programName)
                {
                    _programName = value;
                    OnPropertyChanged();
                }
            }
        }


        private bool _shooterIsParticipating;
        public bool ShooterIsParticipating
        {
            get { return _shooterIsParticipating; }
            set
            {
                if (value != _shooterIsParticipating)
                {
                    _shooterIsParticipating = value;
                    OnPropertyChanged();
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