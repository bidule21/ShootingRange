using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingRange.BusinessObjects
{
  public class ShooterParticipation
  {
    public int ShooterParticipationId { get; set; }
    public int ShooterId { get; set; }
    public int ParticipationId { get; set; }
    public string ParticipationName { get; set; }
  }
}
