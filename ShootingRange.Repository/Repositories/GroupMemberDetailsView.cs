using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ShootingRange.BusinessObjects;
using ShootingRange.Entities;

namespace ShootingRange.Repository.Repositories
{
  public class GroupMemberDetailsView : IGroupMemberDetailsView
  {
    private DbSet<t_group> _groups;
    private DbSet<t_shooter> _shooters;
    private DbSet<t_groupdescription> _groupDescriptions;
    private DbSet<t_person> _people;

    public GroupMemberDetailsView(DbContext context)
    {
      _groups = context.Set<t_group>();
      _shooters = context.Set<t_shooter>();
      _people = context.Set<t_person>();
      _groupDescriptions = context.Set<t_groupdescription>();
    }

    public IEnumerable<GroupMemberDetails> GetAll()
    {
      IEnumerable<GroupMemberDetails> view = 
        from g in _groups
        join s in _shooters on g.GroupId equals s.GroupId
        join p in _people on s.PersonId equals p.PersonId
        join gd in _groupDescriptions on g.GroupDescriptionId equals gd.GroupDescriptionId into gj
        from sgj in gj.DefaultIfEmpty()
        select new GroupMemberDetails
        {
          PersonId = p.PersonId,
          GroupId = g.GroupId,
          ShooterId = s.ShooterId,
          GroupName = g.GroupName,
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
