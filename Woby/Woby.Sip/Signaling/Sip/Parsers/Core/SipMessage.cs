using FluentResults;
using Woby.Core.Signaling.Sip.Headers;

namespace Woby.Core.Signaling.Sip.Parsers.Core
{
    public class SipMessage
    {

        public SipRequestLineHeader RequestLineHeader { get; }
        public IEnumerable<SipHeader> Headers { get; }
        public IEnumerable<Result<SipHeader>> FailedHeaders { get; }

        public SipMessage(
            SipRequestLineHeader requestHeader, 
            IEnumerable<SipHeader> headers, 
            IEnumerable<Result<SipHeader>> failedHeaders)
        {
            RequestLineHeader = requestHeader;
            Headers = headers;
            FailedHeaders = failedHeaders;
        }
    }
}
