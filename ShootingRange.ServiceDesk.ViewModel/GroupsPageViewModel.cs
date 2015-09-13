using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Practices.ServiceLocation;
using ShootingRange.BusinessObjects.Properties;
using ShootingRange.ConfigurationProvider;

namespace ShootingRange.ServiceDesk.ViewModel
{
    public class GroupsPageViewModel : INotifyPropertyChanged
    {
        public GroupsPageViewModel()
        {
            Load();
        }

        public void Load()
        {
            ServiceDeskConfiguration sdk = ServiceLocator.Current.GetInstance<ServiceDeskConfiguration>();
            Groupings = new ObservableCollection<GroupingPageGroupingViewModel>();
            foreach (ParticipationDescription participationDescription in sdk.ParticipationDescriptions.GetAll().Where(x => x.AllowShooterCollectionParticipation))
            {
                GroupingPageGroupingViewModel groupingViewModel = new GroupingPageGroupingViewModel();
                groupingViewModel.ProgramNumber = int.Parse(participationDescription.ProgramNumber);
                groupingViewModel.GroupingType = participationDescription.ProgramName;

                Groupings.Add(groupingViewModel);
            }
        }


        private GroupingPageGroupingViewModel _selectedGrouping;

        public GroupingPageGroupingViewModel SelectedGrouping
        {
            get { return _selectedGrouping; }
            set
            {
                if (value != _selectedGrouping)
                {
                    _selectedGrouping = value;
                    OnPropertyChanged();

                    if (value != null)
                        value.Load();
                }
            }
        }


        private ObservableCollection<GroupingPageGroupingViewModel> _groupings;


        public ObservableCollection<GroupingPageGroupingViewModel> Groupings
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

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}