using System;
using System.IO;

namespace ShootingRange.SiusData
{
  public class SiusDataFileProvider : SiusDataProvider
    {
    private readonly string _filePath;

    public SiusDataFileProvider(string filePath)
    {
      _filePath = filePath;
    }

    public override void Initialize()
    {
      string fullPath = Path.GetFullPath(_filePath);
      if (!File.Exists(fullPath))
        throw new InvalidOperationException(string.Format("File {0} not found.", fullPath));

      using (var reader = new StreamReader(_filePath))
      {
        string line;
        while ((line = reader.ReadLine()) != null)
        {
          try
          {
            ProcessSiusDataMessage(line);
          }
          catch (Exception e)
          {
            LogMessage(string.Format("Error processing message: {0}. Error: {1}", line, e.Message));
          }
        }
      }
    }
    }
}
