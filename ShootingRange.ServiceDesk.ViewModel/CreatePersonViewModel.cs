using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Gui.ViewModel;

namespace ShootingRange.ServiceDesk.ViewModel
{
    public class CreatePersonViewModel : INotifyPropertyChanged
    {
        public CreatePersonViewModel()
        {
            OkCommand = new ViewModelCommand(x =>
            {
                ((IWindow)x).Close();
            });
            OkCommand.AddGuard(x => !(string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName)));
        }

        public ViewModelCommand OkCommand { get; set; }


        
        private DateTime? _dateOfBirthTime;
        public DateTime? DateOfBirth
        {
            get { return _dateOfBirthTime; }
            set
            {
                if (value != _dateOfBirthTime)
                {
                    _dateOfBirthTime = value;
                    OnPropertyChanged("DateOfBirth");
                }
            }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (value != _lastName)
                {
                    _lastName = value;
                    OnPropertyChanged("LastName");
                    OkCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private string  _firstName;
        public string  FirstName
        {
            get { return _firstName; }
            set
            {
                if (value != _firstName)
                {
                    _firstName = value;
                    OnPropertyChanged("FirstName");
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
                    OnPropertyChanged("Title");
                }
            }
        }

        public int PersonId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int? ZipCode { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}