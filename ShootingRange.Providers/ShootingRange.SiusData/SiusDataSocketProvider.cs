using System;
using ShootingRange.Common;
using SiusDataReader;

namespace ShootingRange.SiusData
{
  public class SiusDataSocketProvider : SiusDataProvider, IShootingRange
  {
    private SiusTcpDataReader dataReader;

    public SiusDataSocketProvider()
    {
      dataReader = new SiusTcpDataReader("127.0.0.1", 4000);
    }

    public override void Initialize()
    {
      dataReader.SiusDataReceived += DataReaderOnSiusDataReceived;
      dataReader.Connect();
    }

    private void DataReaderOnSiusDataReceived(object sender, SiusDataReceivedEventArgs siusDataReceivedEventArgs)
    {
      string[] split = siusDataReceivedEventArgs.Message.Split(new string[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
      foreach (string s in split)
      {
        ProcessSiusDataMessage(s);        
      }
    }
  }
}
