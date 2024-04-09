using Woby.Core.Core.Headers;

namespace Woby.Core.Sip.Parsers.Headers
{
    public class SipHeader : HeaderBase
    {

        public IEnumerable<SipParameter>? Parameters { get; } // Additional parameters for appended to the sip header

        public SipHeader(string name, string value, HeaderTypes type, IEnumerable<SipParameter>? parameters) : base(name, value, type) 
        {
            Parameters = parameters;
        }
    }
}
