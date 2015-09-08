using Gui.ViewModel;

namespace ShootingRange.ServiceDesk.ViewModel.MessageTypes
{
    public class DialogMessage
    {
        public string Caption { get; set; }
        public string Message { get; set; }
        public MessageIcon Icon { get; set; }

        public DialogMessage(string caption, string message, MessageIcon icon)
        {
            Caption = caption;
            Message = message;
            Icon = icon;
        }
    }
}