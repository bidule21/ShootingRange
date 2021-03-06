﻿using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using ShootingRange.ConfigurationProvider;
using ShootingRange.ServiceDesk.ViewModel.MessageTypes;

namespace ShootingRange.ServiceDesk.ViewModel
{
    public class GroupsPageViewModel : Gui.ViewModel.ViewModel
    {
        public GroupsPageViewModel()
        {
            if (ServiceLocator.IsLocationProviderSet)
            {
                MessengerInstance.Register<RefreshDataFromRepositoriesMessage>(this, x => Initialize());
                MessengerInstance.Register<SetSelectedCollectionProgramNumber>(this, x=> SelectedGrouping = Groupings.FirstOrDefault(g => g.ProgramNumber == x.ProgramNumber));
            }
        }

        private string _foo;
        public string Foo
        {
            get { return _foo; }
            set
            {
                if (value != _foo)
                {
                    _foo = value;
                    OnPropertyChanged();
                }
            }
        }

        public void Initialize()
        {
            ServiceDeskConfiguration sdk = ServiceLocator.Current.GetInstance<ServiceDeskConfiguration>();
            Groupings = new ObservableCollection<GroupingPageGroupingViewModel>();
            foreach (ParticipationDescription participationDescription in sdk.ParticipationDescriptions.GetAll().Where(x => x.AllowShooterCollectionParticipation))
            {
                GroupingPageGroupingViewModel groupingViewModel = new GroupingPageGroupingViewModel();
                groupingViewModel.Initialize();
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
    }
}