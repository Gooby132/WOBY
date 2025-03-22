using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woby.Sip.Signaling.Sip.Converters;

namespace Woby.Core.Signaling.Sip.Parsers.Core
{
    public class SipRequestLineHeader
    {
        public SipMessageMethod Method { get; }
        public Uri Uri { get; }
        public string Protocol { get; }

        public SipRequestLineHeader(SipMessageMethod method, Uri uri, string protocol)
        {
            Method = method;
            Uri = uri;
            Protocol = protocol;
        }
    }
}
