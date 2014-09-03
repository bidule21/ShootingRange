using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingRange.ViewModel
{
  public class ParticipationListItem
  {
    public int ParticipationId { get; set; }
    public string ParticipationName { get; set; }

    public IEnumerable<GroupListItem> GroupListItems { get; set; }

    public bool HasGroupListItems { get { return GroupListItems.Any(); } }

    public bool IsSelected { get; set; }
  }
}
