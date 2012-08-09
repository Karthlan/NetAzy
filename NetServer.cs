using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace NetAzy
{
    public class NetServer
    {
        protected List<Connection> connections;

        public List<Connection> Connections
        {
            get { return connections; }
        }

        protected Socket listener;

        public NetServer()
        {
            this.connections = new List<Connection>();
        }

        public delegate void OnMessageReceivedDel(IncomingNetMessage msg, Connection con);
        public OnMessageReceivedDel OnMessageReceived;

        public delegate void OnConnectionLostDel(Connection con);
        public OnConnectionLostDel OnConnectionLost;

        public void SendMessage(OutgoingNetMessage msg)
        {
            foreach(Connection con in connections)
                con.Socket.BeginSend(msg.Bytes, 0, msg.Size, 0, new AsyncCallback(send), con);
        }

        public void SendMessage(OutgoingNetMessage msg, Connection con)
        {
            con.Socket.BeginSend(msg.Bytes, 0, msg.Size, 0, new AsyncCallback(send), con);
        }

        private void send(IAsyncResult ar)
        {
            Connection con = (Connection)ar.AsyncState;

            int bytesSent = con.Socket.EndSend(ar);
            Console.WriteLine(bytesSent);
        }

        private void readMessage(IAsyncResult ar)
        {
            try
            {
                Connection con = (Connection)ar.AsyncState;
                Socket handler = con.Socket;

                // Read data from the client socket.
                int read = handler.EndReceive(ar);

                // Data was read from the client socket.
                if (read > 0)
                {
                    byte[] bytes = new byte[read];
                    for (int i = 0; i < read; i++)
                    {
                        bytes[i] = con.ReceiveBuffer[i];
                    }
                    IncomingNetMessage msg = new IncomingNetMessage(bytes);
                    if (OnMessageReceived != null)
                        OnMessageReceived(msg, con);
                    handler.BeginReceive(con.ReceiveBuffer, 0, con.ReceiveBufferSize, 0,
                        new AsyncCallback(readMessage), con);
                }
            }
            catch (SocketException e)
            {
                if (e.ErrorCode == 10054)
                {
                    Connection con = (Connection)ar.AsyncState;
                    Console.WriteLine("Client disconnected: " + con.IP);
                    if (OnConnectionLost != null)
                        OnConnectionLost(con);
                    connections.Remove(con);
                    con.Dispose();
                }
                else
                {
                    throw new Exception("Unkown Error: " + e);
                }
            }
        }

        private void acceptConnection(IAsyncResult result)
        {
            Socket s = listener.EndAccept(result);

            Connection con = new Connection(s);

            this.connections.Add(con);

            s.BeginReceive(con.ReceiveBuffer, 0, con.ReceiveBufferSize, 0, new AsyncCallback(readMessage), con);
            listener.BeginAccept(acceptConnection, null);
        }

        public void Start(string str_ip, int port)
        {
            IPAddress ip = Helper.ResolveAdress(str_ip);
            this.listener = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint localEP = new IPEndPoint(ip, port);
            listener.Bind(localEP);
            listener.Listen(10);
            this.listener.BeginAccept(acceptConnection, null);
        }
    }
}