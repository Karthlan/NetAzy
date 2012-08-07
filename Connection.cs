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

        private NetworkStream stream;
        public NetworkStream Stream
        {
            get { return stream; }
        }

        private TcpClient client;

        public void Dispose()
        {
            client.Close();
            client = null;
            stream = null;
        }

        public bool Active
        {
            get { return client.Connected; }
        }

        public Connection(TcpClient client)
        {
            this.client = client;
            ip = ((IPEndPoint)client.Client.RemoteEndPoint).Address;
            stream = client.GetStream();
        }
    }
}
