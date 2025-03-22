namespace Woby.Core.Commons.Errors;

public class InvalidError : ErrorBase
{
    public InvalidError(int groupCode, int errorCode, string message) : base(groupCode, errorCode, message)
    {
    }
}
