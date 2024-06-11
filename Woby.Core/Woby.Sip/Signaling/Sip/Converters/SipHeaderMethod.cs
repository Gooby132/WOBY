using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woby.Sip.Signaling.Sip.Converters
{
    public class SipHeaderMethod : SmartEnum<SipHeaderMethod>
    {

        public static readonly SipHeaderMethod NotSet = new SipHeaderMethod("Not Set", 0, "Not Set", "notset");
        public static readonly SipHeaderMethod From = new SipHeaderMethod("From", 1, "From", "f");
        public static readonly SipHeaderMethod To = new SipHeaderMethod("To", 2, "To", "t");
        public static readonly SipHeaderMethod Contact = new SipHeaderMethod("Contact", 3, "Contact", "m");
        public static readonly SipHeaderMethod AcceptEncoding = new SipHeaderMethod("AcceptEncoding", 4, "Accept-Encoding", null);
        public static readonly SipHeaderMethod Accept = new SipHeaderMethod("Accept", 5, "Accept", null);
        public static readonly SipHeaderMethod AcceptLanguage = new SipHeaderMethod("Accept-Language", 6, "Accept-Language", null);
        public static readonly SipHeaderMethod CallId = new SipHeaderMethod("CallId", 7, "Call-ID", "i");
        public static readonly SipHeaderMethod ContentLength = new SipHeaderMethod("ContentLength", 8, "Content-Length", "l");
        public static readonly SipHeaderMethod ContentType = new SipHeaderMethod("ContentType", 9, "Content-Type", "c");
        public static readonly SipHeaderMethod CSeq = new SipHeaderMethod("CSeq", 10, "CSeq", null);
        public static readonly SipHeaderMethod MaxForwards = new SipHeaderMethod("MaxForwards", 11, "Max-Forwards", null);
        public static readonly SipHeaderMethod Via = new SipHeaderMethod("Via", 12, "Via", "v");

        public string Key { get; }
        public string? CompactKey { get; }

        private SipHeaderMethod(string name, int value, string key, string? compactKey) : base(name, value)
        {
            Key = key;
            CompactKey = compactKey;
        }

        public static SipHeaderMethod GetType(string name)
        {
            if (name == nameof(From) || name == "f") return From;
            if (name == nameof(To) || name == "t") return To;
            if (name == nameof(Contact) || name == "m") return Contact;
            if (name == "Call-ID" || name == "i") return CallId;
            if (name == "Content-Length" || name == "l") return ContentLength;
            if (name == "Content-Type" || name == "l") return ContentType;
            if (name == nameof(CSeq)) return CSeq;
            if (name == "Max-Forwards") return MaxForwards;
            if (name == "Via" || name == "v") return Via;

            return NotSet;
        }

    }
}
