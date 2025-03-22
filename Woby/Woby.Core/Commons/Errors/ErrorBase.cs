using FluentResults;

namespace Woby.Core.Commons.Errors
{
    public class ErrorBase : Error
    {
        public int GroupCode { get; }
        public int ErrorCode { get; }

        public ErrorBase(int groupCode, int errorCode, string message) : base(message)
        {
            GroupCode = groupCode;
            ErrorCode = errorCode;
        }
    }
}
