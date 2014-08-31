using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ShootingRange.BusinessObjects;
using ShootingRange.Entities;
using ShootingRange.Repository.RepositoryInterfaces;

namespace ShootingRange.Repository.Repositories
{
  public class GroupDetailsView : IGroupDetailsView
  {
    private DbSet<t_shooterparticipation> _shooterParticipations;
    private DbSet<t_person> _people;
    private DbSet<t_shooter> _shooters;
    private DbSet<t_participation> _participations;
    private DbSet<t_shootercollectionparticipation> _shooterCollectionParticipation;
    private DbSet<t_shootercollection> _shooterCollection;
    private DbSet<t_collectionshooter> _collectionshooters;

    public GroupDetailsView(DbContext context)
    {
      _participations = context.Set<t_participation>();
      _shooters = context.Set<t_shooter>();
      _people = context.Set<t_person>();
      _shooterParticipations = context.Set<t_shooterparticipation>();
      _shooterCollectionParticipation = context.Set<t_shootercollectionparticipation>();
      _shooterCollection = context.Set<t_shootercollection>();
      _collectionshooters = context.Set<t_collectionshooter>();
    }
    public IEnumerable<ParticipationDetails> GetAll()
    {
      IEnumerable<ParticipationDetails> view = from p in _participations
                                               join g in _shooterCollectionParticipation on p.ParticipationId equals g.ParticipationId
                                               join sc in _shooterCollection on g.ShooterCollectionId equals sc.ShooterCollectionId into gds
        select new ParticipationDetails
        {
          ParticipationDescription = p.Description,
          ParticipationNames = gds.OrderBy(_ => _.CollectionName).Select(_ => _.CollectionName)
        };

      return view;
    }

    public IEnumerable<ParticipationDetails> FindByShooterId(int shooterId)
    {

      IEnumerable<ParticipationDetails> view = from p in _participations
        join g in _shooterCollectionParticipation on p.ParticipationId equals g.ParticipationId
        join cs in _collectionshooters on g.ShooterCollectionId equals cs.ShooterCollectionId
        where cs.ShooterId == shooterId
        join sc in _shooterCollection on g.ShooterCollectionId equals sc.ShooterCollectionId into gds
        select new ParticipationDetails
        {
          ParticipationDescription = p.Description,
          ParticipationNames = gds.OrderBy(_ => _.CollectionName).Select(_ => _.CollectionName)
        };
      //IEnumerable<ParticipationDetails> view =
      //  from gd in _participation
      //  join g in
      //    (from g in _participations 
      //     join sp in _shooterParticipations on g.ParticipationId equals sp.ParticipationId 
      //     where sp.ShooterId == shooterId
      //     select g) on gd.ParticipationDescriptionId equals
      //    g.ParticipationDescriptionId into gds
      //  select new ParticipationDetails
      //  {
      //    ParticipationDescription = gd.Description,
      //    ParticipationNames = gds.OrderBy(_ => _.ParticipationName).Select(_ => _.ParticipationName)
      //  };

      return view;
    }

    public IEnumerable<ParticipationDetails> FindByPersonId(int personId)
    {
      IEnumerable<ParticipationDetails> view = from p in _participations
                                               join g in _shooterCollectionParticipation on p.ParticipationId equals g.ParticipationId
                                               join cs in _collectionshooters on g.ShooterCollectionId equals cs.ShooterCollectionId
                                               join s in _shooters on cs.ShooterId equals s.ShooterId
                                               where s.PersonId == personId
                                               join sc in _shooterCollection on g.ShooterCollectionId equals sc.ShooterCollectionId into gds
                                               select new ParticipationDetails
                                               {
                                                 ParticipationDescription = p.Description,
                                                 ParticipationNames = gds.OrderBy(_ => _.CollectionName).Select(_ => _.CollectionName)
                                               };

      //IEnumerable<ParticipationDetails> view =
      //  from gd in _participation
      //  join g in
      //    (from g in _participations
      //     join s in _shooterParticipations on g.ParticipationId equals s.ParticipationId 
      //     join p in _shooters on s.ShooterId equals p.ShooterId
      //     where p.PersonId == personId
      //     select g) on gd.ParticipationDescriptionId equals
      //    g.ParticipationDescriptionId into gds
      //  select new ParticipationDetails
      //  {
      //    ParticipationDescription = gd.Description,
      //    ParticipationNames = gds.OrderBy(_ => _.ParticipationName).Select(_ => _.ParticipationName)
      //  };

      return view;
    }
  }
}
