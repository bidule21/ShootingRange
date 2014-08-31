using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingRange.UiBusinessObjects
{
  public class UiSession
  {
    public int SessionId { get; set; }
    public int ShooterId { get; set; }
    public int? ProgramItemId { get; set; }

    public string ProgramDescription { get; set; }
    public int LaneNumber { get; set; }
  }
}
