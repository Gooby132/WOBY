using FluentResults;

namespace Woby.Core.Abstractions
{
    public interface IParser<ProtocolMessageBase>
    {
        public Result<ProtocolMessageBase> Parse(string message);
    }
}
