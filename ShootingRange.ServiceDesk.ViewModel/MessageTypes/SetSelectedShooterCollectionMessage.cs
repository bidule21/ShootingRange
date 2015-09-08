namespace ShootingRange.ServiceDesk.ViewModel.MessageTypes
{
    public class SetSelectedShooterCollectionMessage
    {
        public SetSelectedShooterCollectionMessage(int shooterCollectionId)
        {
            ShooterCollectionId = shooterCollectionId;
        }

        public int ShooterCollectionId { get; set; }
    }
}