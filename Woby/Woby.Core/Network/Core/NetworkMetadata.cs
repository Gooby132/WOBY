using System.Net;

namespace Woby.Core.Network.Core;

public class NetworkMetadata
{
    public required IPEndPoint ReceviedOn { get; init; }
    public required NetworkProtocol NetworkProtocol { get; init; }
}
