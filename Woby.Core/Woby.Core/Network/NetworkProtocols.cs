using Ardalis.SmartEnum;

namespace Woby.Core.Network
{
    public class NetworkProtocols : SmartEnum<NetworkProtocols>
    {
        public static readonly NetworkProtocols Udp = new NetworkProtocols("UDP", 1);
        public static readonly NetworkProtocols Tcp = new NetworkProtocols("TCP", 2);
        public static readonly NetworkProtocols Tls = new NetworkProtocols("TLS", 3);

        private NetworkProtocols(string name, int value) : base(name, value)
        {
        }
    }
}
