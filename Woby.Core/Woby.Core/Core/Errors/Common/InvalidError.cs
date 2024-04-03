namespace Woby.Core.Core.Errors.Common
{
    public class InvalidError : BaseError
    {
        public InvalidError(int groupCode, int errorCode, string message) : base(groupCode, errorCode, message)
        {
        }
    }
}
