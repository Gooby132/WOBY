using Woby.Core.CommonLanguage.Errors.Common;

namespace Woby.Core.Signaling.Sip.Parsers.Core
{
    public static class SipCoreErrors
    {
        public const int GroupCode = 1;

        public static MissingError MissingRequestUri() => new MissingError("Request-URI", GroupCode, 1, "Message received has missing 'Request Uri'");
        public static MissingError MissingTo() => new MissingError("To", GroupCode, 2, "Message received has missing 'To'");
        public static MissingError MissingFrom() => new MissingError("From", GroupCode, 3, "Message received has missing 'From'");
        public static MissingError MissingVia() => new MissingError("Via", GroupCode, 4, "Message received has missing 'Via'");
        public static MissingError MissingMaxForward() => new MissingError("Max-Forward", GroupCode, 5, "Message received has missing 'Max-Forward'");
        public static MissingError MissingCallId() => new MissingError("Call-ID", GroupCode, 6, "Message received has missing 'Call-ID'");
        public static MissingError MissingCSeq() => new MissingError("CSeq", GroupCode, 7, "Message received has missing 'CSeq'");
        public static InvalidError CSeqSequenceIsTooLarge() => new InvalidError(GroupCode, 8, "'CSeq' sequence exceeds the limit");
        public static InvalidError CSeqCouldNotBeParsed() => new InvalidError(GroupCode, 9, "'CSeq' could not be parsed");
        public static InvalidError MaxForwardCouldNotBeParsed() => new InvalidError(GroupCode, 10, "'Max-Forward' could not be parsed");
        public static InvalidError GeneralInvalidMessageError(string message) => new InvalidError(GroupCode, 11, $"General invalid error - '{message}'");
        public static InvalidError FailedParsingHeaderParameters() => new InvalidError(GroupCode, 12, $"Failed parsing header parameters");
    }
}
