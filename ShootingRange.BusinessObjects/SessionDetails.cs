using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingRange.BusinessObjects
{
  public class SubSessionDetails
  {
    public int Ordinal { get; set; }
    public IEnumerable<Shot> Shots { get; set; }    
  }

  public class SessionDetails
  {
    public IEnumerable<SubSessionDetails> SubSessions { get; set; }
    public string SessionDescription { get; set; }
    public int ShooterNumber { get; set; }
    public int ProgramNumber { get; set; }
    public int SessionId { get; set; }
  }
}
