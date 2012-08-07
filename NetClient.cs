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
        private TcpClient tcpClient;
        private NetworkStream stream;

        public NetClient()
        {
            tcpClient = new TcpClient();
        }

        public bool Connect(string str_ip, int port)
        {
            IPAddress ip = Helper.ResolveAdress(str_ip);
            IPEndPoint serverEndPoint = new IPEndPoint(ip, port);

            tcpClient.Connect(serverEndPoint);

            stream = tcpClient.GetStream();

            return true;
        }

        public bool SendMessage(SndNetMessage msg)
        {
            try
            {
                stream.Write(msg.Bytes, 0, msg.Lenght);
                stream.Flush();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
