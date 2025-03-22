using Woby.Core.Commons.Errors;

namespace Woby.Core.Signaling.Errors
{
    public class UnrecognizedResponseError : ErrorBase
    {
        public UnrecognizedResponseError(int groupCode, int errorCode, string message) : base(groupCode, errorCode, message)
        {
        }
    }
}
