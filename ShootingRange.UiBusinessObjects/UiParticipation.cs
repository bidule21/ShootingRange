using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingRange.UiBusinessObjects
{
  public class UiParticipation
  {
    public int ParticipationId { get; set; }

    public string ParticipationName { get; set; }

    public bool AllowCollectionParticipation { get; set; }
    public bool AllowSingleParticipation { get; set; }

    public List<UiShooterCollection> ShooterCollections { get; set; }
  }
}
