using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using ShootingRange.UiBusinessObjects.Annotations;

namespace ShootingRange.UiBusinessObjects
{
  [DebuggerDisplay("{LastName} {FirstName}")]
  public class UiPerson : INotifyPropertyChanged
  {
    private int _personId;
    private string _firstName;
    private string _lastName;
    private string _address;
    private int? _zipCode;
    private string _city;
    private string _email;
    private string _phone;
    private DateTime? _dateOfBirth;
    private Gender _gender;

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

    public string Address
    {
      get { return _address; }
      set
      {
        if (value != _address)
        {
          _address = value;
          OnPropertyChanged("Address");
        }
      }
    }

    public int? ZipCode
    {
      get { return _zipCode; }
      set
      {
        if (value != _zipCode)
        {
          _zipCode = value;
          OnPropertyChanged("ZipCode");
        }
      }
    }

    public string City
    {
      get { return _city; }
      set
      {
        if (value != _city)
        {
          _city = value;
          OnPropertyChanged("City");
        }
      }
    }

    public string Email
    {
      get { return _email; }
      set
      {
        if (value != _email)
        {
          _email = value;
          OnPropertyChanged("Email");
        }
      }
    }

    public string Phone
    {
      get { return _phone; }
      set
      {
        if (value != _phone)
        {
          _phone = value;
          OnPropertyChanged("Phone");
        }
      }
    }

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


    public Gender Gender
    {
      get { return _gender; }
      set
      {
        if (value != _gender)
        {
          _gender = value;
          OnPropertyChanged("Gender");
        }
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
