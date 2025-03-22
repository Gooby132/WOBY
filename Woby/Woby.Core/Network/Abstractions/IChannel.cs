using FluentResults;
using Woby.Core.Abstractions;

namespace Woby.Core.Network.Abstractions
{
    public interface IChannel
    {
        Task<Result> Transmit(Stream listenerer);

        Result Subscribe(IChannelListener listenerer);
        Result Unsubscribe(IChannelListener defaultSaga);
    }
}
