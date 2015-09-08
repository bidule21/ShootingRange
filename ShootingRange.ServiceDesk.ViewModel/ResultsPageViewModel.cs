using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Schema;
using ShootingRange.BusinessObjects;
using ShootingRange.BusinessObjects.Properties;
using ShootingRange.ConfigurationProvider;
using ShootingRange.Repository.RepositoryInterfaces;

namespace ShootingRange.ServiceDesk.ViewModel
{
  public class ResultsPageViewModel : INotifyPropertyChanged, ILoadable
  {
    private List<PersonShooterViewModel> _persons;

    public void Load()
    {
      LoadPersons();
    }

    public void LoadPersons()
    {
      IPersonDataStore personDataStore = null;
      IShooterDataStore shooterDataStore = null;

      IEnumerable<PersonShooterViewModel> shooters = from shooter in shooterDataStore.GetAll()
        join person in personDataStore.GetAll() on shooter.PersonId equals person.PersonId
        orderby person.FirstName
        orderby person.LastName
        select new PersonShooterViewModel
        {
          FirstName = person.FirstName,
          LastName = person.LastName,
          DateOfBirth = person.DateOfBirth,
          PersonId = person.PersonId,
          ShooterId = shooter.ShooterId,
          ShooterNumber = shooter.ShooterNumber
        };

      _persons = shooters.ToList();
      UpdateFilteredPersons();
    }


    private ObservableCollection<SessionViewModel> _sessions;
    public ObservableCollection<SessionViewModel> Sessions
    {
      get { return _sessions; }
      set
      {
        if (value != _sessions)
        {
          _sessions = value;
          OnPropertyChanged("Sessions");
        }
      }
    }

    private SessionViewModel _selectedSession;
    public SessionViewModel SelectedSession
    {
      get { return _selectedSession; }
      set
      {
        if (value != _selectedSession)
        {
          _selectedSession = value;
          OnPropertyChanged("SelectedSession");
        }
      }
    }

    private PersonShooterViewModel _selectedPerson;

    public PersonShooterViewModel SelectedPerson
    {
      get { return _selectedPerson; }
      set
      {
        if (value != _selectedPerson)
        {
          _selectedPerson = value;
          OnPropertyChanged("SelectedPerson");

          SelectedPersonChanged();
        }
      }
    }

    private void SelectedPersonChanged()
    {
      //if (SelectedPerson != null)
      //{
      //  ISessionDataStore sessionDataStore = ConfigurationSource.Configuration.GetSessionDataStore();
      //  ISessionSubtotalDataStore subsessionDataStore = ConfigurationSource.Configuration.GetSessionSubtotalDataStore();
      //  IShotDataStore shotDataStore = ConfigurationSource.Configuration.GetShotDataStore();
      //  IProgramItemDataStore programItemDataStore = ConfigurationSource.Configuration.GetProgramItemDataStore();

      //  Dictionary<int, List<int>> sessionToSubsession = (from session in
      //    sessionDataStore.FindByShooterId(SelectedPerson.ShooterId)
      //    join subsession in subsessionDataStore.GetAll() on session.SessionId equals subsession.SessionId
      //    orderby session.ProgramItemId
      //    group subsession by session.SessionId).ToDictionary(_ => _.Key,
      //      _ => _.Select(fo => fo.SessionSubtotalId).ToList());

      //  Sessions = new ObservableCollection<SessionViewModel>();
      //  foreach (KeyValuePair<int, List<int>> keyValuePair in sessionToSubsession)
      //  {
      //    Session session = sessionDataStore.FindById(keyValuePair.Key);

      //    if (session.ProgramItemId.HasValue)
      //    {
      //      ProgramItem programItem = programItemDataStore.FindById((int) session.ProgramItemId);

      //      SessionViewModel svm = new SessionViewModel
      //      {
      //        LaneNumber = session.LaneNumber,
      //        ProgramName = programItem.ProgramName,
      //      };

      //      List<Shot> shots = new List<Shot>();
      //      foreach (int subsessionId in keyValuePair.Value)
      //      {
      //        shots.AddRange(shotDataStore.FindBySubSessionId(subsessionId).OrderBy(shot => shot.Ordinal));
      //      }

      //      svm.Shots = new ObservableCollection<Shot>(shots);
      //      svm.Total = shots.Sum(s => s.PrimaryScore);
      //      Sessions.Add(svm);
      //    }
      //  }
      //}
    }

    private ObservableCollection<PersonShooterViewModel> _filteredPersons;

    public ObservableCollection<PersonShooterViewModel> FilteredPersons
    {
      get { return _filteredPersons; }
      set
      {
        if (value != _filteredPersons)
        {
          _filteredPersons = value;
          OnPropertyChanged("FilteredPersons");
        }
      }
    }

    private string _personFilterText;

    public string PersonFilterText
    {
      get { return _personFilterText; }
      set
      {
        if (value != _personFilterText)
        {
          _personFilterText = value;
          OnPropertyChanged("PersonFilterText");
          UpdateFilteredPersons();
        }
      }
    }

    private IEnumerable<PersonShooterViewModel> FilterPersons(IEnumerable<PersonShooterViewModel> persons, string filterText)
    {
      if (filterText == null) filterText = string.Empty;
      string[] split = filterText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

      foreach (PersonShooterViewModel person in persons)
      {
        if (string.IsNullOrWhiteSpace(filterText))
        {
          yield return person;
        }
        else
        {
          if (
            split.All(
              x =>
                person.FirstName.IndexOf(x, StringComparison.InvariantCultureIgnoreCase) != -1 ||
                person.LastName.IndexOf(x, StringComparison.InvariantCultureIgnoreCase) != -1 ||
                person.ShooterNumber.ToString().IndexOf(x, StringComparison.CurrentCultureIgnoreCase) != -1))
          {
            yield return person;
          }
        }
      }
    }

    private void UpdateFilteredPersons()
    {
      FilteredPersons = new ObservableCollection<PersonShooterViewModel>(FilterPersons(_persons, PersonFilterText));
      SelectedPersonChanged();
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