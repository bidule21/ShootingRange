using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ShootingRange.BusinessObjects;
using ShootingRange.Entities;

namespace ShootingRange.Repository.Repositories
{
  public class GroupDetailsView : IGroupDetailsView
  {
    private DbSet<t_participation> _participations;
    private DbSet<t_shooterparticipation> _shooterParticipations;
    private DbSet<t_person> _people;
    private DbSet<t_shooter> _shooters;
    private DbSet<t_participationdescription> _participationDescriptions;

    public GroupDetailsView(DbContext context)
    {
      _participations = context.Set<t_participation>();
      _shooters = context.Set<t_shooter>();
      _people = context.Set<t_person>();
      _participationDescriptions = context.Set<t_participationdescription>();
      _shooterParticipations = context.Set<t_shooterparticipation>();
    }
    public IEnumerable<ParticipationDetails> GetAll()
    {
      IEnumerable<ParticipationDetails> view = from pd in _participationDescriptions
        join g in _participations on pd.ParticipationDescriptionId equals g.ParticipationDescriptionId into gds
        select new ParticipationDetails
        {
          ParticipationDescription = pd.Description,
          ParticipationNames = gds.OrderBy(_ => _.ParticipationName).Select(_ => _.ParticipationName)
        };

      return view;
    }

    public IEnumerable<ParticipationDetails> FindByShooterId(int shooterId)
    {
      IEnumerable<ParticipationDetails> view =
        from gd in _participationDescriptions
        join g in
          (from g in _participations 
           join sp in _shooterParticipations on g.ParticipationId equals sp.ParticipationId 
           where sp.ShooterId == shooterId
           select g) on gd.ParticipationDescriptionId equals
          g.ParticipationDescriptionId into gds
        select new ParticipationDetails
        {
          ParticipationDescription = gd.Description,
          ParticipationNames = gds.OrderBy(_ => _.ParticipationName).Select(_ => _.ParticipationName)
        };

      return view;
    }

    public IEnumerable<ParticipationDetails> FindByPersonId(int personId)
    {
      IEnumerable<ParticipationDetails> view =
        from gd in _participationDescriptions
        join g in
          (from g in _participations
           join s in _shooterParticipations on g.ParticipationId equals s.ParticipationId 
           join p in _shooters on s.ShooterId equals p.ShooterId
           where p.PersonId == personId
           select g) on gd.ParticipationDescriptionId equals
          g.ParticipationDescriptionId into gds
        select new ParticipationDetails
        {
          ParticipationDescription = gd.Description,
          ParticipationNames = gds.OrderBy(_ => _.ParticipationName).Select(_ => _.ParticipationName)
        };

      return view;
    }
  }
}
