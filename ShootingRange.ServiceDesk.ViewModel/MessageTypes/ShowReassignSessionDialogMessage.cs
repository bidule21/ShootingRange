namespace ShootingRange.ServiceDesk.ViewModel.MessageTypes
{
    public class ShowReassignSessionDialogMessage
    {
        public int SessionId { get; set; }

        public ShowReassignSessionDialogMessage(int sessionId)
        {
            SessionId = sessionId;
        }
    }
}