using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingRange.ViewModel
{
  public class ParticipationTreeItem
  {
    public string ParticipationDescription { get; set; }
    public IEnumerable<string> ParticipationNames { get; set; }
  }
}
