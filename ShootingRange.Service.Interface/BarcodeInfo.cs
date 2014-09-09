using System;

namespace ShootingRange.Service.Interface
{
  public class BarcodeInfo
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string Barcode { get; set; }
    public string SieUndEr { get; set; }
    public string Gruppenstich { get; set; }

    public bool IsGruppenstich { get; set; }
    public bool IsWorschtUndBrot { get; set; }
    public bool IsNachwuchsstich { get; set; }
    public bool IsSieUndEr { get; set; }
  }
}
