namespace Woby.Core.Commons.Errors
{
    public class NotFoundErrorBase : ErrorBase
    {
        public NotFoundErrorBase(int groupCode, int errorCode, string message) : base(groupCode, errorCode, message)
        {
        }
    }
}
