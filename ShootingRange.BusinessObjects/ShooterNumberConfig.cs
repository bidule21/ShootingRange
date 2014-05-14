using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingRange.BusinessObjects
{
  public class ShooterNumberConfig
  {
    public int ShooterNumberConfigId { get; set; }
    public int LastGivenShooterNumber { get; set; }
    public int ShooterNumberIncrement { get; set; }
  }
}
