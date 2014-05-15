using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingRange.Service.Interface
{
  public interface IBarcodePrintService
  {
    void Print(BarcodeInfo barcodeInfo);
  }
}