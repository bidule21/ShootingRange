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
    
    public partial class t_programsubtotal
    {
        public int ProgramSubtotalId { get; set; }
        public int ProgramItemId { get; set; }
        public Nullable<int> SubtotalOrdinal { get; set; }
        public Nullable<int> NumberOfShots { get; set; }
    
        public virtual t_programitem t_programitem { get; set; }
    }
}
