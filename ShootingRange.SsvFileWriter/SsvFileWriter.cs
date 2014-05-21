using System;
using System.Globalization;
using System.IO;
using System.Text;
using ShootingRange.Service.Interface;

namespace SiusUtils
{
  public class SsvFileWriter : ISsvShooterDataWriterService
  {
    private readonly string _filePath;

    public SsvFileWriter(string filePath)
    {
      _filePath = filePath;
    }

    private const string SEPARATOR = ";";
    private const int NBR_COLUMNS = 32;

    public void WriteShooterData(SsvShooterData ssvShooterData)
    {
      if (File.Exists(_filePath))
      {
        string[] data = new string[NBR_COLUMNS];
        data[0] = ssvShooterData.LicenseNumber.ToString(CultureInfo.InvariantCulture);
        data[1] = ssvShooterData.LastName;
        data[2] = ssvShooterData.FirstName;
        string s = string.Join(SEPARATOR, data, 0, NBR_COLUMNS);

        using (FileStream fileStream = File.Open(_filePath, FileMode.Append, FileAccess.Write, FileShare.Read))
        {
          using (StreamWriter writer = new StreamWriter(fileStream, Encoding.GetEncoding(1250)))
          {
            writer.WriteLine(s);
          }
        }
      }
      else
      {
        Console.WriteLine("SSV Shooter file not found.");
      }
    }
  }
}
