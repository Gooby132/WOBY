using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woby.Core.Signaling.Sip.Messages
{
    public class SipMethods : SmartEnum<SipMethods>
    {

        public static readonly SipMethods NotSet = new SipMethods("Not Set", 0, "Not Set", "notset");
        public static readonly SipMethods From = new SipMethods("From", 1, "From", "f");
        public static readonly SipMethods To = new SipMethods("To", 2, "To", "t");
        public static readonly SipMethods Contact = new SipMethods("Contact", 3, "Contact", "m");
        public static readonly SipMethods AcceptEncoding = new SipMethods("AcceptEncoding", 4, "Accept-Encoding", null);
        public static readonly SipMethods Accept = new SipMethods("Accept", 5, "Accept", null);
        public static readonly SipMethods AcceptLanguage = new SipMethods("Accept-Language", 6, "Accept-Language", null);
        public static readonly SipMethods CallId = new SipMethods("CallId", 7, "Call-ID", "i");
        public static readonly SipMethods ContentLength = new SipMethods("ContentLength", 8, "Content-Length", "l");
        public static readonly SipMethods ContentType = new SipMethods("ContentType", 9, "Content-Type", "c");
        public static readonly SipMethods CSeq = new SipMethods("CSeq", 10, "CSeq", null);
        public static readonly SipMethods MaxForwards = new SipMethods("MaxForwards", 11, "Max-Forwards", null);
        public static readonly SipMethods Via = new SipMethods("Via", 12, "Via", "v");

        public string Key { get; }
        public string? CompactKey { get; }

        private SipMethods(string name, int value, string key, string? compactKey) : base(name, value)
        {
            Key = key;
            CompactKey = compactKey;
        }

        public static SipMethods GetType(string name)
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
