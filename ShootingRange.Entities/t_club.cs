//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ShootingRange.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class t_club
    {
        public t_club()
        {
            this.t_shooter = new HashSet<t_shooter>();
        }
    
        public int ClubId { get; set; }
        public string ClubName { get; set; }
        public Nullable<int> Zip { get; set; }
        public string City { get; set; }
    
        public virtual ICollection<t_shooter> t_shooter { get; set; }
    }
}