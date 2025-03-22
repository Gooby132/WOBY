using FluentResults;
using Woby.Core.CommonLanguage.Messages;

namespace Woby.Core.Abstractions
{
    public interface ISagaTransmitter 
    {
        public Task<Result> SendMessage(MessageBase message);
    }
}
