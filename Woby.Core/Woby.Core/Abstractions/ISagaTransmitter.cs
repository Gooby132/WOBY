using FluentResults;
using Woby.Core.CommonLanguage.Messages;

namespace Woby.Core.Abstractions
{
    public interface ISagaTransmitter 
    {
        public Task<Result> SendMessage(MessageBase message);
        public Task<Result> SendRequest(RequestBase request);
        public Task<Result> SendResponse(ResponseBase response);

    }
}
