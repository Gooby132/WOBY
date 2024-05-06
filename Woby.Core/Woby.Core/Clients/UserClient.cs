using FluentResults;
using Woby.Core.Abstractions;
using Woby.Core.CommonLanguage.Messages;

namespace Woby.Core.Clients
{
    internal class UserClient : ClientBase
    {
        public UserClient(ISagaTransmitter sagaTransmitter) : base(sagaTransmitter)
        {
        }

        public override Action GetGeneralMessage(MessageBase message)
        {
            throw new NotImplementedException();
        }

        public override Action GetRequest(RequestBase request)
        {
            throw new NotImplementedException();
        }

        public override Action GetResponse(ResponseBase response)
        {
            throw new NotImplementedException();
        }
    }
}
