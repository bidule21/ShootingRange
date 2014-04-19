using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ShootingRange.BusinessObjects;
using ShootingRange.BusinessObjects.Properties;
using ShootingRange.Common;
using ShootingRange.ConfigurationProvider;

namespace ShootingRange.ViewModel
{
  public enum Gender
  {
    None = 0,
    Male,
    Female
  }

  public class PersonEditViewModel : INotifyPropertyChanged
  {
    public PersonEditViewModel()
    {
      IConfiguration config = ConfigurationSource.Configuration;
      UIEvents uiEvents = config.GetUIEvents();
      uiEvents.PersonSelected += UiEventPersonSelected;
    }

    private void UiEventPersonSelected(Person person)
    {
      FirstName = person.FirstName;
      LastName = person.LastName;
      Address = person.Address;
      ZipCode = person.ZipCode;
      City = person.City;
      Email = person.Email;
      Phone = person.Phone;
      DateOfBirthTime = person.DateOfBirth;
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

    
    private string _address;
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

    
    private string _zipCode;
    public string ZipCode
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

    
    private string _city;
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

    
    private string _email;
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

    
    private string _phone;
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

    
    private DateTime _dateOfBirth;
    public DateTime DateOfBirthTime
    {
      get { return _dateOfBirth; }
      set
      {
        if (value != _dateOfBirth)
        {
          _dateOfBirth = value;
          OnPropertyChanged("DateOfBirthTime");
        }
      }
    }

    
    private Gender _gender;

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
