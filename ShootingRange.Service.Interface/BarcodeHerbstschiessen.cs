using System;

namespace ShootingRange.Service.Interface
{
  public class BarcodeVolksschiessen
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string Barcode { get; set; }
    public string Gruppenstich { get; set; }
  }
}
