using FluentResults;
using Woby.Core.CommonLanguage.Signaling.Core;

namespace Woby.Core.Signaling.Core
{
    public interface ISignalingConverter
    {
        public Result<SignalingHeader> Parse();
    }
}
