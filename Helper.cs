using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace NetAzy
{
    class Helper
    {
        /// <summary>
        /// Resolves strimg ip or hostname
        /// </summary>
        /// <param name="str_ip">Hostname or IPAddress in string</param>
        /// <returns>IPAddress instance or Exception on failure</returns>
        public static IPAddress ResolveAdress(string str_ip)
        {
            IPAddress ip;
            if (IPAddress.TryParse(str_ip, out ip))
            {
                if (str_ip == "0.0.0.0")
                    return IPAddress.Any;
                return ip;
            }
            else
            {

                IPAddress[] ips = Dns.GetHostAddresses(str_ip);
                return ips[0];
            }
        }
    }
}
