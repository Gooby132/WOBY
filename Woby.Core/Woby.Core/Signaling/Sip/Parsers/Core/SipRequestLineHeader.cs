using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woby.Core.Signaling.Sip.Messages;

namespace Woby.Core.Signaling.Sip.Parsers.Core
{
    public class SipRequestLineHeader
    {
        public SipMethods Method { get; }
        public Uri Uri { get; }
        public string Protocol { get; }

        public SipRequestLineHeader(SipMethods method, Uri uri, string protocol)
        {
            Method = method;
            Uri = uri;
            Protocol = protocol;
        }
    }
}
