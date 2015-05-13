using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Gui.ViewModel;
using ShootingRange.BusinessObjects;
using ShootingRange.BusinessObjects.Properties;
using ShootingRange.ConfigurationProvider;
using ShootingRange.Repository.Repositories;
using ShootingRange.Repository.RepositoryInterfaces;

namespace ShootingRange.ServiceDesk.ViewModel
{
  public class GroupsPageViewModel : INotifyPropertyChanged, ILoadable
  {
    public GroupsPageViewModel()
    {
      _participationDataStore = ConfigurationSource.Configuration.GetParticipationDataStore();

      Initialize();
      Load();
    }

    private void Initialize()
    {

    }

    public void Load()
    {
      List<int> groupTypes = (from p in _participationDataStore.GetAll() where p.AllowCollectionParticipation select p.ParticipationId).ToList();

      Groupings = new ObservableCollection<GroupingPageGroupingViewModel>();
      foreach (int collectionId in groupTypes)
      {
        GroupingPageGroupingViewModel groupingViewModel = new GroupingPageGroupingViewModel();
        groupingViewModel.GroupTypeId = collectionId;

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
          OnPropertyChanged("SelectedGrouping");

          if (value != null)
            value.Load();
        }
      }
    }


    private ObservableCollection<GroupingPageGroupingViewModel> _groupings;
    private IParticipationDataStore _participationDataStore;


    public ObservableCollection<GroupingPageGroupingViewModel> Groupings
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