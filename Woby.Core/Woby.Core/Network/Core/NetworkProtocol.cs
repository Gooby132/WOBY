using Ardalis.SmartEnum;

namespace Woby.Core.Network.Core
{
    public class NetworkProtocol : SmartEnum<NetworkProtocol>
    {
        public static readonly NetworkProtocol Unknown = new NetworkProtocol("Unknown", 0, false);
        public static readonly NetworkProtocol Udp = new NetworkProtocol("UDP", 1, false);
        public static readonly NetworkProtocol Tcp = new NetworkProtocol("TCP", 2, false);
        public static readonly NetworkProtocol Tls = new NetworkProtocol("TLS", 3, true);
        public static readonly NetworkProtocol Sctp = new NetworkProtocol("SCTP", 4, true);

        public bool IsSecured { get; }

        private NetworkProtocol(string name, int value, bool isSecured) : base(name, value)
        {
            IsSecured = isSecured;
        }
    }
}
