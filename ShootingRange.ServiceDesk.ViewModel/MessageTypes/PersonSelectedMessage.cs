using ShootingRange.BusinessObjects;

namespace ShootingRange.ServiceDesk.ViewModel.MessageTypes
{
    public class PersonSelectedMessage
    {
        public Person Person { get; set; }

        public PersonSelectedMessage(Person person)
        {
            Person = person;
        }
    }
}