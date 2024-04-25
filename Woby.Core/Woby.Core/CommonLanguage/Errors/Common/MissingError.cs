namespace Woby.Core.CommonLanguage.Errors.Common
{
    public class MissingError : BaseError
    {
        public string MissingProperty { get; init; }

        public MissingError(string missingProperty, int groupCode, int errorCode, string message) : base(groupCode, errorCode, message)
        {
            MissingProperty = missingProperty;
        }
    }
}
