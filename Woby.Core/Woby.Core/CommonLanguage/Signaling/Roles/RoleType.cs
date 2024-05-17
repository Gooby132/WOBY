using Ardalis.SmartEnum;

namespace Woby.Core.CommonLanguage.Signaling.Roles
{
    public class RoleType : SmartEnum<RoleType>
    {

        public readonly static RoleType None = new RoleType("None", 0);

        // dialog creation request
        public readonly static RoleType DialogCreationRequest = new RoleType("dialog creation request", 1);
        // dialog creation available responses
        public readonly static RoleType DialogCreationRequestAccepted = new RoleType("dialog creation request accepted", 2);
        public readonly static RoleType DialogCreationRequestDeclined = new RoleType("dialog creation request declined", 3);
        public readonly static RoleType DialogCreationRequestIgnored = new RoleType("dialog creation request ignored", 4);

        // notify incoming call request and available responses
        public readonly static RoleType NotifyIncomingCallRequestIgnored = new RoleType("dialog creation request ignored", 4);
        // notify incoming call available responses
        public readonly static RoleType NotifyIncomingCallRequestNotified = new RoleType("dialog creation request ignored", 4);
        public readonly static RoleType NotifyIncomingCallRequestNotSupported = new RoleType("dialog creation request ignored", 4);

        // general responses
        public readonly static RoleType Acknolaged = new RoleType("dialog creation request ignored", 4);
        public readonly static RoleType Accepted = new RoleType("dialog creation request ignored", 4);
        public readonly static RoleType Declined = new RoleType("dialog creation request ignored", 4);

        private RoleType(string name, int value) : base(name, value)
        {
        }
    }
}
