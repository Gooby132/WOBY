using FluentResults;
using Woby.Core.CommonLanguage.Messages;

namespace Woby.Core.Sagas.Clients
{
    internal class SagaProxyClient : SagaClientBase
    {
        public SagaProxyClient()
        {
        }

        public override Func<RequestBase, Task<Result>> GetRequest() => SendMessage;

        public override Func<ResponseBase, Task<Result>> GetResponse() => SendMessage;
    }
}
