using System;

namespace ShootingRange.BusinessObjects
{
  public class Person
  {
    public int PersonId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public int? ZipCode { get; set; }
    public string City { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }

    public Person()
    {
      
    }

    public Person(Person person)
    {
      PersonId = person.PersonId;
      FirstName = person.FirstName;
      LastName = person.LastName;
      Address = person.Address;
      ZipCode = person.ZipCode;
      City = person.City;
      Email = person.Email;
      Phone = person.Phone;
      DateOfBirth = person.DateOfBirth;
    }

    public override string ToString()
    {
      return string.Format("LastName: {0}, FirstName: {1}", LastName, FirstName);
    }
  }
}
