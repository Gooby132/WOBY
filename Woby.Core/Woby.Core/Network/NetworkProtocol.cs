using Ardalis.SmartEnum;

namespace Woby.Core.Network
{
    public class NetworkProtocol : SmartEnum<NetworkProtocol>
    {
        public static readonly NetworkProtocol Udp = new NetworkProtocol("UDP", 1, 3);
        public static readonly NetworkProtocol Tcp = new NetworkProtocol("TCP", 2, 3);
        public static readonly NetworkProtocol Tls = new NetworkProtocol("TLS", 3, 3);
        public static readonly NetworkProtocol Sctp = new NetworkProtocol("SCTP", 4, 4);

        public int CharacterLength { get; }

        private NetworkProtocol(string name, int value, int characterLength) : base(name, value)
        {
            CharacterLength = characterLength;
        }
    }
}
