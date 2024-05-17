using Woby.Core.Commons.Errors;

namespace Woby.Core.Network.Errors
{
    public class MisroutedError : ErrorBase
    {
        public MisroutedError(int groupCode, int errorCode, string message) : base(groupCode, errorCode, message)
        {
        }
    }
}
