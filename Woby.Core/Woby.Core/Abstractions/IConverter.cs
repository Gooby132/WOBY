using FluentResults;
using Woby.Core.CommonLanguage.Messages;

namespace Woby.Core.Abstractions
{
    public interface IConverter<ProtocolMessageBase>
    {

        public Result<MessageBase> Convert(ProtocolMessageBase message);

    }
}
