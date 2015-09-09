using System;
using ShootingRange.Service.Interface;

namespace ShootingRange.Service
{
  public class Barcode2Of5InterleavedService : IBarcodeBuilderService
  {
    private const int SHOOTER_DIGIT = 1;
    private const int MODULO = 97;

    public string BuildBarcode(int shooterNumber, int legalization)
    {
      if (shooterNumber < 0 || shooterNumber > 999999) throw new ArgumentException("Shooter number must be in the range of [0,999999].","shooterNumber");
      if (legalization < 0 || legalization > 9) throw new ArgumentException("Legalization must be in the range [0,9].","legalization");

      string baseNumber = CreateBaseNumber(shooterNumber, legalization);
      int sum = 0;
      for (int i = 0; i < baseNumber.Length; i += 2)
      {
        int m1 = IntPow(3, i / 2 + 1);
        int m2 = 10 * m1;

        int i1 = baseNumber.Length - i - 1;
        int i2 = baseNumber.Length - i - 2;

        int d1 = Convert.ToInt32(new string(baseNumber[i1], 1));
        int d2 = Convert.ToInt32(new string(baseNumber[i2], 1));
        sum += (d1 * m1 + d2 * m2);
      }

      int checksum = MODULO - (sum % MODULO);

      return string.Format("{0}{1}{2}{3}",
                      SHOOTER_DIGIT.ToString("D1"),
                      legalization.ToString("D1"),
                      shooterNumber.ToString("D6"),
                      checksum.ToString("D2"));
    }

    private string CreateBaseNumber(int ticketNumber, int legalization)
    {
      if (legalization > 9)
          throw new ArgumentException("Legalizations greater than 9 are not allowed.", "legalization");
      if (ticketNumber > 999999)
          throw new ArgumentException("Shooter numbers greater than 999999 are not allowed.", "ticketNumber");

      string ticketString = ticketNumber.ToString("D6");
      string legalString = legalization.ToString("D1");
      return string.Format("{0}{1}{2}", SHOOTER_DIGIT, legalString, ticketString);
    }

    private static int IntPow(int a, int b)
    {
      int answer = a;
      for (int i = 0; i < b - 1; i++)
        answer *= a;
      return answer;
    }
  }
}
