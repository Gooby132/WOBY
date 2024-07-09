using Ardalis.SmartEnum;

namespace Woby.Core.CommonLanguage.Signaling.Routings
{
    public class RouteRole : SmartEnum<RouteRole>
    {

        public static readonly RouteRole NotSet = new RouteRole("not set", 0);

        /// <summary>
        /// Sender of the message. e.g 'From' in SIP protocol
        /// </summary>
        public static readonly RouteRole Sender = new RouteRole("sender", 1);

        /// <summary>
        /// Recipient of the message. e.g 'To' in SIP protocol
        /// </summary>
        public static readonly RouteRole Recipient = new RouteRole("recipient", 2);

        public static readonly RouteRole Monitoring = new RouteRole("recipient", 4);

        private RouteRole(string name, int value) : base(name, value)
        {
        }
    }
}
