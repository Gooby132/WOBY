using Ardalis.SmartEnum;

namespace Woby.Core.Core.Headers
{
    public class RouteIntake : SmartEnum<RouteIntake>
    {
        /// <summary>
        /// Indicates the user address the message was intended to
        /// </summary>
        public static readonly RouteIntake To = new RouteIntake("To", 1);

        /// <summary>
        /// Indicates the user that sent the message
        /// </summary>
        public static readonly RouteIntake From = new RouteIntake("From", 2);

        /// <summary>
        /// Indicates the which proxies the message was passed by
        /// </summary>
        public static readonly RouteIntake Proxy = new RouteIntake("Via", 3);

        private RouteIntake(string name, int value) : base(name, value)
        {
        }
    }
}
