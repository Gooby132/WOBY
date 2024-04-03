using FluentResults;

namespace Woby.Core.Core.Errors.Common
{
    public abstract class BaseError : Error
    {
        public int GroupCode { get; init; }
        public int ErrorCode { get; init; }
        public string Message { get; init; }

        protected BaseError(int groupCode, int errorCode, string message)
        {
            GroupCode = groupCode;
            ErrorCode = errorCode;
            Message = message;
        }
    }
}
