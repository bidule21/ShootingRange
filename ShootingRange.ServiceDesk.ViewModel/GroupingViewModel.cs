using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using ShootingRange.BusinessObjects.Properties;

namespace ShootingRange.ServiceDesk.ViewModel
{
    public class GroupingViewModel : INotifyPropertyChanged
    {
        
        private int _shooterCollectionId;

        public int ShooterCollectionId
        {
            get { return _shooterCollectionId; }
            set
            {
                if (value != _shooterCollectionId)
                {
                    _shooterCollectionId = value;
                    OnPropertyChanged();
                }
            }
        }

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
                }
            }
        }


        private string _participationName;

        public string ParticipationName
        {
            get { return _participationName; }
            set
            {
                if (value != _participationName)
                {
                    _participationName = value;
                    OnPropertyChanged();
                }
            }
        }


        public string ShooterNames
        {
            get
            {
                return (Shooters == null || Shooters.Count == 0) ? "keine Schützen" : string.Join(", ",
                    Shooters.Select(x => string.Format("{0} {1}", x.FirstName, x.LastName)).ToArray());
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