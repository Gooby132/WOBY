using FluentResults;

namespace Woby.Core.CommonLanguage.Errors.Common
{
    public abstract class BaseError : Error
    {
        public int GroupCode { get; init; }
        public int ErrorCode { get; init; }

        protected BaseError(int groupCode, int errorCode, string message) : base(message)
        {
            GroupCode = groupCode;
            ErrorCode = errorCode;
        }
    }
}
