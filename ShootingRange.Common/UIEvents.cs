using ShootingRange.BusinessObjects;

namespace ShootingRange.Common
{
  public class UIEvents
  {
    public UIEvents()
    {
      PersonSelected = delegate { };
      RequireSelectedPerson = delegate { };
    }
    public UIEventsDelegate<Person> PersonSelected { get; set; }

    public UIEventsDelegate RequireSelectedPerson { get; set; }
  }


}
