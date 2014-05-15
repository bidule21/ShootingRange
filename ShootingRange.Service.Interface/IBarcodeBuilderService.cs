using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ShootingRange.Service.Interface
{
  public interface IBarcodeBuilderService
  {
    string BuildBarcode(int shooterNumber, int legalization);
  }
}
