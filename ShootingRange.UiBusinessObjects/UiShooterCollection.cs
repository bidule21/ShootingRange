using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingRange.UiBusinessObjects
{
  public class UiShooterCollection
  {
    public string CollectionName { get; set; }
    public int ShooterCollectionId { get; set; }

    public IEnumerable<UiShooter> Shooters { get; set; }
  }
}
