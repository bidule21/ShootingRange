using System;
using System.IO;
using bpac;
using ShootingRange.Service.Interface;

namespace ShootingRange.BarcodePrinter
{
  public class PtouchBarcodePrinter : IBarcodePrintService
  {
    public void Print(BarcodeHerbstschiessen barcodeInfo)
    {
      DocumentClass doc = new DocumentClass();
      const string path = @".\Templates\Herbstschiessen.lbx";
      string fullPath = Path.GetFullPath(path);
      if (doc.Open(fullPath))
      {
        IObject barcode = doc.GetObject("barcode");
        barcode.Text = Convert.ToString(barcodeInfo.Barcode);

        IObject shooterName = doc.GetObject("shooterName");
        shooterName.Text = Convert.ToString(barcodeInfo.FirstName + " " + barcodeInfo.LastName);

        IObject dateOfBirth = doc.GetObject("dateOfBirth");
        dateOfBirth.Text = Convert.ToString(barcodeInfo.DateOfBirth != null ? ((DateTime)barcodeInfo.DateOfBirth).ToString("dd.MM.yyyy") : string.Empty);

        IObject groupName = doc.GetObject("GroupName");
        groupName.Text = Convert.ToString(barcodeInfo.Gruppenstich);

        IObject sieUndErName = doc.GetObject("SieUndErName");
        sieUndErName.Text = Convert.ToString(barcodeInfo.SieUndEr);

        IObject isGruppenstich = doc.GetObject("pass_103");
        isGruppenstich.Text = Convert.ToString(barcodeInfo.IsGruppenstich ? "x" : string.Empty);

        IObject isNachwuchsstich = doc.GetObject("pass_104");
        isNachwuchsstich.Text = Convert.ToString(barcodeInfo.IsNachwuchsstich ? "x" : string.Empty);

        IObject isWorschtUndBrot = doc.GetObject("pass_101");
        isWorschtUndBrot.Text = Convert.ToString(barcodeInfo.IsWorschtUndBrot ? "x" : string.Empty);

        IObject isSieUndEr = doc.GetObject("pass_102");
        isSieUndEr.Text = Convert.ToString(barcodeInfo.IsSieUndEr ? "x" : string.Empty);

        doc.StartPrint("", PrintOptionConstants.bpoDefault);
        doc.PrintOut(1, PrintOptionConstants.bpoDefault);
        doc.EndPrint();
        doc.Close();
      }
      else
      {
        throw new InvalidOperationException(string.Format("Can not open template file '{0}'", fullPath));
      }
    }

    public void Print(BarcodeVolksschiessen barcodeInfo)
    {
      DocumentClass doc = new DocumentClass();
      const string path = @".\Templates\Volksschiessen_2015.lbx";
      string fullPath = Path.GetFullPath(path);
      if (doc.Open(fullPath))
      {
        IObject barcode = doc.GetObject("barcode");
        barcode.Text = Convert.ToString(barcodeInfo.Barcode);

        IObject shooterName = doc.GetObject("shooterName");
        shooterName.Text = Convert.ToString(barcodeInfo.FirstName + " " + barcodeInfo.LastName);

        IObject dateOfBirth = doc.GetObject("dateOfBirth");
        dateOfBirth.Text = Convert.ToString(barcodeInfo.DateOfBirth != null ? ((DateTime)barcodeInfo.DateOfBirth).ToString("dd.MM.yyyy") : string.Empty);

        IObject groupName = doc.GetObject("GroupName");
        groupName.Text = Convert.ToString(barcodeInfo.Gruppenstich ?? string.Empty);

        IObject isGroup = doc.GetObject("isGruppen");
        isGroup.Text = Convert.ToString(barcodeInfo.Gruppenstich == null ? string.Empty : "x");

        doc.StartPrint("", PrintOptionConstants.bpoDefault);
        doc.PrintOut(1, PrintOptionConstants.bpoDefault);
        doc.EndPrint();
        doc.Close();
      }
      else
      {
        throw new InvalidOperationException(string.Format("Can not open template file '{0}'", fullPath));
      }
    }
  }
}
