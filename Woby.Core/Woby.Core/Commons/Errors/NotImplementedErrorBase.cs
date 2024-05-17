namespace Woby.Core.Commons.Errors
{
    public class NotImplementedErrorBase : ErrorBase
    {

        public const int GroupErrorCode = 3;

        public NotImplementedErrorBase(int errorCode, string message) : base(GroupErrorCode, errorCode, message)
        {
        }
    }
}
