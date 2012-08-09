using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace NetAzy
{
    public class NetClient
    {
        private Socket socket;
        private byte[] receiveBuffer;

        public delegate void OnMessageReceivedDel(IncomingNetMessage msg);
        public OnMessageReceivedDel OnMessageReceived;

        public bool Connect(string str_ip, int port)
        {
            IPAddress ip = Helper.ResolveAdress(str_ip);
            socket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            socket.Connect(ip, port);
            receiveBuffer = new byte[socket.ReceiveBufferSize];

            socket.BeginReceive(receiveBuffer, 0, socket.ReceiveBufferSize, 0, new AsyncCallback(readMessage), null);

            return true;
        }

        private void readMessage(IAsyncResult ar)
        {
            try
            {
                // Read data from the client socket.
                int read = socket.EndReceive(ar);

                // Data was read from the client socket.
                if (read > 0)
                {
                    byte[] bytes = new byte[read];
                    for (int i = 0; i < read; i++)
                    {
                        bytes[i] = receiveBuffer[i];
                    }
                    IncomingNetMessage msg = new IncomingNetMessage(bytes);
                    if (OnMessageReceived != null)
                        OnMessageReceived(msg);
                    socket.BeginReceive(receiveBuffer, 0, socket.ReceiveBufferSize, 0,
                        new AsyncCallback(readMessage), null);
                }
            }
            catch (SocketException e)
            {
                if (e.ErrorCode == 10054)
                {
                    Console.WriteLine("Server shutdown");
                }
                else
                {
                    throw new Exception("Unkown Error: " + e);
                }
            }
        }

        public bool SendMessage(OutgoingNetMessage msg)
        {
            try
            {
                socket.BeginSend(msg.Bytes, 0, msg.Size, 0, new AsyncCallback(messageSent), null);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        private void messageSent(IAsyncResult ar)
        {
            socket.EndSend(ar);
        }
    }
}
