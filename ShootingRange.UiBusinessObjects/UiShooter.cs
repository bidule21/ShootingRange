using System;
using System.Collections.Generic;

namespace ShootingRange.UiBusinessObjects
{
  public class UiShooter
  {
    public int ShooterId { get; set; }
    public int ShooterNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int? PersonId { get; set; }
    public int Legalization { get; set; }
  }

  public class UiShooterComparer : IEqualityComparer<UiShooter>
  {
    // Products are equal if their names and uiShooter numbers are equal.
    public bool Equals(UiShooter x, UiShooter y)
    {
      //Check whether the compared objects reference the same data.
      if (Object.ReferenceEquals(x, y)) return true;

      //Check whether any of the compared objects is null.
      if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
        return false;

      //Check whether the products' properties are equal.
      return x.ShooterId == y.ShooterId;
    }

    // If Equals() returns true for a pair of objects 
    // then GetHashCode() must return the same value for these objects.
    public int GetHashCode(UiShooter uiShooter)
    {
      //Check whether the object is null
      if (Object.ReferenceEquals(uiShooter, null)) return 0;

      //Get hash code for the Name field if it is not null.
      int hashProductName = uiShooter.ShooterId.GetHashCode();

      //Get hash code for the Code field.
      int hashProductCode = uiShooter.ShooterId.GetHashCode();

      //Calculate the hash code for the uiShooter.
      return hashProductName ^ hashProductCode;
    }
  }
}
