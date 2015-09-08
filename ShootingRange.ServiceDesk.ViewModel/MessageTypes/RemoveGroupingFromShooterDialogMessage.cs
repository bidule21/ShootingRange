namespace ShootingRange.ServiceDesk.ViewModel.MessageTypes
{
    public class RemoveGroupingFromShooterDialogMessage
    {
        public RemoveGroupingFromShooterDialogMessage(int shooterId, GroupingViewModel grouping)
        {
            ShooterId = shooterId;
            Grouping = grouping;
        }

        public int ShooterId { get; set; }

        public GroupingViewModel Grouping { get; set; }
    }
}