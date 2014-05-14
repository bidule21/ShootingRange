using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.Win32.SafeHandles;
using ShootingRange.BusinessObjects;
using ShootingRange.Entities;

namespace ShootingRange.Repository.Repositories
{
  public class GroupMemberDetailsView : IGroupMemberDetailsView
  {
    private DbSet<t_participation> _participations;
    private DbSet<t_shooter> _shooters;
    private DbSet<t_participationdescription> _participationDescriptions;
    private DbSet<t_person> _people;
    private DbSet<t_shooterparticipation> _shooterParticipations;

    public GroupMemberDetailsView(DbContext context)
    {
      _participations = context.Set<t_participation>();
      _shooters = context.Set<t_shooter>();
      _people = context.Set<t_person>();
      _participationDescriptions = context.Set<t_participationdescription>();
      _shooterParticipations = context.Set<t_shooterparticipation>();
    }

    public IEnumerable<GroupMemberDetails> GetAll()
    {
      IEnumerable<GroupMemberDetails> view = 
        from pa in _participations
        join sp in _shooterParticipations on pa.ParticipationId equals sp.ParticipationId
        join s in _shooters on sp.ShooterId equals s.ShooterId
        join p in _people on s.PersonId equals p.PersonId
        join gd in _participationDescriptions on pa.ParticipationDescriptionId equals gd.ParticipationDescriptionId into gj
        from sgj in gj.DefaultIfEmpty()
        select new GroupMemberDetails
        {
          PersonId = p.PersonId,
          GroupId = pa.ParticipationId,
          ShooterId = s.ShooterId,
          GroupName = pa.ParticipationName,
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
