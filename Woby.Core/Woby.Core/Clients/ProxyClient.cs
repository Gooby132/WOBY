using FluentResults;
using Woby.Core.Abstractions;
using Woby.Core.CommonLanguage.Messages;

namespace Woby.Core.Clients
{
    internal class ProxyClient : ClientBase
    {
        public ProxyClient(ISagaTransmitter sagaTransmitter) : base(sagaTransmitter)
        {
        }

        public override Action GetGeneralMessage(MessageBase message) => () => base.SendMessage(message);

        public override Action GetRequest(RequestBase request) => () => base.SendRequest(request);

        public override Action GetResponse(ResponseBase response) => () => base.SendResponse(response);

    }
}
