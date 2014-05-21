using System;

namespace SiusDataReader
{
  public class SiusDataReceivedEventArgs : EventArgs
  {
    public SiusDataReceivedEventArgs(string message)
    {
      Message = message;
    }

    public string Message { get; private set; }
  }
}