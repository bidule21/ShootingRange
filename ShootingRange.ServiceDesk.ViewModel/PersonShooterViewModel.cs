using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ShootingRange.BusinessObjects;
using ShootingRange.BusinessObjects.Properties;

namespace ShootingRange.ServiceDesk.ViewModel
{
    public class PersonShooterViewModel : INotifyPropertyChanged
    {
        public PersonShooterViewModel()
        {
            
        }

        public PersonShooterViewModel(Person person, Shooter shooter)
        {
            PersonId = person.PersonId;
            ShooterId = shooter.ShooterId;
            FirstName = person.FirstName;
            LastName = person.LastName;
            ShooterNumber = shooter.ShooterNumber;
            DateOfBirth = person.DateOfBirth;
        }

        private int _personId;

        public int PersonId
        {
            get { return _personId; }
            set
            {
                if (value != _personId)
                {
                    _personId = value;
                    OnPropertyChanged("PersonId");
                }
            }
        }

        private int _shooterId;

        public int ShooterId
        {
            get { return _shooterId; }
            set
            {
                if (value != _shooterId)
                {
                    _shooterId = value;
                    OnPropertyChanged("ShooterId");
                }
            }
        }

        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (value != _firstName)
                {
                    _firstName = value;
                    OnPropertyChanged("FirstName");
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
                }
            }
        }


        private int _shooterNumber;

        public int ShooterNumber
        {
            get { return _shooterNumber; }
            set
            {
                if (value != _shooterNumber)
                {
                    _shooterNumber = value;
                    OnPropertyChanged("ShooterNumber");
                }
            }
        }



        private DateTime? _dateOfBirth;

        public DateTime? DateOfBirth
        {
            get { return _dateOfBirth; }
            set
            {
                if (value != _dateOfBirth)
                {
                    _dateOfBirth = value;
                    OnPropertyChanged("DateOfBirth");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}