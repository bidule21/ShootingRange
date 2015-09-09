namespace ShootingRange.ServiceDesk.ViewModel.MessageTypes
{
    public class EditGroupingDialogMessage
    {
        public EditGroupingDialogMessage(int groupingId, string groupingName)
        {
            GroupingName = groupingName;
            GroupingId = groupingId;
        }

        public string GroupingName { get; private set; }
        public int GroupingId { get; private set; }
    }
}