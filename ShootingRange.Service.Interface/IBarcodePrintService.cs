namespace ShootingRange.Service.Interface
{
    public interface IBarcodePrintService
    {
        void Print(BarcodeHerbstschiessen barcodeInfo);
        void Print(BarcodeVolksschiessen barcodeInfo);

        void Print(GenericBarcode_20150909 barcodeInfo);
    }
 }