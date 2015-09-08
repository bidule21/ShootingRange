namespace ShootingRange.ServiceDesk.ViewModel.MessageTypes
{
    public class DeleteShooterDialogMessage
    {
        public int ShooterNumber { get; set; }

        public DeleteShooterDialogMessage(int shooterNumber)
        {
            ShooterNumber = shooterNumber;
        }
    }
}