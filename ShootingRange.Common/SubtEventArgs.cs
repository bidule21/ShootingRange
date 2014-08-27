using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingRange.Common
{
  public class SubtEventArgs : EventArgs
  {
    public int LaneId { get; set; }

    public int LaneNumber { get; set; }
  }
}
