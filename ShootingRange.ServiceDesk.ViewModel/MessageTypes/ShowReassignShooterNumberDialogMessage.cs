namespace ShootingRange.ServiceDesk.ViewModel.MessageTypes
{
    public class ShowReassignShooterNumberDialogMessage
    {
        public int SessionId { get; set; }

        public ShowReassignShooterNumberDialogMessage(int sessionId)
        {
            SessionId = sessionId;
        }
    }
}