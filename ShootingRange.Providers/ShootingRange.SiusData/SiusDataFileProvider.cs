using System;
using System.IO;

namespace ShootingRange.SiusData
{
  public class SiusDataFileProvider : SiusDataProvider
    {
    private readonly string _filePath;

    public SiusDataFileProvider(string filePath)
    {
      string fullPath = Path.GetFullPath(filePath);
      if (!File.Exists(fullPath))
        throw new ArgumentException(string.Format("File {0} not found.", fullPath), "filePath");
        
      _filePath = fullPath;
      }

    public override void Initialize()
    {
      using (var reader = new StreamReader(_filePath))
      {
        string line;
        while ((line = reader.ReadLine()) != null)
        {
          ProcessSiusDataMessage(line);
        }
      }
    }
    }
}
