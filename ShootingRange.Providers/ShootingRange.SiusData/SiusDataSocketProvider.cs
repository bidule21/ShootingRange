using System;
using ShootingRange.Common;
using SiusDataReader;

namespace ShootingRange.SiusData
{
  public class SiusDataSocketProvider : SiusDataProvider, IShootingRange
  {
    private readonly string _address;
    private readonly int _port;
    private SiusTcpDataReader dataReader;

    public SiusDataSocketProvider(string address, int port)
    {
      _address = address;
      _port = port;
    }

    public override void Initialize()
    {
      if (dataReader != null)
      {
        dataReader.Dispose();
        dataReader = null;
      }
      dataReader = new SiusTcpDataReader(_address, _port);
      dataReader.SiusDataReceived += DataReaderOnSiusDataReceived;
      dataReader.Connect();
    }

    private void DataReaderOnSiusDataReceived(object sender, SiusDataReceivedEventArgs siusDataReceivedEventArgs)
    {
      string[] split = siusDataReceivedEventArgs.Message.Split(new string[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
      foreach (string s in split)
      {
        try
        {
          ProcessSiusDataMessage(s);        
        }
        catch (Exception e)
        {
          LogMessage(string.Format("Error processing message: {0}. Error: {1}", s, e.Message));
        }
      }
    }
  }
}
