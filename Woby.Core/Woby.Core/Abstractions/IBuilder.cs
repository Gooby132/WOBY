using FluentResults;
using Woby.Core.CommonLanguage.Messages;

namespace Woby.Core.Abstractions
{
    public interface IBuilder
    {
        public Task<Result<Stream>> Build(MessageBase messageBase);
        public Task<Result<Stream>> BuildRinging(MessageBase message);
        public Task<Result<Stream>> BuildTrying(MessageBase message);
        public Task<Result<Stream>> BuildUserAgentWasNotFound(MessageBase message);
    }
}
