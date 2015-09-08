namespace ShootingRange.ServiceDesk.ViewModel.MessageTypes
{
    public class SetSelectedPersonMessage
    {
        public SetSelectedPersonMessage(int personId)
        {
            PersonId = personId;
        }

        public int PersonId { get; set; }
    }
}