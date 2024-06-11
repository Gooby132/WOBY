using FluentResults;
using Woby.Core.CommonLanguage.Messages;

namespace Woby.Core.Abstractions
{
    public interface ISignalingConverter<ProtocolMessageBase>
    {

        public Result<SignalingSection> Convert(ProtocolMessageBase message);

    }
}
