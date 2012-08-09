using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace NetAzy
{
    public class Connection
    {
        private IPAddress ip;
        private Socket socket;
        private byte[] receiveBuffer;

        public IncomingNetMessage HalfMessage;

        public void Dispose()
        {
            socket.Close();
            socket = null;
        }

        public bool Active
        {
            get { return socket.Connected; }
        }

        public int ReceiveBufferSize
        {
            get { return socket.ReceiveBufferSize; }
        }


        public byte[] ReceiveBuffer
        {
            get { return receiveBuffer; }
        }

        public Socket Socket
        {
            get { return socket; }
        }

        public IPAddress IP
        {
            get { return ip; }
        }

        public Connection(Socket socket)
        {
            this.socket = socket;
            ip = ((IPEndPoint)socket.RemoteEndPoint).Address;
            receiveBuffer = new byte[ReceiveBufferSize];
        }
    }
}
