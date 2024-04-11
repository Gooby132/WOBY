using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woby.Core.Sip.Messages
{
    public class SipHeaderType : SmartEnum<SipHeaderType>
    {

        public static readonly SipHeaderType NotSet = new SipHeaderType("Not Set", 0, "Not Set", "notset");
        public static readonly SipHeaderType From = new SipHeaderType("From", 1, "From", "f");
        public static readonly SipHeaderType To = new SipHeaderType("To", 2,"To", "t");
        public static readonly SipHeaderType Contact = new SipHeaderType("Contact", 3, "Contact", "m");
        public static readonly SipHeaderType AcceptEncoding = new SipHeaderType("AcceptEncoding", 4, "Accept-Encoding", null);
        public static readonly SipHeaderType Accept = new SipHeaderType("Accept", 5, "Accept", null);
        public static readonly SipHeaderType AcceptLanguage = new SipHeaderType("Accept-Language", 6, "Accept-Language", null);
        public static readonly SipHeaderType CallId = new SipHeaderType("CallId", 7, "Call-Id", "i");
        public static readonly SipHeaderType ContentLength = new SipHeaderType("ContentLength", 8, "Content-Length", "l");
        public static readonly SipHeaderType ContentType = new SipHeaderType("ContentType", 9, "Content-Type", "c");

        public string Key { get; }
        public string? CompactKey { get; }

        private SipHeaderType(string name, int value, string key, string? compactKey) : base(name, value)
        {
            Key = key;
            CompactKey = compactKey;
        }

        public static SipHeaderType GetType(string name)
        {
            if (name == nameof(From) || name == "f") return From;
            if (name == nameof(To) || name == "t") return To;
            if (name == nameof(Contact) || name == "m") return Contact;
            if (name == "Call-Id" || name == "i") return CallId;
            if (name == "Content-Length" || name == "l") return CallId;

            return NotSet;
        }

    }
}
