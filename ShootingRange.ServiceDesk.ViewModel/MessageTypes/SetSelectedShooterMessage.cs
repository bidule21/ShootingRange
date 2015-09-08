namespace ShootingRange.ServiceDesk.ViewModel.MessageTypes
{
    public class SetSelectedShooterMessage
    {
        public int ShooterId { get; set; }

        public SetSelectedShooterMessage(int shooterId)
        {
            ShooterId = shooterId;
        }
    }
}