namespace ShootingRange.ServiceDesk.ViewModel.MessageTypes
{
    public class DeleteGroupingDialogMessage
    {
        private readonly ShooterCollectionViewModel _shooterCollection;

        public DeleteGroupingDialogMessage(ShooterCollectionViewModel shooterCollection)
        {
            _shooterCollection = shooterCollection;
        }

        public int ShooterCollectionId
        {
            get { return _shooterCollection.ShooterCollectionId; }
        }

        public string CollectionName
        {
            get { return _shooterCollection.CollectionName; }
        }
    }
}