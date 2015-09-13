using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using ShootingRange.BusinessObjects.Properties;

namespace ShootingRange.ServiceDesk.ViewModel
{
    public class ShooterCollectionViewModel : INotifyPropertyChanged
    {

        private ObservableCollection<UiCollectionShooter> _shooters;

        public ObservableCollection<UiCollectionShooter> Shooters
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

        private string _collectionName;

        public string CollectionName
        {
            get { return _collectionName; }
            set
            {
                if (value != _collectionName)
                {
                    _collectionName = value;
                    OnPropertyChanged();
                }
            }
        }

        public int CollectionShooterCount
        {
            get { return Shooters == null ? 0 : Shooters.Count; }
        }

        public int CollectionScore
        {
            get { return Shooters == null ? 0 : Shooters.Sum(x => x.CollectionShooterScore); }
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