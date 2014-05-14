using System;
using ShootingRange.BusinessObjects;
using ShootingRange.UiBusinessObjects;

namespace ShootingRange.ViewModel
{
  public static class UiBusinessObjectMapper
  {
    public static Person ToPerson(this UiPerson uiPerson)
    {
      return new Person()
      {
        PersonId = uiPerson.PersonId,
        FirstName = uiPerson.FirstName,
        LastName = uiPerson.LastName,
        Address = uiPerson.Address,
        ZipCode = uiPerson.ZipCode,
        City = uiPerson.City,
        DateOfBirth = uiPerson.DateOfBirth,
        Email = uiPerson.Email,
        Phone = uiPerson.Phone
      };
    }

    public static Func<Person, UiPerson> ToUiPerson = person => new UiPerson
    {
      PersonId = person.PersonId,
      FirstName = person.FirstName,
      LastName = person.LastName,
      Address = person.Address,
      ZipCode = person.ZipCode,
      City = person.City,
      DateOfBirth = person.DateOfBirth,
      Email = person.Email,
      Phone = person.Phone
    };

    public static Shooter ToShooter(this UiShooter uiShooter)
    {
      return new Shooter()
      {
        ShooterId = uiShooter.ShooterId,
        ShooterNumber = uiShooter.ShooterNumber
      };
    }

    public static Func<Shooter, UiShooter> ToUiShooter = shooter => new UiShooter()
    {
      ShooterId = shooter.ShooterId,
      ShooterNumber = shooter.ShooterNumber
    };
  }
}
