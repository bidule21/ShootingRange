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
    private DbSet<t_group> _groups;
    private DbSet<t_person> _people;
    private DbSet<t_shooter> _shooters;
    private DbSet<t_groupdescription> _groupDescriptions;

    public GroupDetailsView(DbContext context)
    {
      _groups = context.Set<t_group>();
      _shooters = context.Set<t_shooter>();
      _people = context.Set<t_person>();
      _groupDescriptions = context.Set<t_groupdescription>();
    }
    public IEnumerable<GroupDetails> GetAll()
    {
      IEnumerable<GroupDetails> view = from gd in _groupDescriptions
        join g in _groups on gd.GroupDescriptionId equals g.GroupDescriptionId into gds
        select new GroupDetails
        {
          GroupDescription = gd.Description,
          GroupNames = gds.Select(_ => _.GroupName)
        };

      return view;
    }

    public IEnumerable<GroupDetails> FindByShooterId(int shooterId)
    {
      IEnumerable<GroupDetails> view =
        from gd in _groupDescriptions
        join g in
          (from g in _groups 
           join s in _shooters on g.GroupId equals s.GroupId 
           where s.ShooterId == shooterId
           select g) on gd.GroupDescriptionId equals
          g.GroupDescriptionId into gds
        select new GroupDetails
        {
          GroupDescription = gd.Description,
          GroupNames = gds.Select(_ => _.GroupName)
        };

      return view;
    }

    public IEnumerable<GroupDetails> FindByPersonId(int personId)
    {
      IEnumerable<GroupDetails> view =
        from gd in _groupDescriptions
        join g in
          (from g in _groups
           join s in _shooters on g.GroupId equals s.GroupId 
           where s.PersonId == personId
           select g) on gd.GroupDescriptionId equals
          g.GroupDescriptionId into gds
        select new GroupDetails
        {
          GroupDescription = gd.Description,
          GroupNames = gds.Select(_ => _.GroupName)
        };

      return view;
    }
  }
}
