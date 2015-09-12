using System;
using ShootingRange.BusinessObjects;

namespace ShootingRange.ServiceDesk.ViewModel.MessageTypes
{
    public class DeletePersonDialogMessage
    {
        private readonly UiPerson _person;

        public DeletePersonDialogMessage(UiPerson person)
        {
            if (person == null) throw new ArgumentNullException("person");
            _person = person;
        }

        public int PersonId
        {
            get { return _person.PersonId; }
            set { _person.PersonId = value; }
        }

        public string FirstName
        {
            get { return _person.FirstName; }
            set { _person.FirstName = value; }
        }

        public string LastName
        {
            get { return _person.LastName; }
            set { _person.LastName = value; }
        }

        //public string Address
        //{
        //    get { return _person.Address; }
        //    set { _person.Address = value; }
        //}

        //public int? ZipCode
        //{
        //    get { return _person.ZipCode; }
        //    set { _person.ZipCode = value; }
        //}

        //public string City
        //{
        //    get { return _person.City; }
        //    set { _person.City = value; }
        //}

        //public string Email
        //{
        //    get { return _person.Email; }
        //    set { _person.Email = value; }
        //}

        //public string Phone
        //{
        //    get { return _person.Phone; }
        //    set { _person.Phone = value; }
        //}

        //public DateTime? DateOfBirth
        //{
        //    get { return _person.DateOfBirth; }
        //    set { _person.DateOfBirth = value; }
        //}
    }
}