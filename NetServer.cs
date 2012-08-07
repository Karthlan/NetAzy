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
        private List<Connection> Connections;

        protected TcpListener tcpListener;

        protected Thread thread;

        public NetServer()
        {
            this.Connections = new List<Connection>();
        }

        private void HandleClientComm()
        {
            while(true)
            foreach (Connection con in Connections)
            {
                byte[] message = new byte[4096];
                if (con.Active)
                {
                    int bytesRead = 0;
                    try
                    {
                        bytesRead = con.Stream.Read(message, 0, 4096);
                    }
                    catch
                    {
                        break;
                    }
                    if (bytesRead != 0)
                    {
                        RcvNetMessage msg = new RcvNetMessage(message, bytesRead);
                        Console.WriteLine(msg.ReadInt32());
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    con.Dispose();
                    Connections.Remove(con);
                }
            }
        }

        private void ListenForClients()
        {
            this.tcpListener.Start();
            Thread clientThread = new Thread(HandleClientComm);
            clientThread.Start();
            while (true)
            {
                Connection connection = new Connection(this.tcpListener.AcceptTcpClient());
                Connections.Add(connection);
                Thread.Sleep(1);
            }
        }

        public void Start(string str_ip, int port)
        {
            IPAddress ip = Helper.ResolveAdress(str_ip);
            this.tcpListener = new TcpListener(ip, port);
            this.thread = new Thread(ListenForClients);
            this.thread.Start();
        }
    }
}