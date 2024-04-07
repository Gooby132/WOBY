using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woby.Core.Sip.Messages
{
    public class SipHeaderTypes : SmartEnum<SipHeaderTypes>
    {

        public static readonly SipHeaderTypes NotSet = new SipHeaderTypes("Not Set", 0);
        public static readonly SipHeaderTypes From = new SipHeaderTypes("From", 1);
        public static readonly SipHeaderTypes To = new SipHeaderTypes("To", 2);
        public static readonly SipHeaderTypes Via = new SipHeaderTypes("Via", 3);

        private SipHeaderTypes(string name, int value) : base(name, value)
        {
        }

        public static SipHeaderTypes GetType(string name)
        {
            switch (name)
            {
                case nameof(From):
                    return From;
                case nameof(To):
                    return To;
                case nameof(Via):
                    return Via;
                default:
                    return NotSet;
            }
        }

    }
}
