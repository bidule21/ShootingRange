using System;
using ShootingRange.UiBusinessObjects;

namespace ShootingRange.Common
{
  public class UIEvents
  {
    public UIEvents()
    {
      PersonSelected = delegate { };
      RequireSelectedPerson = delegate { };
      PersonDataStoreChanged = delegate { };
      ShooterDataStoreChanged = delegate { };
      FetchSelectedShooter = () => null;
    }
    public UIEventsDelegate<UiPerson> PersonSelected { get; set; }

    public UIEventsDelegate RequireSelectedPerson { get; set; }

    public UIEventsDelegate PersonDataStoreChanged { get; set; }

    public UIEventsDelegate ShooterDataStoreChanged { get; set; }
    public UIEventsDelegate<UiShooter> ShooterSelected { get; set; }

    public UIEventsDelegate RequireSelectedShooter { get; set; }

    public UiEventsDelegate<UiShooter> FetchSelectedShooter { get; set; }

    public UIEventsDelegate<int> SelectPersonById { get; set; }

    public UiEventsDelegate<UiParticipation> FetchSelectedParticipation { get; set; }
  }


}
