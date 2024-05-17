using Woby.Core.Commons.Errors;

namespace Woby.Core.Signaling.Errors
{
    public class UnsupportedHeaderError : ErrorBase
    {
        public UnsupportedHeaderError(int groupCode, int errorCode, string message) : base(groupCode, errorCode, message)
        {
        }
    }
}
