using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShootingRange.BusinessObjects;
using ShootingRange.Entities;
using ShootingRange.Repository.RepositoryInterfaces;

namespace ShootingRange.Repository.Repositories
{
  public class ShooterParticipationView : IShooterParticipationView
  {
    private DbSet<t_participation> _participations;
    private DbSet<t_shooterparticipation> _shooterParticipations;

    public ShooterParticipationView(DbContext context)
    {
      _participations = context.Set<t_participation>();
      _shooterParticipations = context.Set<t_shooterparticipation>();
    }

    public IEnumerable<ShooterParticipationDetails> GetAll()
    {
      return from sp in _shooterParticipations
        join p in _participations on sp.ParticipationId equals p.ParticipationId
        select new ShooterParticipationDetails
        {
          ShooterParticipationId = sp.ShooterParticipationId,
          ParticipationName = p.Description
        };
    }

    public IEnumerable<ShooterParticipationDetails> FindByShooterId(int shooterId)
    {
      return from sp in _shooterParticipations
        join p in _participations on sp.ParticipationId equals p.ParticipationId
        where sp.ShooterId == shooterId
        select new ShooterParticipationDetails
        {
          ShooterParticipationId = sp.ShooterParticipationId,
          ParticipationName = p.Description
        };
    }
  }
}
