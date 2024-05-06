using FluentResults;
using Woby.Core.Abstractions;
using Woby.Core.CommonLanguage.Messages;

namespace Woby.Core.Clients
{
    public abstract class ClientBase : IClient
    {

        private readonly ISagaTransmitter _sagaTransmitter;

        public ClientBase(ISagaTransmitter sagaTransmitter)
        {
            _sagaTransmitter = sagaTransmitter;
        }

        public abstract Action GetGeneralMessage(MessageBase message);

        public abstract Action GetRequest(RequestBase request);

        public abstract Action GetResponse(ResponseBase response);

        public virtual Task<Result> SendMessage(MessageBase message) => _sagaTransmitter.SendMessage(message);

        public virtual Task<Result> SendRequest(RequestBase request) => _sagaTransmitter.SendRequest(request);

        public virtual Task<Result> SendResponse(ResponseBase response) => _sagaTransmitter.SendResponse(response);
    }
}
