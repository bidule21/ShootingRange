//------------------------------------------------------------------------------
// <auto-generated>
//    Dieser Code wurde aus einer Vorlage generiert.
//
//    Manuelle Änderungen an dieser Datei führen möglicherweise zu unerwartetem Verhalten Ihrer Anwendung.
//    Manuelle Änderungen an dieser Datei werden überschrieben, wenn der Code neu generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ShootingRange.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class t_session
    {
        public t_session()
        {
            this.t_rankignoresession = new HashSet<t_rankignoresession>();
            this.t_sessionsubtotal = new HashSet<t_sessionsubtotal>();
        }
    
        public int SessionId { get; set; }
        public Nullable<int> ProgramItemId { get; set; }
        public int ShooterId { get; set; }
        public int LaneNumber { get; set; }
    
        public virtual t_programitem t_programitem { get; set; }
        public virtual ICollection<t_rankignoresession> t_rankignoresession { get; set; }
        public virtual t_shooter t_shooter { get; set; }
        public virtual ICollection<t_sessionsubtotal> t_sessionsubtotal { get; set; }
    }
}
