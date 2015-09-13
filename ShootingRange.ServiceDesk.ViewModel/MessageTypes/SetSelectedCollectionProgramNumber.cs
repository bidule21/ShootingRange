namespace ShootingRange.ServiceDesk.ViewModel.MessageTypes
{
    public class SetSelectedCollectionProgramNumber
    {
        public int ProgramNumber { get; set; }

        public SetSelectedCollectionProgramNumber(int programNumber)
        {
            ProgramNumber = programNumber;
        }
    }
}