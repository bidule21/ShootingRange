namespace ShootingRange.ServiceDesk.ViewModel.MessageTypes
{
    public class AddGroupingToShooterDialogMessage
    {
        public AddGroupingToShooterDialogMessage(int shooterId)
        {
            ShooterId = shooterId;
        }

        public int ShooterId { get; set; }
    }
}