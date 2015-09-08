namespace ShootingRange.ServiceDesk.ViewModel.MessageTypes
{
    public class AddShooterToGroupingDialogMessage
    {
        public AddShooterToGroupingDialogMessage(int programNumber, int shooterCollectionId)
        {
            ProgramNumber = programNumber;
            ShooterCollectionId = shooterCollectionId;
        }

        public int ProgramNumber { get; set; }
        public int ShooterCollectionId { get; set; }
    }
}