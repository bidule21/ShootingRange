namespace ShootingRange.ServiceDesk.ViewModel.MessageTypes
{
    public class RemoveParticipationFromShooterDialogMessage
    {
        public int ShooterId { get; set; }
        public ParticipationViewModel Participation { get; set; }

        public RemoveParticipationFromShooterDialogMessage(int shooterId, ParticipationViewModel participation)
        {
            ShooterId = shooterId;
            Participation = participation;
        }
    }
}