using SnifferDeRede.UseCases.Interface;
using System.Collections.Generic;
using System.Net;

namespace SnifferDeRede.UseCases
{
    public class GetAllLocalIpAddressCase : IGetAllLocalIpAddressCase
    {
        public IList<string> Execute()
        {
            IPHostEntry HosyEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress[] ipAddress = HosyEntry.AddressList;
            if (ipAddress.Length == 0)
                return new string[0];

            IList<string> ips = new List<string>();
            foreach (IPAddress ip in ipAddress)
            {
                string strIP = ip.ToString();
                ips.Add(strIP);
            }

            return ips;
        }
    }
}
