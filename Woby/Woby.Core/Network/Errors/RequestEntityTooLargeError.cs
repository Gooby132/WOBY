using Woby.Core.Commons.Errors;

namespace Woby.Core.Network.Errors
{
    public class RequestEntityTooLargeError : ErrorBase
    {
        public RequestEntityTooLargeError(int groupCode, int errorCode, string message) : base(groupCode, errorCode, message)
        {
        }
    }
}
