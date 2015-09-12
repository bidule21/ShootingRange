using System;

namespace ShootingRange.ServiceDesk.ViewModel
{
    public class UiPerson
    {
        public UiPerson()
        {
            
        }

        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool HasShooters { get; set; }
    }
}