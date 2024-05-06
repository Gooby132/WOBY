using FluentResults;
using Woby.Core.CommonLanguage.Messages;

namespace Woby.Core.Abstractions
{
    public interface IBuilder
    {

        public Task<Result<Stream>> Build(MessageBase messageBase);

    }
}
