using Gui.ViewModel;

namespace ShootingRange.ServiceDesk.ViewModel.MessageTypes
{
    public class CreateGroupingDialogMessage
    {
        public IWindow Parent { get; set; }

        public CreateGroupingDialogMessage(IWindow parent)
        {
            Parent = parent;
        }
    }
}