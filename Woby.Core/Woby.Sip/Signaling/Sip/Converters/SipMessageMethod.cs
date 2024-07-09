using Ardalis.SmartEnum;
using Woby.Core.CommonLanguage.Signaling.Roles;

namespace Woby.Sip.Signaling.Sip.Converters
{
    public class SipMessageMethod : SmartEnum<SipMessageMethod>
    {
        public static readonly SipMessageMethod None = new SipMessageMethod("None", 0);
        public static readonly SipMessageMethod Invite = new SipMessageMethod("INVITE", 1);
        public static readonly SipMessageMethod Bye = new SipMessageMethod("BYE", 2);
        public static readonly SipMessageMethod Ok = new SipMessageMethod("OK", 3);
        public static readonly SipMessageMethod NotFound = new SipMessageMethod("Not Found", 404);
        public static readonly SipMessageMethod Trying = new SipMessageMethod("Trying", 100);
        public static readonly SipMessageMethod Ringing = new SipMessageMethod("Ringing", 180);

        private SipMessageMethod(string name, int value) : base(name, value)
        {
        }

        public static SipMessageMethod FromString(string value)
        {
            if (value == "INVITE") return Invite;
            if (value == "BYE") return Bye;

            return None;
        }

        public override string ToString() => $"{NotFound.Value} {NotFound.Name}";
    }
}
