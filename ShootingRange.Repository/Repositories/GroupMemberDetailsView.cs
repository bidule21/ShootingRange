using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.Win32.SafeHandles;
using ShootingRange.BusinessObjects;
using ShootingRange.Entities;
using ShootingRange.Repository.RepositoryInterfaces;

namespace ShootingRange.Repository.Repositories
{
  public class GroupMemberDetailsView : IGroupMemberDetailsView
  {
    private DbSet<t_participation> _participations;
    private DbSet<t_shooter> _shooters;
    private DbSet<t_person> _people;
    private DbSet<t_shooterparticipation> _shooterParticipations;
    private DbSet<t_shootercollectionparticipation> _shooterCollectionParticipation;
    private DbSet<t_shootercollection> _shooterCollection;
    private DbSet<t_collectionshooter> _collectionshooters;

    public GroupMemberDetailsView(DbContext context)
    {
      _participations = context.Set<t_participation>();
      _shooters = context.Set<t_shooter>();
      _people = context.Set<t_person>();
      _participations = context.Set<t_participation>();
      _shooterParticipations = context.Set<t_shooterparticipation>();
      _shooterCollectionParticipation = context.Set<t_shootercollectionparticipation>();
      _shooterCollection = context.Set<t_shootercollection>();
      _collectionshooters = context.Set<t_collectionshooter>();
    }

    public IEnumerable<GroupMemberDetails> GetAll()
    {
      IEnumerable<GroupMemberDetails> view = 
        from pa in _participations
        join sp in _shooterCollectionParticipation on pa.ParticipationId equals sp.ParticipationId
        join sc in _shooterCollection on sp.ShooterCollectionId equals sc.ShooterCollectionId
        join cs in _collectionshooters on sc.ShooterCollectionId equals cs.ShooterCollectionId
        join s in _shooters on cs.ShooterId equals s.ShooterId
        join p in _people on s.PersonId equals p.PersonId
        join gd in _participations on pa.Description equals gd.Description into gj
        from sgj in gj.DefaultIfEmpty()
        select new GroupMemberDetails
        {
          PersonId = p.PersonId,
          GroupId = pa.ParticipationId,
          ShooterId = s.ShooterId,
          GroupName = pa.Description,
          GroupDescription = (sgj == null ? string.Empty : sgj.Description)
        };

      return view;
    }

    public IEnumerable<GroupMemberDetails> FindByGroupId(int groupId)
    {
      return GetAll().Where(_ => _.GroupId == groupId);
    }

    public IEnumerable<GroupMemberDetails> FindByShooterId(int shooterId)
    {
      return GetAll().Where(_ => _.ShooterId == shooterId);
    }

    public IEnumerable<GroupMemberDetails> FindByPersonId(int personId)
    {
      return GetAll().Where(_ => _.PersonId == personId);
    }
  }
}
