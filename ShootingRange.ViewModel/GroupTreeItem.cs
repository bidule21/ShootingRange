using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingRange.ViewModel
{
  public class GroupTreeItem
  {
    public string GroupDescription { get; set; }
    public IEnumerable<string> GroupNames { get; set; }
  }
}
