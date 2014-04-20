using System.Collections.Generic;

namespace ShootingRange.BusinessObjects
{
  public class GroupDetails
  {
    public string GroupDescription { get; set; }

    public IEnumerable<string> GroupNames { get; set; }
  }
}
