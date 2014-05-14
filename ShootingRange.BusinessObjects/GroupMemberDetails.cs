using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShootingRange.BusinessObjects
{
  public class GroupMemberDetails
  {
    public int GroupId { get; set; }
    public int ShooterId { get; set; }
    public int PersonId { get; set; }
    public string GroupName { get; set; }
    public string GroupDescription { get; set; }
  }
}
