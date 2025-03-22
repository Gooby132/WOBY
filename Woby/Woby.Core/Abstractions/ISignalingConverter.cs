using FluentResults;
using Woby.Core.CommonLanguage.Messages;
using Woby.Core.Network.Core;

namespace Woby.Core.Abstractions
{
    public interface ISignalingConverter<ProtocolMessageBase>
    {

        public Result<SignalingSection> Convert(ProtocolMessageBase message, NetworkMetadata metadata);

    }
}
