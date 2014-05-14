using System.Collections.Generic;

namespace ShootingRange.BusinessObjects
{
  public class ParticipationDetails
  {
    public string ParticipationDescription { get; set; }

    public IEnumerable<string> ParticipationNames { get; set; }
  }
}
