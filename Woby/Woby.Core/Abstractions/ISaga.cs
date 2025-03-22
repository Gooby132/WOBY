using FluentResults;

namespace Woby.Core.Abstractions;

public interface ISaga<InputMessage> : IChannelListener
{
    public Result Start();
}
