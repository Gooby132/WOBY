using Woby.Core.Commons.Errors;

namespace Woby.Core.Signaling.Sip.Parsers.Core
{
    public static class SipCoreErrors
    {
        public const int GroupCode = 1;

        public static MissingPropertyError MissingRequestUri() => new MissingPropertyError("Request-URI", GroupCode, 1, "Message received has missing 'Request Uri'");
        public static MissingPropertyError MissingTo() => new MissingPropertyError("To", GroupCode, 2, "Message received has missing 'To'");
        public static MissingPropertyError MissingFrom() => new MissingPropertyError("From", GroupCode, 3, "Message received has missing 'From'");
        public static MissingPropertyError MissingVia() => new MissingPropertyError("Via", GroupCode, 4, "Message received has missing 'Via'");
        public static MissingPropertyError MissingMaxForward() => new MissingPropertyError("Max-Forward", GroupCode, 5, "Message received has missing 'Max-Forward'");
        public static MissingPropertyError MissingCallId() => new MissingPropertyError("Call-ID", GroupCode, 6, "Message received has missing 'Call-ID'");
        public static MissingPropertyError MissingCSeq() => new MissingPropertyError("CSeq", GroupCode, 7, "Message received has missing 'CSeq'");
        public static MissingPropertyError MissingContentType() => new MissingPropertyError("ContentType", GroupCode, 8, "Message received has missing 'Content-Type'");
        public static MissingPropertyError MissingContentLength() => new MissingPropertyError("ContenLength", GroupCode, 9, "Message received has missing 'Content-Length'");
        public static MissingPropertyError MissingRequestLine() => new MissingPropertyError("Request-Line", GroupCode, 10, "Message received has missing 'Request Line'");

        public static InvalidError CSeqSequenceIsTooLarge() => new InvalidError(GroupCode, 8, "'CSeq' sequence exceeds the limit");
        public static InvalidError CSeqCouldNotBeParsed() => new InvalidError(GroupCode, 9, "'CSeq' could not be parsed");
        public static InvalidError MaxForwardCouldNotBeParsed() => new InvalidError(GroupCode, 10, "'Max-Forward' could not be parsed");
        public static InvalidError GeneralInvalidMessageError(string message) => new InvalidError(GroupCode, 11, $"General invalid error - '{message}'");
        public static InvalidError FailedParsingHeaderParameters() => new InvalidError(GroupCode, 12, $"Failed parsing header parameters");
        public static InvalidError FailedParsingRequestHeader() => new InvalidError(GroupCode, 13, $"Failed parsing request header");
        public static InvalidError FailedGeneratingDialogId() => new InvalidError(GroupCode, 14, $"Failed generating dialog id");
    }
}
