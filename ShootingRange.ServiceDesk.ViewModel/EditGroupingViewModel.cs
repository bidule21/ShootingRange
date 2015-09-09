using System.ComponentModel;
using System.Runtime.CompilerServices;
using Gui.ViewModel;

namespace ShootingRange.ServiceDesk.ViewModel
{
    public class EditGroupingViewModel : INotifyPropertyChanged
    {
        public EditGroupingViewModel()
        {
            OkCommand = new ViewModelCommand(x =>
            {
                ((IWindow)x).Close();
            });
            OkCommand.AddGuard(x => !string.IsNullOrWhiteSpace(GroupingName));
        }

        public ViewModelCommand OkCommand { get; private set; }


        private int _groupingId;
        public int GroupingId
        {
            get { return _groupingId; }
            set
            {
                if (value != _groupingId)
                {
                    _groupingId = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _groupingName;

        public string GroupingName
        {
            get { return _groupingName; }
            set
            {
                if (value != _groupingName)
                {
                    _groupingName = value;
                    OnPropertyChanged();
                    OkCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                if (value != _title)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}