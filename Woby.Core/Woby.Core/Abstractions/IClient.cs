using FluentResults;
using Woby.Core.CommonLanguage.Messages;

namespace Woby.Core.Abstractions
{
    public interface IClient
    {
        public Action GetGeneralMessage(MessageBase message);
        public Action GetResponse(ResponseBase response);
        public Action GetRequest(RequestBase request);

        //public Task<Result<MessageBase?>> SendMessage(MessageBase request);
        //public Task<Result<MessageBase?>> SendRequest(RequestBase request);
        //public Task<Result<MessageBase?>> SendResponse(ResponseBase response);
    }
}
