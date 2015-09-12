using ShootingRange.BusinessObjects;

namespace ShootingRange.ServiceDesk.ViewModel.MessageTypes
{
    public class PersonSelectedMessage
    {
        public int PersonId { get; set; }

        public PersonSelectedMessage(int personId)
        {
            PersonId = personId;
        }
    }
}