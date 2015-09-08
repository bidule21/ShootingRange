namespace ShootingRange.Service.Interface
{
  public interface IBarcodeBuilderService
  {
    string BuildBarcode(int shooterNumber, int legalization);
  }
}
