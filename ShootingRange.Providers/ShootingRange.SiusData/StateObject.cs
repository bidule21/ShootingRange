using System.Net.Sockets;

namespace SiusDataReader
{
  public class StateObject
  {
    // Client socket.
    public Socket WorkSocket = null;
    // Size of receive buffer.
    public const int BUFFER_SIZE = 256;
    // Receive buffer.
    public byte[] Buffer = new byte[BUFFER_SIZE];
    public string BufferResidue = string.Empty;
  }
}