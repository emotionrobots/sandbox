using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace SDRobots.Networking
{
    public static class IPCompare
    {
        public static IPAddress GetSubnet(IPAddress ip, IPAddress mask)
        {
            IPAddress subnet = IPAddress.Parse("0.0.0.0");
            subnet.Address = ip.Address & mask.Address;
            return subnet;
        }

        public static bool OnSubnet(IPAddress ip, IPAddress mask, IPAddress subnet)
        {
            return subnet.Address == (ip.Address & mask.Address);
        }
    }
}
