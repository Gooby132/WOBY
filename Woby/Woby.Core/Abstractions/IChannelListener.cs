using Woby.Core.Network.Core;

namespace Woby.Core.Abstractions
{
    public interface IChannelListener
    {
        public Task ReceiveMessage(Stream stream, NetworkMetadata metadata);
    }
}
