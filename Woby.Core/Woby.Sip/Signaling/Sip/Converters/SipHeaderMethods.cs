using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woby.Sip.Signaling.Sip.Converters
{
    public class SipHeaderMethods : SmartEnum<SipHeaderMethods>
    {

        public static readonly SipHeaderMethods NotSet = new SipHeaderMethods("Not Set", 0, "Not Set", "notset");
        public static readonly SipHeaderMethods From = new SipHeaderMethods("From", 1, "From", "f");
        public static readonly SipHeaderMethods To = new SipHeaderMethods("To", 2, "To", "t");
        public static readonly SipHeaderMethods Contact = new SipHeaderMethods("Contact", 3, "Contact", "m");
        public static readonly SipHeaderMethods AcceptEncoding = new SipHeaderMethods("AcceptEncoding", 4, "Accept-Encoding", null);
        public static readonly SipHeaderMethods Accept = new SipHeaderMethods("Accept", 5, "Accept", null);
        public static readonly SipHeaderMethods AcceptLanguage = new SipHeaderMethods("Accept-Language", 6, "Accept-Language", null);
        public static readonly SipHeaderMethods CallId = new SipHeaderMethods("CallId", 7, "Call-ID", "i");
        public static readonly SipHeaderMethods ContentLength = new SipHeaderMethods("ContentLength", 8, "Content-Length", "l");
        public static readonly SipHeaderMethods ContentType = new SipHeaderMethods("ContentType", 9, "Content-Type", "c");
        public static readonly SipHeaderMethods CSeq = new SipHeaderMethods("CSeq", 10, "CSeq", null);
        public static readonly SipHeaderMethods MaxForwards = new SipHeaderMethods("MaxForwards", 11, "Max-Forwards", null);
        public static readonly SipHeaderMethods Via = new SipHeaderMethods("Via", 12, "Via", "v");

        public string Key { get; }
        public string? CompactKey { get; }

        private SipHeaderMethods(string name, int value, string key, string? compactKey) : base(name, value)
        {
            Key = key;
            CompactKey = compactKey;
        }

        public static SipHeaderMethods GetType(string name)
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
