namespace ShootingRange.Service.Interface
{
  public interface IBarcodePrintService
  {
    void Print(BarcodeHerbstschiessen barcodeInfo);
    void Print(BarcodeFruehlingsschiessen barcodeInfo);
  }
}