using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingRange.ViewModel
{
  public class Subsession
  {
    public string SubsessionHeader { get; set; }
    public IEnumerable<string> Shots { get; set; } 
  }
  public class SessionTreeViewItem
  {
    public string SessionHeader { get; set; }
    public IEnumerable<Subsession> Subsessions { get; set; }
  }
}
