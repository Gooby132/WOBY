using FluentResults;

namespace Woby.Core.Commons.Errors
{
    public interface IParser<ProtocolMessageBase>
    {
        public Result<ProtocolMessageBase> Parse(string message);
    }
}
