namespace ShootingRange.ServiceDesk.ViewModel.MessageTypes
{
    public class AddParticipationToShooterDialogMessage
    {
        public int ShooterId { get; set; }

        public AddParticipationToShooterDialogMessage(int shooterId)
        {
            ShooterId = shooterId;
        }
    }
}