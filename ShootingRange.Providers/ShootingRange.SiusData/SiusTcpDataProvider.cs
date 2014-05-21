using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SiusDataReader
{
  public class SiusTcpDataReader
  {
    private readonly string _host;
    private readonly int _port;
    private readonly AsyncOperation _asyncOperation;

    private readonly ManualResetEvent _connectDone = new ManualResetEvent(false);

    Socket _socket;

    public event EventHandler<SiusDataReceivedEventArgs> SiusDataReceived;

    public SiusTcpDataReader(string host, int port)
    {
      _asyncOperation = AsyncOperationManager.CreateOperation(null);
      _host = host;
      _port = port;
    }

    public void Connect()
    {
      IPAddress ipAddress = IPAddress.Parse(_host);
      IPEndPoint endPoint = new IPEndPoint(ipAddress, _port);
      _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      _socket.BeginConnect(endPoint, new AsyncCallback(ConnectCallback), _socket);
      _connectDone.WaitOne(5000);

      BeginReceive();
    }

    public void Dispose()
    {
      _socket.Shutdown(SocketShutdown.Both);
      _socket.Close();
      _socket.Dispose();
    }

    private void ConnectCallback(IAsyncResult ar)
    {
      try
      {
        Socket socket = (Socket)ar.AsyncState;
        socket.EndConnect(ar);
        Debug.WriteLine(string.Format("Socket connected to {0}", socket.RemoteEndPoint.ToString()));

        _connectDone.Set();
      }
      catch (Exception e)
      {
        Console.WriteLine(e.ToString());
      }
    }

    private void BeginReceive()
    {
      StateObject state = new StateObject();
      state.WorkSocket = _socket;
      _socket.BeginReceive(state.Buffer, 0, StateObject.BUFFER_SIZE, SocketFlags.None, ReceiveCallback, state);
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
      StateObject state = (StateObject)ar.AsyncState;
      Socket client = state.WorkSocket;
      int bytesRead = client.EndReceive(ar);
      if (bytesRead > 0)
      {
        string receivedData = Encoding.Default.GetString(state.Buffer, 0, bytesRead);
        string data = state.BufferResidue + receivedData;
        int lastLinebreak = data.LastIndexOf("\r\n", System.StringComparison.Ordinal);
        int length = data.Length;
        string completeMessages = data;
        if (lastLinebreak == -1 || lastLinebreak != length - 2)
        {
          int startIdx = lastLinebreak == -1 ? lastLinebreak + 1 : lastLinebreak + 2;
          int cnt = length - startIdx;
          state.BufferResidue = data.Substring(startIdx, cnt);
          completeMessages = data.Remove(startIdx, cnt);
        }
        else
        {
          state.BufferResidue = string.Empty;
        }

        if (!string.IsNullOrEmpty(completeMessages))
          OnSiusDataReceived(new SiusDataReceivedEventArgs(completeMessages));
        client.BeginReceive(state.Buffer, 0, StateObject.BUFFER_SIZE, SocketFlags.None, ReceiveCallback, state);
      }
    }

    protected virtual void OnSiusDataReceived(SiusDataReceivedEventArgs e)
    {
      EventHandler<SiusDataReceivedEventArgs> handler = SiusDataReceived;
      if (handler != null) _asyncOperation.Post(delegate { handler(this, e); }, null);
    }
  }
}
