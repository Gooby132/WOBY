namespace Woby.Core.Commons.Errors;

public class MissingPropertyError : ErrorBase
{
    public string MissingProperty { get; init; }

    public MissingPropertyError(string missingProperty, int groupCode, int errorCode, string message) : base(groupCode, errorCode, message)
    {
        MissingProperty = missingProperty;
    }
}
